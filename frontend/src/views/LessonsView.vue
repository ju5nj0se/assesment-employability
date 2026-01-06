<script setup>
import { onMounted, onUnmounted, ref } from 'vue';
import { useLessonStore } from '../stores/lessons';
import { useAuthStore } from '../stores/auth';
import { useRoute, onBeforeRouteLeave } from 'vue-router';
import { watch } from 'vue';

const lessonStore = useLessonStore();
const authStore = useAuthStore();
const route = useRoute();

const courseId = route.params.courseId;
const newLessonTitle = ref('');
const isEditing = ref(null);
const editTitle = ref('');
const notification = ref(null);
const localLessons = ref([]);
const hasChanges = ref(false);

const showNotification = (message, type = 'error') => {
  notification.value = { message, type };
  setTimeout(() => {
    notification.value = null;
  }, 5000);
};

onMounted(async () => {
  console.log(`[LessonsView] Mounted for course ${courseId}, fetching lessons...`);
  await lessonStore.fetchLessons(courseId);
  localLessons.value = [...lessonStore.lessons].sort((a, b) => a.order - b.order);
});

// Watch store for changes (like after adding/deleting)
watch(() => lessonStore.lessons, (newVal) => {
  if (!hasChanges.value) {
    localLessons.value = [...newVal].sort((a, b) => a.order - b.order);
  }
}, { deep: true });

onBeforeRouteLeave((to, from, next) => {
  if (hasChanges.value) {
    const answer = window.confirm('Tienes cambios sin guardar en el orden de las lecciones. ¿Seguro que quieres salir?');
    if (answer) next();
    else next(false);
  } else {
    next();
  }
});

// Browser tab close protection
watch(hasChanges, (val) => {
  if (val) {
    window.onbeforeunload = () => 'Tienes cambios sin guardar. ¿Seguro que quieres salir?';
  } else {
    window.onbeforeunload = null;
  }
});

onUnmounted(() => {
  window.onbeforeunload = null;
});

const handleCreate = async () => {
  if (!newLessonTitle.value) return;
  console.log('[LessonsView] Creating new lesson:', newLessonTitle.value);
  const result = await lessonStore.addLesson(courseId, newLessonTitle.value);
  if (result.success) {
    newLessonTitle.value = '';
    showNotification('Lección creada con éxito', 'success');
  } else {
    showNotification(result.message);
  }
};

const handleEdit = (lesson) => {
  console.log('[LessonsView] Starting edit for:', lesson.id);
  isEditing.value = lesson.id;
  editTitle.value = lesson.title;
};

const handleUpdate = async (id) => {
  console.log('[LessonsView] Updating lesson:', id);
  const result = await lessonStore.updateLesson(id, courseId, editTitle.value);
  if (result.success) {
    isEditing.value = null;
    showNotification('Lección actualizada', 'success');
  } else {
    showNotification(result.message);
  }
};

const handleDelete = async (id) => {
  if (confirm('¿Estás seguro de eliminar esta lección?')) {
    console.log('[LessonsView] Deleting lesson:', id);
    const result = await lessonStore.deleteLesson(id, courseId);
    if (result.success) {
      showNotification('Lección eliminada', 'success');
      hasChanges.value = false; // Reset changes on delete to avoid confusion
    } else {
      showNotification(result.message);
    }
  }
};

// Drag & Drop logic
let draggedItemIndex = null;

const onDragStart = (index) => {
  if (!authStore.isAdmin) return;
  draggedItemIndex = index;
};

const onDragOver = (event) => {
  event.preventDefault();
};

const onDrop = (targetIndex) => {
  if (!authStore.isAdmin || draggedItemIndex === null) return;
  
  const movedItem = localLessons.value.splice(draggedItemIndex, 1)[0];
  localLessons.value.splice(targetIndex, 0, movedItem);
  
  // Re-assign order numbers locally
  localLessons.value.forEach((lesson, idx) => {
    lesson.order = idx + 1;
  });
  
  hasChanges.value = true;
  draggedItemIndex = null;
};

const handleSaveOrder = async () => {
  console.log('[LessonsView] Saving new order...');
  const items = localLessons.value.map((l, index) => ({
    id: l.id,
    order: index + 1
  }));
  
  const result = await lessonStore.reorderLessons(courseId, items);
  if (result.success) {
    showNotification('Orden guardado correctamente', 'success');
    hasChanges.value = false;
  } else {
    showNotification(result.message);
  }
};
</script>

