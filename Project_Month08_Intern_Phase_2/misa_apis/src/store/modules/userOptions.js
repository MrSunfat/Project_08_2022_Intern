import { domain, columnOption } from '@/scripts/constants';
import axios from 'axios';

const userOptionsModule = {
    state: {
        columnOptions: [],
    },
    getters: {
        columnsOption(state) {
            return state.columnsOption;
        },
    },
    actions: {
        /**
         * Lấy về tùy chỉnh cột
         * @param {*} param0
         * @returns
         * Author: TNDanh (19/9/2022)
         */
        async getColumnsOption({ commit }) {
            try {
                const res = await axios.get(`${domain}/${columnOption}`);
                commit('setColumnOptions', res.data);
                return res;
            } catch (error) {
                console.log(error);
            }
        },
        /**
         * Xét giá mới cho bộ tùy chỉnh cột
         * @param {*} param0
         * @param {*} colums
         * @returns
         * Author: TNDanh (19/9/2022)
         */
        async updateColumnsOption({ commit }, colums) {
            try {
                const res = await axios.post(
                    `${domain}/${columnOption}/New`,
                    colums,
                    {
                        headers: {
                            'Content-Type': 'application/json',
                        },
                    }
                );

                commit('setColumnOptions', []);
                return res;
            } catch (error) {
                console.log(error);
            }
        },
        /**
         * Xét bộ tùy chọn mặc định
         * @param {*} param0
         * @returns
         * Author: TNDanh (19/9/2022)
         */
        async updateDefaultColumnsOption({ commit }) {
            try {
                const res = await axios.post(
                    `${domain}/${columnOption}/Default`
                );

                commit('setColumnOptions', []);
                return res;
            } catch (error) {
                console.log(error);
            }
        },
    },
    mutations: {
        /**
         * Xét giá trị mới cho tùy chỉnh cột
         * @param {*} state
         * @param {*} columnOptions
         * Author: TNDanh (19/9/2022)
         */
        setColumnOptions(state, columnOptions) {
            state.columnOptions = columnOptions;
        },
    },
};

export default userOptionsModule;
