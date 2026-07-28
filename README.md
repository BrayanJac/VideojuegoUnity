# FIRE RESCUE UNIT 7 — Juego de Bombero Rescatista

Un juego en primera persona en Unity donde eres un bombero que debe rescatar civiles heridos en un edificio en llamas.

## Gameplay

1. **Explora** el edificio en llamas — el fuego daña al jugador y el humo agota el oxígeno.
2. **Recoge** suministros médicos (MedKit, Vendaje, Analgesico, Adrenalina) y equipo (Extintor, Linterna) repartidos por el mapa.
3. **Apaga incendios** con el Extintor — apunta y haz clic para extinguirlos.
4. **Cura NPCs heridos** acercándote (tecla E) con el objeto médico correcto en el menú radial (tecla Q).
5. **Guía a los NPCs curados** hasta la Ambulancia — te siguen automáticamente tras ser curados.
6. **Gana** cuando todos los NPCs sean rescatados. **Pierde** si el tiempo se acaba o la salud del jugador llega a cero.

## Escenas

| Escena | Descripción |
|---|---|
| `MenuPrincipal` | Pantalla de título con Play, Opciones, Créditos, Salir |
| `MenuOpciones` | Centro de opciones: Sonido, Volumen, Dificultad, Idioma |
| `MenuDifficultad` | Selección de dificultad (Fácil / Normal / Difícil) |
| `MenuCargando` | Pantalla de carga con texto de misión; carga la partida asíncronamente |
| `Juego` | Escena principal de juego — la misión de rescate |
| `PantallaGanar` | Pantalla de victoria con puntuación, estrellas y estadísticas |
| `PantallaPerder` | Pantalla de derrota con motivo de muerte |

## Mecánicas principales

### Jugador
- Movimiento primera persona (WASD + mouse, CharacterController, salto, gravedad, daño por caída)
- Salud (100 HP) — dañado por fuego cercano (10 HP/s a 25 unidades)
- Oxígeno (100 O2) — se agota en zonas de humo (3 O2/s); al vaciarse el jugador recibe daño
- Muerte — muestra `PantallaPerder` con el motivo

### Extintor
- Debe recogerse y equiparse en el menú radial
- Clic izquierdo para apagar fuegos (30 unidades de alcance, 20° de ángulo)
- Lleva la cuenta de incendios apagados para la puntuación final

### Inventario y equipo
- 7 tipos de objeto: MedKit, Analgesico, Vendaje, Adrenalina, Linterna, Hacha, Extintor
- **Menú radial** (tecla Q): muestra los objetos en un círculo; selecciona con el ratón
- Los objetos médicos dan cantidad ×2 al recogerse
- Equipar un objeto activa su representación visual en la mano del personaje
- La **Linterna** reduce la opacidad del humo al recogerse (mejora la visibilidad)

### Sistema de rescate NPC (FSM)
Los NPCs siguen una máquina de estados:

| Estado | Comportamiento |
|---|---|
| `Esperando` | Acostado, esperando |
| `PideAyuda` | Pide ayuda cuando el jugador se acerca (reproduce audio) |
| `RecibePrimerosAuxilios` | Siendo curado (5 segundos) |
| `SigueBombero` | Curado — se levanta y sigue al jugador vía NavMesh |
| `Empeora` | Estado crítico: salud se deteriora más rápido (60s) |
| `Rescatado` | Llegó a la ambulancia |
| `NoRescatado` | NPC murió (estado terminal) |

Curación por objeto: Analgesico = 15 HP, Vendaje = 30 HP, Adrenalina = 50 HP, MedKit = 90 HP.

### Dificultad

| Dificultad | Tiempo límite | Salud inicial NPC | Deterioro NPC |
|---|---|---|---|
| Fácil | 300s | 100% | 0.5× (lento) |
| Normal | 180s | 90% | 1.0× (normal) |
| Difícil | 120s | 80% | 2.0× (rápido) |

### Puntuación y estrellas
Al ganar se calcula:
- Base: 500 pts
- NPCs rescatados: +50 pts c/u
- Incendios extinguidos: +30 pts c/u
- Tiempo restante: hasta 200 pts (proporcional)
- **Estrellas:** 3 (≥1000 pts — casi perfecto), 2 (≥600 pts — buen trabajo), 1 (<600 pts — mínimo)

## Controles

| Tecla | Acción |
|---|---|
| W-A-S-D | Moverse |
| Mouse | Mirar |
| Espacio | Saltar |
| E | Interactuar (recoger objetos, curar NPC) |
| Q | Abrir menú radial de inventario |
| P | Pausa |
| I | Instrucciones |

## Tecnología

- **Unity** 6000.4.8f1
- **Input System** (Paquete Unity)
- **TextMesh Pro** para UI
- **NavMesh** para pathfinding de NPCs
- **Universal Render Pipeline (URP)**
- **ProBuilder** para geometría del escenario
- **ScriptableObjects** para datos de objetos

## Assets de terceros

- **Gabriel Aguiar Productions** — efectos visuales
- **Vefects Free Fire VFX URP** — partículas de fuego
- **TimmyRobot** — modelo 3D del jugador y sonidos de pasos
- **npc_casual_set_00** — modelos de NPCs
