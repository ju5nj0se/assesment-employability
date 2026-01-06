# Assessment Employability - Frontend

Este es el cliente web del proyecto Assessment Employability, una aplicación modular tipo Dashboard construida con **Vue.js 3** y **Vite**.

## 🚀 Tecnologías Principales

- **Vue 3** (Composition API)
- **Vite** (Build tool de última generación)
- **Pinia** (Gestión de estado global)
- **Vue Router** (Sistema de rutas)
- **Axios** (Peticiones HTTP con interceptores para logging y JWT)

## ✨ Características Implementadas

- **Autenticación Completa**: Login, Registro y Logout con limpieza de estado.
- **Gestión de Cursos**: Crear, editar, eliminar y cambiar estado (Publicar/Despublicar) de cursos.
- **Lecciones Interactivas**:
    - CRUD completo de lecciones por curso.
    - **Reordenamiento interactivo**: Sistema Drag & Drop para organizar las lecciones.
- **Control de Acceso (RBAC)**: Visibilidad de botones y controles restringida según el rol del usuario (Admin vs User).
- **Sistema de Notificaciones**: Toasts animados para feedback inmediato de operaciones.
- **Protección de Datos**: Alertas de navegación si intentas salir con cambios sin guardar en el orden de las lecciones.

## 📦 Instalación y Configuración

### Prerrequisitos
- **Node.js** (v20 o superior)

### Ejecución Local
1. Instalar dependencias:
```bash
npm install
```
2. Ejecutar en modo desarrollo:
```bash
npm run dev
```

La aplicación estará disponible en `http://localhost:3000`.

## 🐳 Docker Deployment

Para levantar el frontend dentro de la red del proyecto:
```bash
docker compose up -d frontend
```
El contenedor utiliza **Nginx** para servir los archivos estáticos generados en el build.

## 🛡️ Seguridad

La aplicación utiliza interceptores de Axios para adjuntar el token JWT en el encabezado `Authorization` de cada petición. La persistencia de la sesión se maneja mediante `localStorage`.
