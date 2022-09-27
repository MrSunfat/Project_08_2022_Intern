import axios from 'axios';
import { domain, jobTitle } from '@/scripts/constants';
const jobTitlesModule = {
    state: {
        jobTitles: [],
    },
    getters: {
        jobTitles(state) {
            return state.jobTitles;
        },
    },
    actions: {
        /**
         * Lấy tất cả thông tin chức vụ
         * @param {*} param0
         * @returns
         * Author: TNDanh (13/9/2022)
         */
        async getJobTitles({ commit }) {
            try {
                let res = await axios.get(`${domain}/${jobTitle}`);
                commit('setJobTitles', res.data);
                return res;
            } catch (error) {
                console.log(error);
            }
        },
    },
    mutations: {
        /**
         * Xét giá trị cho jobTitles
         * Author: TNDanh (13/9/2022)
         */
        setJobTitles(state, jobTitles) {
            state.jobTitles = jobTitles;
        },
    },
};

export default jobTitlesModule;