<template>
  <div class="lessons-container">
    <div class="header-actions">
      <router-link to="/dashboard/courses" class="btn-back">⬅️ Volver a cursos</router-link>
      <button 
        v-if="authStore.isAdmin && hasChanges" 
        @click="handleSaveOrder" 
        class="btn-save-order"
        :disabled="lessonStore.loading"
      >
        💾 Guardar Orden
      </button>
    </div>

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
      <h3>Agregar Lección</h3>
      <form @submit.prevent="handleCreate" class="inline-form">
        <input 
          v-model="newLessonTitle" 
          type="text" 
          placeholder="Título de la lección"
          required
        >
        <button type="submit" class="btn-primary">Agregar</button>
      </form>
    </div>

    <!-- List Section -->
    <div class="lessons-list">
      <div 
        v-for="(lesson, index) in localLessons" 
        :key="lesson.id" 
        class="lesson-item card"
        :draggable="authStore.isAdmin"
        @dragstart="onDragStart(index)"
        @dragover="onDragOver"
        @drop="onDrop(index)"
        :class="{ 'is-draggable': authStore.isAdmin }"
      >
        <div v-if="isEditing === lesson.id" class="edit-mode">
          <input v-model="editTitle" type="text" class="edit-input">
          <div class="actions">
            <button @click="handleUpdate(lesson.id)" class="btn-success">Guardar</button>
            <button @click="isEditing = null" class="btn-ghost">Cancelar</button>
          </div>
        </div>
        
        <div v-else class="view-mode">
          <div class="lesson-info">
            <span class="order">{{ lesson.order }}</span>
            <h4>{{ lesson.title }}</h4>
          </div>
          
          <div v-if="authStore.isAdmin" class="actions">
            <span class="drag-handle">≡</span>
            <button @click="handleEdit(lesson)" class="btn-ghost" title="Editar">✏️</button>
            <button @click="handleDelete(lesson.id)" class="btn-danger-ghost" title="Eliminar">🗑️</button>
          </div>
        </div>
      </div>
      
      <div v-if="localLessons.length === 0 && !lessonStore.loading" class="empty">
        No hay lecciones en este curso todavía.
      </div>
    </div>
    
    <div v-if="lessonStore.loading" class="loading">Cargando lecciones...</div>
  </div>
</template>

<style scoped>
.lessons-container {
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

.header-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.btn-back {
  text-decoration: none;
  color: #667eea;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.btn-save-order {
  background: #4a5568;
  color: white;
  padding: 0.6rem 1.25rem;
  border-radius: 0.75rem;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  box-shadow: 0 4px 6px -1px rgba(0,0,0,0.1);
  animation: pulse 2s infinite;
}

@keyframes pulse {
  0% { transform: scale(1); }
  50% { transform: scale(1.02); box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4); }
  100% { transform: scale(1); }
}

.card {
  background: white;
  padding: 1.25rem;
  border-radius: 1rem;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
  transition: all 0.2s;
}

.lesson-item.is-draggable {
  cursor: grab;
}

.lesson-item.is-draggable:active {
  cursor: grabbing;
}

.lesson-item:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
}

.drag-handle {
  color: #a0aec0;
  font-size: 1.5rem;
  margin-right: 0.5rem;
  user-select: none;
}

.create-card h3 {
  margin-bottom: 1rem;
  font-size: 1.1rem;
  color: #4a5568;
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

.lessons-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.lesson-item {
  display: flex;
  flex-direction: column;
}

.view-mode {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.lesson-info {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.order {
  background: #ebf4ff;
  color: #4299e1;
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  font-weight: bold;
  font-size: 0.8rem;
}

.actions {
  display: flex;
  gap: 0.5rem;
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
.btn-success { background: #48bb78; color: white; }
.btn-ghost { background: #f7fafc; color: #4a5568; }
.btn-ghost:hover { background: #edf2f7; }

.btn-danger-ghost { background: #fff5f5; color: #f56565; }
.btn-danger-ghost:hover { background: #fed7d7; }

.edit-mode { display: flex; flex-direction: column; gap: 1rem; }
.edit-input { padding: 0.5rem; border: 1px solid #e2e8f0; border-radius: 0.5rem; width: 100%; }

.empty { text-align: center; color: #718096; padding: 2rem; background: #f7fafc; border-radius: 1rem; border: 2px dashed #e2e8f0; }
.loading { text-align: center; color: #718096; padding: 2rem; }
</style>
