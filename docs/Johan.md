# Johan Manuel Vicente Berroa

## Issue #1 — Reorganización del repositorio y estructura de carpetas

### ¿Qué se hizo?
Se limpió y reorganizó el repositorio para que todos los miembros del equipo puedan trabajar con una estructura consistente desde el inicio. Se eliminó la carpeta `proyecto_final_GrupoB/` que existía por defecto y se estableció la estructura mínima requerida por el profesor.

### Estructura final del repositorio

RouteX GPS RD/
├── src/          → código fuente del proyecto (Program.cs, algoritmos, modelos)
├── tests/        → proyecto xUnit con todas las pruebas unitarias
├── docs/         → documentación individual de cada miembro
├── metrics/      → resultados de comparativas de rendimiento
├── .gitignore    → excluye bin/, obj/, .vs/ y archivos de build
└── .sln          → solución que apunta a /src y /tests

### Decisiones tomadas
- Se eliminó `proyecto_final_GrupoB/` porque el código fuente debe vivir en `/src` según los requisitos del profesor.
- Se creó el proyecto xUnit en `/tests` desde este issue para que todos los miembros puedan agregar sus pruebas sin bloqueos.
- El `.gitignore` cubre `bin/`, `obj/`, `.vs/`, `*.dll`, `*.exe` y output de publish para evitar subir archivos compilados al repositorio.
- El archivo `.sln` fue verificado para que apunte correctamente a ambos proyectos (`/src` y `/tests`).