# Análisis del código — Juego de Bombero con NPCs heridos

## 1. Movimiento y seguimiento del NPC

### Comportamiento actual

El NPC usa una **Máquina de Estados Finitos (FSM)** con 7 estados definidos en `EstadoHerido.cs`:

```csharp
public enum EstadoHerido
{
    Esperando,            // Acostado, esperando al jugador
    PideAyuda,            // Llama al jugador
    RecibePrimerosAuxilios, // Recibiendo curación (acostado)
    SigueBombero,         // Sigue al jugador hacia la ambulancia
    Empeora,              // Estado crítico, empeora rápido
    Rescatado,            // Terminal: llegó a la ambulancia
    NoRescatado           // Terminal: murió
}
```

El movimiento del NPC **solo ocurre en el estado `SigueBombero`**. Utiliza **NavMeshAgent** de Unity para pathfinding:

*EstadoSigueBombero.cs — Entrar()*
```csharp
public override void Entrar()
{
    npc.StandUp();  // Se levanta (animación de interpolación de rotación)
    npc.PuedeSerRescatado = true;
    agente = npc.Agente;
    // ...
    if (agente != null)
    {
        agente.enabled = true;
        agente.speed = npc.VelocidadSeguir;       // 28
        agente.acceleration = Mathf.Max(agente.acceleration, npc.VelocidadSeguir * 2f);
        agente.stoppingDistance = npc.DistanciaSeguimiento; // 2
        agente.isStopped = false;
    }
}
```

*EstadoSigueBombero.cs — Actualizar()*
```csharp
public override void Actualizar()
{
    if (bombero != null && agente != null && agente.enabled)
    {
        agente.SetDestination(bombero.position);

        if (agente.pathStatus == NavMeshPathStatus.PathInvalid
            || agente.pathStatus == NavMeshPathStatus.PathPartial)
        {
            WarpCercanoAlJugador();
        }

        if (tieneParametroSpeed && npc.Animator != null)
        {
            float velocidad = agente.velocity.magnitude;
            npc.Animator.SetFloat("Speed", velocidad);
        }
    }
    // Transición a Rescatado si está cerca de la ambulancia
    float distancia = Vector3.Distance(npc.transform.position, ambulancia.position);
    if (distancia <= npc.DistanciaRescate)  // 25
        fsm.CambiarEstado(EstadoHerido.Rescatado);
}
```

Si el pathfinding falla, el NPC se teletransporta (warp) cerca del jugador:

```csharp
private void WarpCercanoAlJugador()
{
    NavMeshHit hit;
    if (NavMesh.SamplePosition(bombero.position, out hit,
        npc.RadioDeteccion, NavMesh.AllAreas))
    {
        agente.Warp(hit.position);
        agente.SetDestination(bombero.position);
    }
}
```

También hay detección por trigger con la ambulancia en `NPCHerido.cs`:

```csharp
private void OnTriggerEnter(Collider other)
{
    if (!PuedeSerRescatado || EstaMuerto) return;
    if (other.CompareTag("Ambulancia"))
        fsm.CambiarEstado(EstadoHerido.Rescatado);
}
```

### Estados de reposo/inmovilidad

Los NPCs comienzan acostados (rotación -90° en X) y se levantan solo al entrar en `SigueBombero`. Durante `Esperando`, `PideAyuda`, `RecibePrimerosAuxilios`, `Empeora` y `NoRescatado` permanecen acostados sin animación ni movimiento.

### NPCs que NO pueden caminar (heridos graves)

**No existe lógica para transportar NPCs en camilla, cargarlos o arrastrarlos.** Todos los NPCs que logran curarse completamente (salud al 100%) se levantan y caminan solos hacia la ambulancia. No hay distinción entre heridos leves y graves en cuanto al método de traslado.

### Conclusión — Punto 1

| Aspecto | Estado |
|---------|--------|
| Pathfinding | ✅ NavMeshAgent con SetDestination |
| Sigue al bombero | ✅ Estado SigueBombero |
| Warp/teletransporte por path inválido | ✅ WarpCercanoAlJugador |
| NPCs que no pueden caminar | ❌ **No implementado** — Todos caminan por sí mismos |
| Camilla / arrastre / carga | ❌ **No implementado** |
| Animación de velocidad | ✅ SetFloat("Speed") |

---

## 2. Integración con Realidad Virtual (VR)

### Detección de acciones del jugador

El sistema de interacción usa **inputs tradicionales por teclado (tecla E)** y **detección por distancia/trigger**. No hay VR, XR, ni sistemas de agarre físico.

*NPCHeridoInteractuable.cs — Interacción con E*
```csharp
private void Update()
{
    // ... chequeo de distancia
    if (!Keyboard.current.eKey.wasPressedThisFrame)
        return;

    if (!ObjetoMedico.CurarNPC(npc))
        return;
    // ...
}
```

Hay también un `InteractorJugador.cs` que usa `Physics.OverlapSphere`, pero no está conectado a ningún input en el código actual:

```csharp
public class InteractorJugador : MonoBehaviour
{
    private float distanciaInteraccion = 4f;
    [SerializeField] private LayerMask capaNPC;

    public void Interactuar()
    {
        Collider[] npcs = Physics.OverlapSphere(
            transform.position, distanciaInteraccion, capaNPC);
        foreach (Collider c in npcs)
        {
            NPCHerido npc = c.GetComponent<NPCHerido>();
            if (npc != null)
            {
                ObjetoMedico.CurarNPC(npc);
                return;
            }
        }
    }
}
```

La curación se aplica directamente desde el inventario. No hay detección de qué objeto físico está cerca del NPC — solo se verifica qué item tiene equipado el jugador:

