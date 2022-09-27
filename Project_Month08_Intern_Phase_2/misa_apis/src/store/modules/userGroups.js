import axios from 'axios';
import { domain, userGroup } from '@/scripts/constants';
import { statusTagEnum } from '@/scripts/enum';

const userGroupsModule = {
    state: {
        userGroups: [],
        userGroup: {},
        userGroupCurrent: {
            UserGroupID: 0,
            UserGroupName: '',
            Description: '',
            Status: 1,
        },
        memberCheckInUserGroup: [],
        totalUserGroups: 0,
        totalPageUserGroups: 0,
        userGroupStart: 0,
        userGroupEnd: 0,
    },
    getters: {
        userGroups(state) {
            state.userGroups.sort((a, b) => a.SortOrder - b.SortOrder);
            return state.userGroups.map((userGroup) => ({
                ...userGroup,
                Status:
                    userGroup.Status === 1
                        ? statusTagEnum.Content.Use
                        : statusTagEnum.Content.InActive,
            }));
        },
        totalPageUserGroups(state) {
            return state.totalPageUserGroups;
        },
        totalUserGroups(state) {
            return state.totalUserGroups;
        },
        userGroupStart(state) {
            return state.userGroupStart;
        },
        userGroupEnd(state) {
            return state.userGroupEnd;
        },
        userGroup(state) {
            return {
                ...state.userGroup,
                Members: state.userGroup?.Members?.map((member) => ({
                    ...member,
                    Check: false,
                })),
                Status:
                    state.userGroup.Status === 1
                        ? statusTagEnum.Content.Use
                        : statusTagEnum.Content.InActive,
            };
        },
        userGroupCurrent(state) {
            return state.userGroupCurrent;
        },
        memberCheckInUserGroup(state) {
            return state.memberCheckInUserGroup;
        },
    },
    actions: {
        /**
         * Lấy dữ liệu người dùng
         * @param {*} param0
         * @param {*} idxPage
         * Author: TNDanh (7/9/2022)
         */
        async getUserGroups({ commit }, pageDetail) {
            try {
                const res = await axios.get(`${domain}/${userGroup}/Filter`, {
                    params: {
                        pageSize: pageDetail.pageSize,
                        pageNumber: pageDetail.pageNumber,
                        searchWord: pageDetail?.searchWord,
                    },
                });
                console.log(res);
                commit('setUserGroups', res.data.Data);
                commit('setTotalUserGroups', res.data.TotalRecord);
                commit('setTotalPageUserGroups', res.data.TotalPage);
                commit('setUserGroupStart', res.data.UserGroupStart);
                commit('setUserGroupEnd', res.data.UserGroupEnd);
                return res;
            } catch (error) {
                console.log(error);
            }
        },
        /**
         * Lấy dữ liệu nhóm người dùng
         * @param {*} param0
         * @param {*} userGroupID
         * @returns
         * Author: TNDanh (8/9/2022)
         */
        async getUserGroupByID({ commit }, userGroupDetail) {
            try {
                const res = await axios.get(
                    `${domain}/${userGroup}/Filter/${userGroupDetail?.userGroupID}/Members`,
                    {
                        params: {
                            searchWord: userGroupDetail?.searchWord,
                        },
                    }
                );
                commit('setUserGroup', res.data);
                return res;
            } catch (error) {
                console.log(error);
            }
        },
        /**
         * Gửi request xóa thành viên khỏi nhóm trên db
         * @param {*} param0
         * @param {*} memberDetail
         * @returns
         * Author: TNDanh (11/9/2022)
         */
        async deleteMemberInUserGroup({ commit }, memberDetail) {
            try {
                const res = await axios.delete(
                    `${domain}/${userGroup}/RemoveUserGroup/${memberDetail?.userGroupID}/Members`,
                    {
                        data: memberDetail?.memberIDs,
                    }
                );
                commit('');
                return res;
            } catch (error) {
                console.log(error);
            }
        },
        /**
         * Thêm thành viên vào nhóm người dùng
         * @param {*} param0
         * @param {*} memberDetail
         * @returns
         * Author: TNDanh (12/9/2022)
         */
        async addMembersForUserGroup({ commit }, memberDetail) {
            try {
                const res = await axios.post(
                    `${domain}/${userGroup}/AddUserGroup/${memberDetail?.userGroupID}/Members`,
                    memberDetail?.memberIDs,
                    {
                        headers: {
                            'Content-Type': 'application/json',
                        },
                    }
                );
                commit('');
                return res;
            } catch (error) {
                console.log(error);
            }
        },
    },
    mutations: {
        /**
         * Xét giá trị cho userGroups
         * @param {*} state
         * @param {*} userGroups
         * Author: TNDanh (8/9/2022)
         */
        setUserGroups(state, userGroups) {
            state.userGroups = userGroups;
        },
        /**
         * Xét giá trị cho userGroup
         * @param {*} state
         * @param {*} userGroup
         * Author: TNDanh (8/9/2022)
         */
        setUserGroup(state, userGroup) {
            state.userGroup = userGroup;
        },
        /**
         * Xét giá trị cho totalUserGroups
         * @param {*} state
         * @param {*} totalUserGroups
         * Author: TNDanh (8/9/2022)
         */
        setTotalUserGroups(state, totalUserGroups) {
            state.totalUserGroups = totalUserGroups;
        },
        /**
         * Xét giá trị cho totalPage
         * @param {*} state
         * @param {*} totalPage
         * Author: TNDanh (8/9/2022)
         */
        setTotalPageUserGroups(state, totalPageUserGroups) {
            state.totalPageUserGroups = totalPageUserGroups;
        },
        /**
         * Xét giá trị cho userGroupStart
         * @param {*} state
         * @param {*} userGroupStart
         * Author: TNDanh (8/9/2022)
         */
        setUserGroupStart(state, userGroupStart) {
            state.userGroupStart = userGroupStart;
        },
        /**
         * Xét giá trị cho userGroupEnd
         * @param {*} state
         * @param {*} userGroupEnd
         * Author: TNDanh (8/9/2022)
         */
        setUserGroupEnd(state, userGroupEnd) {
            state.userGroupEnd = userGroupEnd;
        },
        /**
         * Xét giá trị cho userGroupCurrent
         * @param {*} state
         * @param {*} userGroupCurrent
         * Author: TNDanh (11/9/2022)
         */
        setUserGroupCurrent(state, userGroupCurrent) {
            state.userGroupCurrent = userGroupCurrent;
        },
        /**
         * Loại bỏ thành viên ra nhóm người dùng
         * @param {*} state
         * @param {*} memberID
         * Author: TNDanh (11/9/2022)
         */
        setMembersInUserGroup(state, memberIDs) {
            state.userGroup.Members = state.userGroup.Members.filter(
                (member) => !memberIDs.includes(member.MemberID)
            );
        },
        /**
         * Xét giá trị của thành viên dã chọn trong nhóm người dùng
         * @param {*} state
         * @param {*} memberCheckInUserGroup
         * Author: TNDanh (14/9/2022)
         */
        setMemberCheckInUserGroup(state, memberCheckInUserGroup) {
            state.memberCheckInUserGroup = memberCheckInUserGroup;
        },
    },
};

export default userGroupsModule;
