import { createStore } from 'vuex';
import usersModule from './modules/users';
import userGroupsModule from './modules/userGroups';
import jobTitlesModule from './modules/jobTitles';
import userOptionsModule from './modules/userOptions';

const storeData = {
    modules: {
        usersModule,
        userGroupsModule,
        jobTitlesModule,
        userOptionsModule,
    },
};

const store = createStore(storeData);
export default store;
