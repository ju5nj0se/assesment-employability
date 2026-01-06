import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5000/api',
    headers: {
        'Content-Type': 'application/json',
    },
});

// Request Interceptor
api.interceptors.request.use((config) => {
    console.log(`🚀 [API Request] ${config.method.toUpperCase()} ${config.url}`, config.data || '');
    const token = localStorage.getItem('token');
    if (token && token !== 'undefined' && token !== 'null') {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
}, (error) => {
    console.error('❌ [API Request Error]', error);
    return Promise.reject(error);
});

// Response Interceptor
api.interceptors.response.use((response) => {
    console.log(`✅ [API Response] ${response.status} ${response.config.url}`, response.data);
    return response;
}, (error) => {
    console.group('❌ [API Error Detail]');
    console.error('Status:', error.response?.status);
    console.error('URL:', error.config?.url);
    console.error('Message:', error.message);
    console.error('Data:', error.response?.data);
    console.groupEnd();
    return Promise.reject(error);
});

export default api;
