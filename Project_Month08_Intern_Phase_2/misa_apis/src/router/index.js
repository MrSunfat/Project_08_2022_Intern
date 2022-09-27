import { createRouter, createWebHistory } from 'vue-router';
import TheMain from '@/views/main/TheMain.vue';
import TheDevelop from '@/views/main/TheDevelop.vue';
import UserPage from '@/views/pages/UserPage.vue';
import UserGroupPage from '@/views/pages/UserGroupPage.vue';
import SignPage from '@/views/pages/SignPage.vue';
import AuthoriPage from '@/views/pages/AuthoriPage.vue';

const routes = [
    {
        path: '/business/setting',
        component: TheMain,
        children: [
            {
                path: 'user',
                component: UserPage,
            },
            {
                path: 'user-group',
                component: UserGroupPage,
            },
            {
                path: 'signature',
                component: SignPage,
            },
            {
                path: 'authori',
                component: AuthoriPage,
            },
        ],
    },
    {
        path: '/business/overview',
        component: TheDevelop,
    },
    {
        path: '/business/execute',
        component: TheDevelop,
    },
    {
        path: '/business/vehicle',
        component: TheDevelop,
    },
    {
        path: '/business/advance',
        component: TheDevelop,
    },
    {
        path: '/business/payment',
        component: TheDevelop,
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;
