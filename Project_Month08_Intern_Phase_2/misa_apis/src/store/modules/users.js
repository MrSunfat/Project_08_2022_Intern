import axios from 'axios';
import { domain, user, userGroup } from '@/scripts/constants';
import { statusTagEnum } from '@/scripts/enum';

const usersModule = {
    state: {
        users: [],
        user: {},
        userDetailForUserGroup: {
            UserID: '',
            UserGroups: [],
        },
        userGroupsTag: [],
        totalUsers: 0,
        totalPage: 0,
        userStart: 0,
        userEnd: 0,
        pageIndex: 1,
    },
    getters: {
        users(state) {
            return state.users.map((user) => ({
                ...user,
                Status:
                    user.Status === 3
                        ? statusTagEnum.Content.Work
                        : statusTagEnum.Content.InActive,
            }));
        },
        totalUsers(state) {
            return state.totalUsers;
        },
        totalPage(state) {
            return state.totalPage;
        },
        userStart(state) {
            return state.userStart;
        },
        userEnd(state) {
            return state.userEnd;
        },
        userID(state) {
            return state.user.userID;
        },
        user(state) {
            return state.user;
        },
        userGroupsTag(state) {
            return [
                {
                    UserGroupName: 'Tất cả',
                },
                ...state.userGroupsTag,
            ];
        },
        userGroupsTagW(state) {
            return state.userGroupsTag;
        },
        userDetailForUserGroup(state) {
            return state.userDetailForUserGroup;
        },
    },
    actions: {
        /**
         * Lấy dữ liệu người dùng
         * @param {*} param0
         * @param {*} idxPage
         * Author: TNDanh (7/9/2022)
         */
        async getUsers({ commit }, pageDetail) {
            try {
                const res = await axios.get(`${domain}/${user}/Filter`, {
                    params: {
                        pageSize: pageDetail.pageSize,
                        pageNumber: pageDetail.pageNumber,
                        searchWord: pageDetail?.searchWord,
                        userGroupName: pageDetail?.userGroupName,
                        jobTitle: pageDetail?.jobTitle,
                        organizationName: pageDetail?.organizationName,
                    },
                });

                commit('setUsers', res.data.Data);
                commit('setTotalUsers', res.data.TotalRecord);
                commit('setTotalPage', res.data.TotalPage);
                commit('setUserStart', res.data.UserStart);
                commit('setUserEnd', res.data.UserEnd);
                return res;
            } catch (error) {
                console.log(error);
            }
        },
        /**
         * Lấy dữ liệu user qua ID
         * @param {*} param0
         * @param {*} userID
         * Author: TNDanh (7/9/2022)
         */
        async getUserByID({ commit }, userID) {
            try {
                const res = await axios.get(`${domain}/${user}/${userID}`);
                // console.log(res);
                commit('setUser', res.data);
                return res;
            } catch (error) {
                console.log(error);
            }
        },
        /**
         * Lấy dữ liệu người dùng
         * @param {*} param0
         * @param {*} idxPage
         * Author: TNDanh (7/9/2022)
         */
        async getAllUserGroupTag({ commit }) {
            try {
                const res = await axios.get(`${domain}/${userGroup}`);
                commit('setUserGroupsTag', res.data);
                return res;
            } catch (error) {
                console.log(error);
            }
        },
        /**
         * Xóa dữ liệu người dùng qua id
         * @param {*} param0
         * @param {*} userID
         * @returns
         * Author: TNDanh (9/9/2022)
         */
        async deleteUserByID({ commit }, userID) {
            try {
                const res = await axios.delete(`${domain}/${user}/${userID}`);
                commit('');
                return res;
            } catch (error) {
                console.log(error);
            }
        },
        /**
         * Sửa đổi thuộc tính nhóm người dùng của user
         * @param {*} param0
         * @param {*} userDetail
         * @returns
         * Author: TNDanh (11/9/2022)
         */
        async editUserGroupInGroup({ commit }, userDetail) {
            try {
                const res = await axios.post(
                    `${domain}/${user}/EditUserGroupOfUser/${userDetail?.userID}`,
                    userDetail?.userGroups,
                    {
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                        },
                    }
                );
                commit('');
                return res;
            } catch (error) {
                console.log(error);
            }
        },
        /**
         * Thêm nhiều người dùng
         * @param {*} param0
         * @param {*} users
         * @returns
         * Author: TNDanh (19/9/2022)
         */
        async addUsers({ commit }, users) {
            try {
                const res = await axios.post(
                    `${domain}/${user}/AddUsers`,
                    users,
                    {
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
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
         * Xét giá trị cho users
         * @param {*} state
         * @param {*} users
         * Author: TNDanh (7/9/2022)
         */
        setUsers(state, users) {
            state.users = users;
        },
        /**
         * Xét giá trị cho totalUsers
         * Author: TNDanh (7/9/2022)
         */
        setTotalUsers(state, totalUsers) {
            state.totalUsers = totalUsers;
        },
        /**
         * Xét giá trị cho totalPage
         * Author: TNDanh (7/9/2022)
         */
        setTotalPage(state, totalPage) {
            state.totalPage = totalPage;
        },
        /**
         * Xét giá trị cho userStart
         * Author: TNDanh (7/9/2022)
         */
        setUserStart(state, userStart) {
            state.userStart = userStart;
        },
        /**
         * Xét giá trị cho userEnd
         * Author: TNDanh (7/9/2022)
         */
        setUserEnd(state, userEnd) {
            state.userEnd = userEnd;
        },
        /**
         * Xét ID của user cần quan tâm
         * Author: TNDanh (7/9/2022)
         */
        setUserID(state, userID) {
            state.user.userID = userID;
        },
        /**
         * Xét giá trị cho user
         * Author: TNDanh (7/9/2022)
         */
        setUser(state, user) {
            state.user = user;
        },
        /**
         * Xét giá trị cho userGroupsTag
         * @param {*} state
         * @param {*} userGroupsTag
         * Author: TNDanh (8/9/2022)
         */
        setUserGroupsTag(state, userGroupsTag) {
            state.userGroupsTag = userGroupsTag;
        },
        /**
         * Xét giá trị cho userDetailForUserGroup
         * @param {*} state
         * @param {*} userDetailForUserGroup
         * Author: TNDanh (9/9/2022)
         */
        setUserDetailForUserGroup(state, userDetailForUserGroup) {
            state.userDetailForUserGroup = userDetailForUserGroup;
        },
    },
};

export default usersModule;
