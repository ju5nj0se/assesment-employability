import api from './index';

export default {
    getAll(status) {
        console.log(`[API] Fetching courses with status: ${status || 'all'}`);
        return api.get('/courses', { params: { status } });
    },
    create(data) {
        console.log('[API] Creating course:', data);
        return api.post('/courses', data);
    },
    update(id, data) {
        console.log(`[API] Updating course ${id}:`, data);
        return api.put(`/courses/${id}`, data);
    },
    delete(id) {
        console.log(`[API] Deleting course ${id}`);
        return api.delete(`/courses/${id}`);
    },
    publish(id) {
        console.log(`[API] Publishing course ${id}`);
        return api.patch(`/courses/${id}/publish`);
    },
    unpublish(id) {
        console.log(`[API] Unpublishing course ${id}`);
        return api.patch(`/courses/${id}/unpublish`);
    }
};
