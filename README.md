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
- 7 tipos de objeto definidos en `TipoItem`: MedKit, Analgesico, Vendaje, Adrenalina, Linterna, Hacha, Extintor
- **Menú radial** (tecla Q): muestra los objetos en disposición circular; selecciona con el ratón (`IPointerEnterHandler`)
- Los **objetos médicos** dan cantidad ×2 al recogerse (frente a ×1 para el resto)
- Equipar un objeto activa su representación visual en la mano del personaje (`EquipmentManager`)
- La **Linterna** es un objeto especial: no entra al inventario, `LinternaController` la activa globalmente y muestra su icono en el HUD. Persiste entre escenas via `DontDestroyOnLoad`.
- El **Hacha** está definida como tipo pero no tiene mecánica de juego asignada actualmente.

### Sistema de rescate NPC (FSM)
Los NPCs siguen una máquina de estados (`NPCHeridoFSM`) con 7 estados:

| Estado | Comportamiento |
|---|---|
| `Esperando` | Acostado, esperando al jugador |
| `PideAyuda` | Pide ayuda cuando el jugador se acerca (reproduce `sonidoAyuda`) |
| `RecibePrimerosAuxilios` | Siendo curado (progreso de 5 segundos) |
| `SigueBombero` | Curado — se levanta y sigue al jugador vía NavMesh |
| `Empeora` | Estado crítico (<60s restantes): salud se deteriora más rápido |
| `Rescatado` | Llegó a la ambulancia (trigger `Ambulancia`) |
| `NoRescatado` | NPC murió (salud ≤ 0, estado terminal) |

Curación por objeto: Analgesico = 15 HP, Vendaje = 30 HP, Adrenalina = 50 HP, MedKit = 90 HP.

La salud del NPC se deteriora constantemente en `NPCSalud.Update()`:
`saludActual -= (saludMaxima × 0.10 / 20s) × multiplicadorDeterioro × Time.deltaTime`

### Dificultad

Configurada en `DatosDificultad` (static class, persiste entre escenas):

| Dificultad | Tiempo límite | Salud inicial NPC | Deterioro NPC |
|---|---|---|---|
| Fácil | 300s | 100% | 0.5× (lento) |
| Normal | 180s | 90% | 1.0× (normal) |
| Difícil | 120s | 80% | 2.0× (rápido) |

### Sistema de HUD

Gestionado por `UIManager` (singleton), se compone de:

| Elemento | Posición | Descripción | Actualización |
|---|---|---|---|
| Contador rescates | Arriba izquierda | `"N/M"` — NPCs salvados / totales | Evento `ContadorRescates.OnRescatesChanged` |
| Equipado | Abajo derecha | Icono + nombre del objeto seleccionado + cantidad si es apilable | Evento `InventoryManager.OnInventoryChanged` |
| Icono linterna | Arriba centro | Se muestra solo si la linterna ha sido recogida | `UIManager.ActualizarIconoLinterna()` |
| Temporizador | — | Cuenta regresiva en formato `MM:SS` | `TemporizadorPartida.Update()` cada frame |
| Mensaje interacción | Centro | `"Presiona [E] para ..."` al estar cerca de objetos/NPCs | `UIManager.MostrarTextoRecoger()` / `OcultarTextoRecoger()` |
| Barra de vida NPC | Sobre el NPC | Barra verde/amarilla/roja según porcentaje, se muestra al 50% o cerca | `NPCSalud.Update()` con `barraVida.fillAmount` |

El menú radial (`RadialMenu`) se abre con Q (pulsación) y se cierra al soltar. Durante la apertura:
1. Pausa el movimiento del jugador (`playerMovement.puedeMoverse = false`)
2. Muestra el cursor y libera su bloqueo
3. Crea slots dinámicamente según el inventario actual
4. Anima entrada con `CanvasGroup.alpha` y escala (`SmoothStep`, 0.15s)
5. Al cerrar, equipa el slot bajo el ratón (`InventoryManager.EquiparItem`)

### Artículos — qué hace cada uno

| Artículo | Tipo | Al recogerlo | Al usarlo |
|---|---|---|---|
| **MedKit** | Médico (apilable) | `InventoryManager.AgregarItem` → +2 unidades | `ObjetoMedico.CurarNPC` → +90 HP al NPC. Se consume 1 unidad |
| **Analgesico** | Médico (apilable) | +2 unidades | +15 HP al NPC |
| **Vendaje** | Médico (apilable) | +2 unidades | +30 HP al NPC |
| **Adrenalina** | Médico (apilable) | +2 unidades | +50 HP al NPC |
| **Extintor** | Equipo (no apilable) | +1 unidad al inventario | Click izquierdo: apaga fuegos en 30m/20° (`ExtintorController`) |
| **Linterna** | Equipo especial | No va al inventario — `LinternaController.RecogerLinterna` activa el objeto globalmente | Mejora visibilidad reduciendo opacidad del humo |

El **extintor** además lleva el registro estático de `incendiosApagados` e `incendiosTotales` para la puntuación final.

### Puntuación y estrellas

Al rescatar al último NPC, `ContadorRescates.RegistrarRescate()` guarda las estadísticas en `PantallaGanar` (campos static) y carga la escena de victoria.

Cálculo en `PantallaGanar.Start()`:
```
puntajeBase     = 500
puntajeNPCs     = npcSalvados × 50
puntajeIncendios = incendiosExtinguidos × 30
puntajeTiempo   = (tiempoRestante / tiempoMaximo) × 200  (clamp 0-200)
puntajeTotal    = puntajeBase + puntajeNPCs + puntajeIncendios + puntajeTiempo
```

**Estrellas** (3 máximo):
| Puntaje total | Estrellas | Condición |
|---|---|---|
| ≥ 1000 pts | ★★★ | Casi perfecto — rescatar todos los NPCs, apagar suficientes incendios, y hacerlo rápido |
| ≥ 600 pts | ★★ | Buen trabajo — objetivo mínimo cumplido holgadamente |
| < 600 pts | ★ | Mínimo — lograste rescatar al menos a alguien |

Para obtener 3 estrellas (≥1000 pts) necesitas, por ejemplo: rescatar todos los NPCs (+50 c/u), apagar casi todos los incendios (+30 c/u), dejar poco tiempo en el cronómetro (+200), y sumar la base de 500.

## Controles

| Tecla | Acción |
|---|---|
| W-A-S-D | Moverse |
| Mouse | Mirar |
| Espacio | Saltar |
| E | Interactuar (recoger objetos, curar NPC) |
| Q | Abrir menú radial de inventario |
| P | Pausa |
| I | Controles |

## Tecnología

- **Unity** 6000.4.8f1
- **Input System** (Paquete Unity)
- **TextMesh Pro** para UI
- **NavMesh** para pathfinding de NPCs
- **Universal Render Pipeline (URP)**
- **ProBuilder** para geometría del escenario
- **ScriptableObjects** para datos de objetos

## Assets de terceros

- **Gabriel Aguiar Productions** — efectos visuales (humo, explosiones, destellos, fuego)
- **Vefects Free Fire VFX URP** — partículas de fuego usadas en incendios del edificio y fuego cercano al jugador
- **npc_casual_set_00** — modelos 3D de NPCs heridos (NPC_Herido_0, NPC_Herido_01)
