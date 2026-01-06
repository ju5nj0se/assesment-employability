<script setup>
import { useAuthStore } from '../stores/auth';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

const handleLogout = () => {
  console.log('[Dashboard] Logging out...');
  authStore.logout();
  // Forzamos recarga para limpiar estados internos de Vue/Router
  window.location.href = '/login';
};
</script>

<template>
  <div class="dashboard-layout">
    <aside class="sidebar">
      <div class="logo">
        <span class="icon">🚀</span>
        <h2>Assessment</h2>
      </div>
      
      <nav class="nav-links">
        <router-link to="/dashboard/courses" class="nav-item">
          <span class="icon">📚</span> Cursos
        </router-link>
      </nav>

      <div class="user-info">
        <div class="user-details">
          <div class="avatar">{{ authStore.user?.fullName?.charAt(0) }}</div>
          <div class="info">
            <p class="name">{{ authStore.user?.fullName }}</p>
            <p class="role">{{ authStore.user?.roles?.[0] }}</p>
          </div>
        </div>
        <button @click="handleLogout" class="btn-logout" title="Cerrar sesión">
          <span class="icon">🚪</span>
          <span class="text">Salir</span>
        </button>
      </div>
    </aside>

    <main class="content">
      <header class="top-bar">
        <h1>{{ $route.meta.title || 'Dashboard' }}</h1>
      </header>
      
      <div class="page-content">
        <RouterView />
      </div>
    </main>
  </div>
</template>

<style scoped>
.dashboard-layout {
  display: flex;
  height: 100vh;
  background-color: #f0f2f5;
}

.sidebar {
  width: 260px;
  background: white;
  box-shadow: 2px 0 10px rgba(0,0,0,0.05);
  display: flex;
  flex-direction: column;
  z-index: 10;
}

.logo {
  padding: 2rem;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  color: #4a5568;
}

.logo h2 {
  font-size: 1.25rem;
}

.nav-links {
  flex: 1;
  padding: 1rem;
}

.nav-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 0.75rem 1rem;
  color: #4a5568;
  text-decoration: none;
  border-radius: 0.75rem;
  margin-bottom: 0.5rem;
  transition: all 0.2s;
  font-weight: 500;
}

.nav-item:hover, .nav-item.router-link-active {
  background: #f7fafc;
  color: #667eea;
}

.nav-item.router-link-active {
  background: #ebf4ff;
}

.user-info {
  padding: 1.5rem;
  border-top: 1px solid #edf2f7;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.user-details {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.avatar {
  width: 40px;
  height: 40px;
  background: #667eea;
  color: white;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  flex-shrink: 0;
}

.info {
  flex: 1;
  overflow: hidden;
}

.name {
  font-weight: 600;
  font-size: 0.9rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  color: #2d3748;
}

.role {
  font-size: 0.75rem;
  color: #718096;
}

.btn-logout {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  width: 100%;
  padding: 0.6rem;
  background: #fff5f5;
  color: #f56565;
  border: 1px solid #fed7d7;
  border-radius: 0.75rem;
  font-size: 0.9rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-logout:hover {
  background: #f56565;
  color: white;
}

.content {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.top-bar {
  padding: 1.5rem 2.5rem;
  background: white;
  border-bottom: 1px solid #edf2f7;
}

.top-bar h1 {
  font-size: 1.5rem;
  color: #2d3748;
}

.page-content {
  flex: 1;
  padding: 2.5rem;
  overflow-y: auto;
}
</style>
