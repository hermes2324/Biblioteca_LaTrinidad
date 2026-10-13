<h1 align="center">📚 Biblioteca La Trinidad</h1>

<p align="center">
  Sistema de escritorio para administrar una biblioteca:<br />
  libros, inventario, préstamos, devoluciones y reportes, con acceso por roles.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="C#" />
  <img src="https://img.shields.io/badge/.NET%20Framework%204.8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET Framework 4.8" />
  <img src="https://img.shields.io/badge/Windows%20Forms-0078D6?style=for-the-badge&logo=windows&logoColor=white" alt="Windows Forms" />
  <img src="https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server" />
</p>

---

## ✨ Funciones

| 📖 Libros | 🔄 Préstamos | 👥 Usuarios |
|---|---|---|
| Agregar, actualizar y eliminar libros | Solicitar préstamos | Inicio de sesión de administrador y bibliotecario |
| Buscar por título, autor o categoría | Registrar devoluciones y el estado del libro | Alta de bibliotecarios con permisos |
| Consultar disponibilidad | Control de cantidad disponible | Registro de clientes |
| Generar inventario | Generar reportes | Recuperar contraseña |

- 🔒 Las contraseñas se guardan **cifradas con hash**, nunca en texto plano
- 🛡️ Consultas **parametrizadas** para evitar inyección SQL
- 🧩 Capa de datos separada en el proyecto `Conexiones` (libros, préstamos, clientes y bibliotecarios)

## 🚀 Cómo correrlo

1. Instala **Visual Studio** con *Desarrollo de escritorio de .NET* y **SQL Server** (Express sirve).
2. Crea la base de datos `BibliotecaDB` con las tablas `Libro1`, `Cliente`, `Bibliotecario`, `Prestamo`, `DetallePrestamo` y `Devolucion`.
3. Abre `Biblioteca_LaTrinidad.sln` y ejecuta. La conexión usa **autenticación de Windows** (`Integrated Security`), así que no hay contraseñas de base de datos en el código.

## 🗂️ Estructura

```text
├── Biblioteca_LaTrinidad/   ← formularios de Windows Forms (interfaz)
└── Conexiones/              ← acceso a SQL Server: libros, préstamos, clientes, bibliotecarios
```

## 👥 Equipo

Proyecto escolar del **TecNM Campus Guasave**, hecho por [Hermes Verdugo](https://github.com/hermes2324) y [Aaron Gaxiola](https://github.com/aron-alv).
