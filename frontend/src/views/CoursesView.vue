<script setup>
import { onMounted, ref } from 'vue';
import { useCourseStore } from '../stores/courses';
import { useAuthStore } from '../stores/auth';
import { useRouter } from 'vue-router';

const courseStore = useCourseStore();
const authStore = useAuthStore();
const router = useRouter();

const newCourseTitle = ref('');
const isEditing = ref(null);
const editTitle = ref('');
const notification = ref(null);
const currentFilter = ref(''); // '', 'draft', 'published'

const handleFilter = (filter) => {
  currentFilter.value = filter;
  courseStore.fetchCourses(filter);
};

const showNotification = (message, type = 'error') => {
  notification.value = { message, type };
  setTimeout(() => {
    notification.value = null;
  }, 5000);
};

onMounted(() => {
  console.log('[CoursesView] Mounted, fetching courses...');
  courseStore.fetchCourses();
});

const handleCreate = async () => {
  if (!newCourseTitle.value) return;
  console.log('[CoursesView] Creating new course:', newCourseTitle.value);
  const success = await courseStore.addCourse(newCourseTitle.value);
  if (!success) {
    showNotification(courseStore.error || 'Error al crear el curso');
  } else {
    newCourseTitle.value = '';
    courseStore.fetchCourses(currentFilter.value); // Refresh with current filter
  }
};

const handleEdit = (course) => {
  console.log('[CoursesView] Starting edit for:', course.id);
  isEditing.value = course.id;
  editTitle.value = course.title;
};

const handleUpdate = async (id) => {
  console.log('[CoursesView] Updating course:', id);
  const success = await courseStore.updateCourse(id, editTitle.value);
  if (success) {
    isEditing.value = null;
    courseStore.fetchCourses(currentFilter.value);
  } else {
    showNotification(courseStore.error || 'Error al actualizar el curso');
  }
};

const handleDelete = async (id) => {
  if (confirm('¿Estás seguro de eliminar este curso?')) {
    console.log('[CoursesView] Deleting course:', id);
    await courseStore.deleteCourse(id);
    courseStore.fetchCourses(currentFilter.value);
  }
};

const handleTogglePublish = async (course) => {
  const isPublished = course.statusId === 2; // Publicado
  console.log('[CoursesView] Toggling publish for:', course.id);
  const result = await courseStore.togglePublish(course.id, isPublished);
  if (!result.success) {
    showNotification(result.message);
  } else {
    courseStore.fetchCourses(currentFilter.value);
  }
};

const goToLessons = (courseId) => {
  console.log('[CoursesView] Navigating to lessons for:', courseId);
  router.push(`/dashboard/courses/${courseId}/lessons`);
};
</script>

<template>
  <div class="courses-container">
    <!-- Notifications -->
    <Transition name="fade">
      <div v-if="notification" :class="['notification', notification.type]">
        <span class="icon">{{ notification.type === 'error' ? '⚠️' : '✅' }}</span>
        <p>{{ notification.message }}</p>
        <button @click="notification = null" class="close-notify">&times;</button>
      </div>
    </Transition>

    <!-- Create Section (Admin Only) -->
    <div v-if="authStore.isAdmin" class="card create-card">
      <h3>Crear Nuevo Curso</h3>
      <form @submit.prevent="handleCreate" class="inline-form">
        <input 
          v-model="newCourseTitle" 
          type="text" 
          placeholder="Título del curso"
          required
        >
        <button type="submit" class="btn-primary">Crear</button>
      </form>
    </div>

    <!-- Filter Section -->
    <div class="filter-bar">
      <button 
        @click="handleFilter('')" 
        :class="['btn-filter', currentFilter === '' ? 'active' : '']"
      >
        Todos
      </button>
      <button 
        @click="handleFilter('published')" 
        :class="['btn-filter', currentFilter === 'published' ? 'active' : '']"
      >
        Publicados
      </button>
      <button 
        @click="handleFilter('draft')" 
        :class="['btn-filter', currentFilter === 'draft' ? 'active' : '']"
      >
        Borradores
      </button>
    </div>

    <!-- List Section -->
    <div class="courses-grid">
      <div v-for="course in courseStore.courses" :key="course.id" class="card course-card">
        <div v-if="isEditing === course.id" class="edit-mode">
          <input v-model="editTitle" type="text" class="edit-input">
          <div class="actions">
            <button @click="handleUpdate(course.id)" class="btn-success">Guardar</button>
            <button @click="isEditing = null" class="btn-ghost">Cancelar</button>
          </div>
        </div>
        
        <div v-else class="view-mode">
          <div class="header">
            <h4>{{ course.title }}</h4>
            <span :class="['badge', course.statusId === 2 ? 'published' : 'draft']">
              {{ course.statusId === 2 ? 'Publicado' : 'Borrador' }}
            </span>
          </div>
          
          <p class="meta">Creado: {{ new Date(course.createdAt).toLocaleDateString() }}</p>
          
          <div class="actions">
            <button @click="goToLessons(course.id)" class="btn-info">Lecciones</button>
            <template v-if="authStore.isAdmin">
              <button @click="handleTogglePublish(course)" class="btn-ghost">
                {{ course.statusId === 2 ? 'Despublicar' : 'Publicar' }}
              </button>
              <button @click="handleEdit(course)" class="btn-ghost" title="Editar">✏️</button>
              <button @click="handleDelete(course.id)" class="btn-danger-ghost" title="Eliminar">🗑️</button>
            </template>
          </div>
        </div>
      </div>
    </div>
    
    <div v-if="courseStore.loading" class="loading">Cargando cursos...</div>
  </div>
