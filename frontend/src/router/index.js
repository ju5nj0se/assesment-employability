import { createRouter, createWebHistory } from 'vue-router';
import LoginView from '../views/LoginView.vue';
import RegisterView from '../views/RegisterView.vue';

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/login',
            name: 'login',
            component: LoginView,
            meta: { title: 'Iniciar Sesión' }
        },
        {
            path: '/register',
            name: 'register',
            component: RegisterView,
            meta: { title: 'Registro' }
        },
        {
            path: '/dashboard',
            component: () => import('../views/DashboardView.vue'),
            children: [
                {
                    path: '',
                    redirect: '/dashboard/courses'
                },
                {
                    path: 'courses',
                    name: 'courses',
                    component: () => import('../views/CoursesView.vue'),
                    meta: { title: 'Gestión de Cursos' }
                },
                {
                    path: 'courses/:courseId/lessons',
                    name: 'lessons',
                    component: () => import('../views/LessonsView.vue'),
                    meta: { title: 'Contenido del Curso' }
                }
            ]
        },
        {
            path: '/',
            redirect: '/login' // Cambiado de /dashboard a /login
        }
    ]
});

// Guard to check authentication
router.beforeEach((to, from, next) => {
    const token = localStorage.getItem('token');
    const user = localStorage.getItem('user');

    // Validación estricta del token
    const isAuthenticated = token && token !== 'undefined' && token !== 'null';

    console.log(`[Router] Navigation: ${from.path} -> ${to.path}`);
    console.log(`[Router] Auth State - Authenticated: ${!!isAuthenticated}`);

    const isAuthRoute = to.path === '/login' || to.path === '/register';
    const isDashboardRoute = to.path.startsWith('/dashboard');

    if (isDashboardRoute && !isAuthenticated) {
        console.warn('[Router] Access denied: Dashboard requires authentication. Redirecting to /login');
        next('/login');
    } else if (isAuthRoute && isAuthenticated) {
        console.log('[Router] Already authenticated. Redirecting to /dashboard');
        next('/dashboard');
    } else {
        next();
    }
});

export default router;
