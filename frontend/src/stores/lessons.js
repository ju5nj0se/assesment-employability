import { defineStore } from 'pinia';
import lessonsApi from '../api/lessons';

export const useLessonStore = defineStore('lessons', {
    state: () => ({
        lessons: [],
        loading: false,
        error: null,
    }),
    actions: {
        async fetchLessons(courseId) {
            if (!courseId) return;
            this.loading = true;
            this.error = null;
            console.log(`[Store] Fetching lessons for course ${courseId}...`);
            try {
                const response = await lessonsApi.getByCourse(courseId);
                this.lessons = response.data.data;
                console.log('[Store] Lessons loaded:', this.lessons);
            } catch (err) {
                const message = err.response?.data?.message || 'Error al cargar lecciones';
                this.error = message;
                console.error('[Store] Fetch lessons error:', message);
            } finally {
                this.loading = false;
            }
        },
        async addLesson(courseId, title) {
            this.loading = true;
            console.log(`[Store] Adding lesson to content ${courseId}:`, title);
            try {
                await lessonsApi.create({ courseId, title });
                await this.fetchLessons(courseId);
                return { success: true };
            } catch (err) {
                const message = err.response?.data?.message || 'Error al crear lección';
                console.error('[Store] Add lesson error:', message);
                return { success: false, message };
            } finally {
                this.loading = false;
            }
        },
        async updateLesson(id, courseId, title) {
            this.loading = true;
            console.log(`[Store] Updating lesson ${id}:`, title);
            try {
                await lessonsApi.update(id, { courseId, title });
                await this.fetchLessons(courseId);
                return { success: true };
            } catch (err) {
                const message = err.response?.data?.message || 'Error al actualizar lección';
                console.error('[Store] Update lesson error:', message);
                return { success: false, message };
            } finally {
                this.loading = false;
            }
        },
        async deleteLesson(id, courseId) {
            console.log(`[Store] Deleting lesson ${id}`);
            try {
                await lessonsApi.delete(id);
                await this.fetchLessons(courseId);
                return { success: true };
            } catch (err) {
                const message = err.response?.data?.message || 'Error al eliminar lección';
                console.error('[Store] Delete lesson error:', message);
                return { success: false, message };
            }
        },
        async reorderLessons(courseId, items) {
            this.loading = true;
            console.log('[Store] Reordering lessons:', items);
            try {
                await lessonsApi.reorder(items);
                await this.fetchLessons(courseId);
                return { success: true };
            } catch (err) {
                const message = err.response?.data?.message || 'Error al reordenar lecciones';
                console.error('[Store] Reorder lessons error:', message);
                return { success: false, message };
            } finally {
                this.loading = false;
            }
        }
    }
});