</template>

<style scoped>
.courses-container {
  display: flex;
  flex-direction: column;
  gap: 2rem;
  position: relative;
}

/* Notifications Styles */
.notification {
  position: fixed;
  top: 2rem;
  right: 2rem;
  z-index: 9999;
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1rem 1.5rem;
  background: white;
  border-radius: 1rem;
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
  border-left: 5px solid #e53e3e;
  max-width: 400px;
  animation: slideIn 0.3s ease-out;
}

.notification.error { border-left-color: #f56565; color: #c53030; background: #fff5f5; }
.notification.success { border-left-color: #48bb78; color: #276749; background: #f0fff4; }

.notification p {
  font-size: 0.95rem;
  font-weight: 500;
  margin: 0;
  flex: 1;
}

.close-notify {
  background: none;
  border: none;
  font-size: 1.5rem;
  color: currentColor;
  cursor: pointer;
  padding: 0;
  line-height: 1;
  opacity: 0.5;
}

.close-notify:hover { opacity: 1; }

@keyframes slideIn {
  from { transform: translateX(100%); opacity: 0; }
  to { transform: translateX(0); opacity: 1; }
}

.fade-enter-active, .fade-leave-active { transition: all 0.3s ease; }
.fade-enter-from, .fade-leave-to { opacity: 0; transform: translateY(-20px); }

.card {
  background: white;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.create-card h3 {
  margin-bottom: 1rem;
  font-size: 1.1rem;
  color: #4a5568;
}

.filter-bar {
  display: flex;
  gap: 1rem;
  margin-bottom: 1rem;
}

.btn-filter {
  background: white;
  color: #4a5568;
  border: 1px solid #e2e8f0;
  padding: 0.5rem 1.5rem;
  border-radius: 2rem;
  font-weight: 500;
  transition: all 0.2s;
}

.btn-filter:hover {
  border-color: #cbd5e0;
}

.btn-filter.active {
  background: #667eea;
  color: white;
  border-color: #667eea;
  box-shadow: 0 4px 6px -1px rgba(102, 126, 234, 0.4);
}

.inline-form {
  display: flex;
  gap: 1rem;
}

.inline-form input {
  flex: 1;
  padding: 0.75rem;
  border: 1px solid #e2e8f0;
  border-radius: 0.5rem;
}

.courses-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
}

.course-card {
  transition: transform 0.2s;
}

.course-card:hover {
  transform: translateY(-4px);
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 1rem;
}

.header h4 {
  font-size: 1.25rem;
  color: #2d3748;
}

.meta {
  font-size: 0.85rem;
  color: #718096;
  margin-bottom: 1.5rem;
}

.badge {
  padding: 0.25rem 0.75rem;
  border-radius: 1rem;
  font-size: 0.75rem;
  font-weight: 600;
}

.published { background: #c6f6d5; color: #22543d; }
.draft { background: #edf2f7; color: #4a5568; }

.actions {
  display: flex;
  gap: 0.5rem;
  flex-wrap: wrap;
}

button {
  padding: 0.5rem 1rem;
  border-radius: 0.5rem;
  font-size: 0.875rem;
  font-weight: 600;
  cursor: pointer;
  border: none;
  transition: all 0.2s;
}

.btn-primary { background: #667eea; color: white; }
.btn-primary:hover { background: #5a67d8; }

.btn-info { background: #ebf4ff; color: #4299e1; }
.btn-info:hover { background: #bee3f8; }

.btn-success { background: #48bb78; color: white; }
.btn-ghost { background: #f7fafc; color: #4a5568; }
.btn-ghost:hover { background: #edf2f7; }

.btn-danger-ghost { background: #fff5f5; color: #f56565; }
.btn-danger-ghost:hover { background: #fed7d7; }

.edit-mode { display: flex; flex-direction: column; gap: 1rem; }
.edit-input { padding: 0.5rem; border: 1px solid #e2e8f0; border-radius: 0.5rem; width: 100%; }

.loading { text-align: center; color: #718096; padding: 2rem; }
</style>
