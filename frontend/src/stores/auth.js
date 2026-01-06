import { defineStore } from 'pinia';
import api from '../api';

export const useAuthStore = defineStore('auth', {
    state: () => ({
        user: JSON.parse(localStorage.getItem('user')) || null,
        token: localStorage.getItem('token') || null,
        loading: false,
        error: null,
    }),
    getters: {
        isAdmin: (state) => state.user?.roles?.includes('Admin'),
    },
    actions: {
        async login(email, password) {
            this.loading = true;
            this.error = null;
            console.log(`[Store] Fetching login API for: ${email}`);
            try {
                const response = await api.post('/auth/login', { email, password });
                console.log('[Store] API Success, result:', response.data);
                this.user = {
                    email: response.data.email,
                    fullName: response.data.fullName,
                    roles: response.data.roles
                };
                this.token = response.data.token;
                localStorage.setItem('user', JSON.stringify(this.user));
                localStorage.setItem('token', this.token);
                console.log('[Store] LocalStorage updated successfully');
                return true;
            } catch (err) {
                console.error('[Store] Auth error:', err);
                this.error = err.response?.data?.message || 'Error en el login';
                return false;
            } finally {
                this.loading = false;
            }
        },
        async register(fullName, email, document, password) {
            this.loading = true;
            this.error = null;
            try {
                await api.post('/auth/register', { fullName, email, document, password });
                return true;
            } catch (err) {
                this.error = err.response?.data?.message || 'Error en el registro';
                return false;
            } finally {
                this.loading = false;
            }
        },
        logout() {
            this.user = null;
            this.token = null;
            localStorage.removeItem('user');
            localStorage.removeItem('token');
        },
    },
});
