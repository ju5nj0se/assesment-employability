import { defineStore } from 'pinia';
import coursesApi from '../api/courses';

export const useCourseStore = defineStore('courses', {
    state: () => ({
        courses: [],
        loading: false,
        error: null,
    }),
    actions: {
        async fetchCourses(status) {
            this.loading = true;
            console.log('[Store] Fetching courses...');
            try {
                const response = await coursesApi.getAll(status);
                this.courses = response.data.data;
                console.log('[Store] Courses loaded:', this.courses);
            } catch (err) {
                this.error = 'Error fetching courses';
                console.error('[Store] Fetch courses error:', err);
            } finally {
                this.loading = false;
            }
        },
        async addCourse(title) {
            console.log('[Store] Adding course:', title);
            try {
                await coursesApi.create({ title });
                await this.fetchCourses();
                return true;
            } catch (err) {
                console.error('[Store] Add course error:', err);
                return false;
            }
        },
        async updateCourse(id, title) {
            console.log('[Store] Updating course:', id, title);
            try {
                await coursesApi.update(id, { title });
                await this.fetchCourses();
                return true;
            } catch (err) {
                console.error('[Store] Update course error:', err);
                return false;
            }
        },
        async deleteCourse(id) {
            console.log('[Store] Deleting course:', id);
            try {
                await coursesApi.delete(id);
                await this.fetchCourses();
            } catch (err) {
                console.error('[Store] Delete course error:', err);
            }
        },
        async togglePublish(id, isPublished) {
            this.loading = true;
            this.error = null;
            console.log(`[Store] Toggling publish for ${id}: ${isPublished}`);
            try {
                if (isPublished) {
                    await coursesApi.unpublish(id);
                } else {
                    await coursesApi.publish(id);
                }
                await this.fetchCourses();
                return { success: true };
            } catch (err) {
                const message = err.response?.data?.message || 'Error al cambiar estado del curso';
                this.error = message;
                console.error('[Store] Toggle publish error:', message);
                return { success: false, message };
            } finally {
                this.loading = false;
            }
        }
    }
});
