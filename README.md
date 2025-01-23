# Unity Project: Movement and Animation System 

## 📖 Descripción
Este proyecto de Unity está diseñado para demostrar un sistema de movimiento en primera persona que incluye interacciones básicas como caminar, correr, saltar y animaciones. También cuenta con un **sistema de doble cuerda (grappling hook)**, inspirado en el **sistema de maniobras tridimensionales de Attack on Titan (AOT)**, para engancharse a objetos etiquetados como `Grappable`.

El objetivo es proporcionar una base para juegos o simulaciones en 3D donde se requiera un personaje en primera persona con un sistema de movimiento, animación y mecánicas avanzadas como el grappling hook.

---

## 🛠️ Características
- **Movimiento del personaje**:
  - Caminar y correr con detección de la tecla **Shift**.
  - Saltar cuando el personaje está en contacto con objetos etiquetados como `Ground`.
- **Animaciones**:
  - Cambios automáticos entre animaciones de `Idle`, `Walk`, `Run` y `Jump`, controladas mediante un **Blend Tree**.
- **Sistema de doble cuerda (Grappling Hook)**:
  - **Cuerda derecha** controlada con el clic derecho.
  - **Cuerda izquierda** controlada con el clic izquierdo.
  - Cada cuerda puede engancharse a objetos etiquetados como `Grappable`.
  - Las cuerdas se pueden acortar o cancelar dinámicamente, lo que permite moverse hacia el objetivo de forma controlada.

---

## 📂 Estructura del Proyecto
- **Assets**:
  - `Scripts`: Contiene los scripts principales del sistema de movimiento y cuerdas.
  - `Animations`: Controlador de animaciones (`Animator Controller`) configurado con un Blend Tree.
  - `Prefabs`: Prefabricados del personaje y otros objetos reutilizables.
- **Scenes**:
  - Una escena principal con un entorno básico, cubos etiquetados como `Ground` y `Grappable`, y un personaje en primera persona controlable.

---

## 📋 Requisitos del sistema
- **Unity**: Version 2020.3 LTS o superior.
- **Plataforma soportada**: Windows, macOS, Linux.

---

## 🚀 Cómo usar el proyecto
### 1. **Configuración inicial**
   - Descarga el proyecto desde el repositorio:
     ```bash
     git clone https://github.com/zylberman/AOT.git
     ```
   - Ábrelo en Unity.

### 2. **Control del personaje**
   - **W/A/S/D**: Moverse hacia adelante, atrás, izquierda y derecha.
   - **Shift**: Correr mientras te mueves hacia adelante.
   - **Espaciadora**: Saltar (si estás en contacto con `Ground`).
   - **Clic izquierdo**:
     - Disparar la cuerda izquierda hacia un objeto etiquetado como `Grappable`.
     - Doble clic izquierdo + mantener: Acortar la cuerda izquierda y atraer al personaje hacia el objetivo.
     - Soltar después de un doble clic: Elimina la cuerda izquierda.
   - **Clic derecho**:
     - Disparar la cuerda derecha hacia un objeto etiquetado como `Grappable`.
     - Doble clic derecho + mantener: Acortar la cuerda derecha y atraer al personaje hacia el objetivo.
     - Soltar después de un doble clic: Elimina la cuerda derecha.

### 3. **Ejecuta la escena**
   - Abre la escena principal (por ejemplo, `MainScene`).
   - Ejecuta el proyecto presionando el botón **Play**.

---

## 📜 Scripts principales
### **PlayerMovement.cs**
- Gestiona el movimiento del personaje en primera persona, incluyendo caminar, correr y saltar.
- Actualiza los parámetros del Animator (`XSpeed` y `YSpeed`) para controlar las animaciones.
- Detecta colisiones con objetos etiquetados como `Ground`.

### **GunController.cs**
- Sistema de doble cuerda (grappling hook) que permite al personaje engancharse y moverse hacia objetos etiquetados como `Grappable`.
- Cada lado del mouse controla una cuerda:
  - **Clic izquierdo**: Controla la cuerda izquierda.
  - **Clic derecho**: Controla la cuerda derecha.
- Incluye configuraciones como fuerza del resorte y velocidad de acortamiento de cada cuerda.

---

## ✨ Mejoras futuras
- Añadir más animaciones, como aterrizajes suaves o caídas.
- Implementar efectos visuales o sonoros al disparar la cuerda.
- Crear un sistema de física para objetos interactuables.
- Mejorar el movimiento del grappling hook para incluir físicas avanzadas, como balanceo dinámico, para una experiencia más inmersiva.
- Integrar un HUD que indique la cuerda activa y su longitud.

---

## 👨‍💻 Contribuciones
¡Las contribuciones son bienvenidas! Si deseas mejorar el proyecto, sigue estos pasos:
1. Haz un fork del repositorio.
2. Crea una rama para tu funcionalidad (`git checkout -b feature/nueva-funcionalidad`).
3. Realiza un pull request.

---

## 📧 Contacto
Si tienes dudas o sugerencias, puedes contactarme en:
- **Correo electrónico**: [byronmena05@gmail.com](mailto:byronmena05@gmail.com)
- **GitHub**: [https://github.com/zylberman](https://github.com/zylberman)

---

### 📝 Nota
Este proyecto está inspirado en el sistema de maniobras tridimensionales de **Attack on Titan (AOT)** y busca replicar la sensación de movimiento dinámico con cuerdas y anclajes. No es una copia oficial, pero puede servir como base para proyectos similares.