```csharp
public static bool CurarNPC(NPCHerido npc)
{
    // ...
    TipoItem tipo = InventoryManager.Instance.objetoEquipado.item.tipoItem;
    float curacion;
    switch (tipo)
    {
        case TipoItem.Analgesico:   curacion = 15;  break;
        case TipoItem.Vendaje:      curacion = 30;  break;
        case TipoItem.Adrenalina:   curacion = 50;  break;
        case TipoItem.MedKit:       curacion = 90;  break;
        default: return false;
    }
    npc.Curar(curacion);
    InventoryManager.Instance.ConsumirObjetoEquipado();
    return true;
}
```

Las transiciones de estado se disparan así:
- `Curar()` en `NPCHerido.cs` chequea si `salud.EstaCurado()` y cambia a `SigueBombero`
- `IniciarPrimerosAuxilios()` cambia a `RecibePrimerosAuxilios` (pero **no es llamado por ningún input** — el flujo real pasa directamente a `Curar`)

```csharp
public void Curar(float cantidad)
{
    salud.Curar(cantidad);
    if (salud.EstaCurado())
    {
        salud.DetenerDeterioro();
        fsm.CambiarEstado(EstadoHerido.SigueBombero);
    }
}
```

### Conclusión — Punto 2

| Aspecto | Estado |
|---------|--------|
| VR / XR / SteamVR / Oculus | ❌ **No existe ningún código VR** |
| Input por teclado (E) | ✅ Tecla E en `NPCHeridoInteractuable` |
| Input por colisión/trigger | ❌ No se usa para aplicar curación |
| Agarre físico de objetos | ❌ No existe |
| Eventos de interacción física | ❌ No existen |
| InteractorJugador (OverlapSphere) | ⚠️ Existe el método pero **no se llama desde ningún lado** |

---

## 3. Triaje / Evaluación médica

### Estado actual

**No existe un sistema de triaje, diagnóstico ni evaluación médica.** El flujo es directo:

1. Jugador se acerca al NPC
2. Presiona E
3. El item equipado se aplica inmediatamente
4. Si la salud llega a 100, el NPC se cura y sigue al bombero

No hay:
- Evaluación de signos vitales
- Uso de oxímetro
- Medición de presión arterial
- Diagnóstico antes del tratamiento
- Sub-estados de evaluación

### Lo que SÍ existe (relacionado con salud)

El NPC tiene salud que se deteriora automáticamente:

*NPCSalud.cs*
```csharp
void Update()
{
    if (deterioroActivo)
    {
        saludActual -= danioPorSegundo * multiplicadorDeterioro * Time.deltaTime;
        // ...
    }
}
```

Y el estado `Empeora` actúa como condición crítica:

```csharp
public override void Entrar()
{
    tiempoRestante = npc.TiempoCritico;  // 60 segundos
    if (npc.Salud != null)
        npc.Salud.EstablecerMultiplicadorDeterioro(2f);  // Deterioro ×2
}
```

El `PacienteCritico` es una propiedad booleana en `NPCHerido.cs` que **nunca se establece a true** — siempre es `false` por defecto, por lo que la transición a `Empeora` solo ocurre cuando `vidaBaja <= 50%`.

### Conclusión — Punto 3

| Aspecto | Estado |
|---------|--------|
| Triaje / diagnóstico | ❌ **No existe** |
| Oximetro | ❌ **No existe** (ni como item, ni como concepto) |
| Signos vitales | ❌ **No existen** |
| Presión arterial | ❌ **No existe** |
| Evaluación antes de tratar | ❌ La curación es inmediata al presionar E |
| Deterioro automático de salud | ✅ Sí, con multiplicador por dificultad |
| Estado crítico (Empeora) | ✅ Sí, con tiempo límite de 60s y deterioro ×2 |
| Health bar UI | ✅ Sí, con colores y alerta crítica |

---

## 4. Otras funcionalidades existentes

### Sistema de inventario radial
El jugador abre un menú radial con Q para seleccionar items médicos y herramientas. Los items médicos se consumen al usarlos.

### Detección de jugador por radio
`DetectorJugador.cs` detecta al jugador en un radio de 90 unidades para iniciar la FSM. No usa colliders ni triggers.

### Dificultad ajustable
- Fácil: deterioro ×0.5, salud inicial 100%
- Normal: deterioro ×1.0, salud inicial 90%
- Difícil: deterioro ×2.0, salud inicial 80%

### Sistema de incendios
El jugador puede apagar incendios con un extintor. Esto afecta la puntuación final en la pantalla de victoria.

### Contador de rescates y condición de victoria
Cuando todos los NPCs son rescatados, se carga la escena `PantallaGanar` con estadísticas (NPCs salvados, incendios apagados, tiempo restante, puntuación con estrellas 1-3).

---

## Resumen de gaps

1. **NPCs que no pueden caminar** — No hay soporte para transportar NPCs en camilla, cargarlos o arrastrarlos. Todos los NPCs curados caminan solos.
2. **VR / XR** — No existe ningún código. Toda la interacción es por teclado (tecla E).
3. **Triaje / evaluación médica** — No existe diagnosis previa al tratamiento. El jugador aplica items directamente sin evaluar el estado del NPC.
4. **Oxímetro / signos vitales** — No existen como items ni como mecánica.
5. **InteractorJugador** — El método `Interactuar()` existe pero no está conectado a ningún evento o input.
6. **Estado `RecibePrimerosAuxilios`** — El contador de progreso de curación (5 segundos) existe pero la transición real de estado ocurre en `Curar()`, no cuando el tiempo se completa. Hay lógica duplicada/conflictiva.
7. **PacienteCritico** — La propiedad booleana siempre es `false`; nunca se asigna `true` en ningún lado.
