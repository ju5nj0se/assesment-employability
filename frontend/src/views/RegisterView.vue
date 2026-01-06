<script setup>
import { ref } from 'vue';
import { useAuthStore } from '../stores/auth';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

const fullName = ref('');
const email = ref('');
const document = ref('');
const password = ref('');

const handleRegister = async () => {
  console.log('[RegisterView] Form submitted, calling store...');
  const success = await authStore.register(
    fullName.value, 
    email.value, 
    document.value, 
    password.value
  );
  if (success) {
    console.log('[RegisterView] Registration success, redirecting to login...');
    router.push('/login');
  } else {
    console.error('[RegisterView] Registration failed:', authStore.error);
  }
};
</script>

<template>
  <div class="auth-container">
    <div class="auth-card">
      <h1>Crea tu cuenta</h1>
      <p class="subtitle">Únete a nuestra plataforma</p>
      
      <form @submit.prevent="handleRegister">
        <div class="form-group">
          <label>Nombre Completo</label>
          <input 
            v-model="fullName" 
            type="text" 
            placeholder="Juan Pérez" 
            required
          >
        </div>

        <div class="form-group">
          <label>Email</label>
          <input 
            v-model="email" 
            type="email" 
            placeholder="correo@ejemplo.com" 
            required
          >
        </div>

        <div class="form-group">
          <label>Documento de Identidad</label>
          <input 
            v-model="document" 
            type="text" 
            placeholder="12345678" 
            required
          >
        </div>
        
        <div class="form-group">
          <label>Contraseña</label>
          <input 
            v-model="password" 
            type="password" 
            placeholder="••••••••" 
            minlength="6"
            required
          >
        </div>

        <div v-if="authStore.error" class="error-message">
          {{ authStore.error }}
        </div>

        <button :disabled="authStore.loading" type="submit" class="btn-primary">
          {{ authStore.loading ? 'Creando cuenta...' : 'Registrarse' }}
        </button>
      </form>

      <p class="auth-footer">
        ¿Ya tienes cuenta? 
        <router-link to="/login">Inicia sesión</router-link>
      </p>
    </div>
  </div>
</template>

<style scoped>
.auth-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 1rem;
}

.auth-card {
  background: rgba(255, 255, 255, 0.95);
  padding: 2.5rem;
  border-radius: 1.5rem;
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.2);
  width: 100%;
  max-width: 450px;
  backdrop-filter: blur(10px);
}

h1 {
  margin-bottom: 0.5rem;
  color: #2d3748;
  font-size: 2rem;
  text-align: center;
}

.subtitle {
  color: #718096;
  text-align: center;
  margin-bottom: 2rem;
}

.form-group {
  margin-bottom: 1.25rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  color: #4a5568;
  font-weight: 500;
}

input {
  width: 100%;
  padding: 0.75rem 1rem;
  border: 1px solid #e2e8f0;
  border-radius: 0.75rem;
  font-size: 1rem;
  transition: all 0.3s;
}

input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.btn-primary {
  width: 100%;
  padding: 0.75rem;
  background: #667eea;
  color: white;
  border: none;
  border-radius: 0.75rem;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.3s;
  margin-top: 1rem;
}

.btn-primary:hover:not(:disabled) {
  background: #5a67d8;
}

.btn-primary:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.error-message {
  color: #e53e3e;
  background: #fff5f5;
  padding: 0.75rem;
  border-radius: 0.5rem;
  margin-bottom: 1rem;
  font-size: 0.875rem;
  text-align: center;
}

.auth-footer {
  margin-top: 1.5rem;
  text-align: center;
  color: #718096;
}

.auth-footer a {
  color: #667eea;
  text-decoration: none;
  font-weight: 600;
}

.auth-footer a:hover {
  text-decoration: underline;
}
</style>
