import api from './index';

export default {
    getByCourse(courseId) {
        console.log(`[API] Fetching lessons for course: ${courseId}`);
        return api.get('/lessons', { params: { courseId } });
    },
    create(data) {
        console.log('[API] Creating lesson:', data);
        return api.post('/lessons', data);
    },
    update(id, data) {
        console.log(`[API] Updating lesson ${id}:`, data);
        return api.put(`/lessons/${id}`, data);
    },
    delete(id) {
        console.log(`[API] Deleting lesson ${id}`);
        return api.delete(`/lessons/${id}`);
    },
    reorder(items) {
        console.log('[API] Reordering lessons:', items);
        return api.post('/lessons/reorder', items);
    }
};
