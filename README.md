# Assessment Employability - Plataforma de Gestión de Cursos

Este proyecto es una solución integral para la gestión de cursos y lecciones, diseñada bajo estándares modernos de desarrollo. Combina una arquitectura robusta en el backend con una interfaz de usuario dinámica y premium.

## 🚀 Descripción del Proyecto

Assessment Employability permite a los administradores gestionar un catálogo de cursos, organizar lecciones mediante un sistema visual de arrastrar y soltar, y controlar la visibilidad del contenido para los usuarios finales.

### Funcionalidades Clave:
- **Gestión de Cursos**: Creación, actualización (título), borrado lógico y sistema de publicación.
- **Control de Lecciones**: CRUD completo de lecciones vinculado a cursos.
- **Drag & Drop Reordering**: Sistema interactivo para ordenar lecciones con guardado persistente.
- **Control de Acceso basado en Roles (RBAC)**: Diferenciación clara entre Administradores y Usuarios.
- **Seguridad**: Autenticación y Autorización mediante JWT (JSON Web Tokens).

## 🏗️ Arquitectura Técnica

- **Backend**: .NET 8 con **Clean Architecture** (Domain, Application, Infrastructure, API).
- **Frontend**: Vue.js 3 con Composition API, **Pinia** para el estado y **Vite** para el build.
- **Base de Datos**: PostgreSQL 15.
- **Infraestructura**: Containerización completa con Docker y Docker Compose.
- **Admin Tools**: Integración de **pgAdmin 4** para inspección directa de datos.

## 🛠️ Cómo Desplegar el Proyecto

La forma más rápida y recomendada es utilizar **Docker Compose**. Asegúrate de estar en la raíz del proyecto y ejecuta:

```bash
docker compose up --build -d
```

### URLs de Acceso:
- **Frontend (Web App)**: [http://localhost:3000](http://localhost:3000)
- **Backend (Swagger UI)**: [http://localhost:5000/swagger](http://localhost:5000/swagger)
- **pgAdmin (DB Manager)**: [http://localhost:8080](http://localhost:8080) (Email: `admin@admin.com`, Pass: `admin`)

---

## 🔐 Credenciales y Roles

El sistema cuenta con dos roles principales que determinan qué acciones puede realizar un usuario:

### 1. Rol: Administrador (Admin)
Tiene control total sobre la plataforma (Crear, Editar, Eliminar, Publicar, Reordenar).
- **Email**: `admin@gmail.com`
- **Contraseña**: `admin123`

### 2. Rol: Usuario (User)
Rol de consumo. Solo puede visualizar los cursos disponibles y sus lecciones. No puede modificar el contenido.
- **Acceso**: Cualquier usuario que se registre manualmente a través de la opción "Registrarse" en el login obtendrá el rol de `User` por defecto.

---

## 📂 Estructura del Repositorio

- `/`: Configuración global de Docker y Documentación general.
- `/backend`: Lógica de API, Servicios y Persistencia de datos.
- `/frontend`: Código fuente de la interfaz, estilos premium y lógica de cliente.


