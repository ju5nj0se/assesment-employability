import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import router from './router'

const app = createApp(App)

app.config.errorHandler = (err, vm, info) => {
    console.group('🚨 [Vue Global Error]');
    console.error('Error:', err);
    console.error('Vue Component:', vm);
    console.error('Info:', info);
    console.groupEnd();
};

app.use(createPinia())
app.use(router)

app.mount('#app')
