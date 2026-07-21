using UnityEngine;

/// <summary>
/// El herido empeora: cuenta regresiva (tiempoCritico) y deterioro acelerado.
/// Si no se atiende a tiempo → NoRescatado.
/// </summary>
public class EstadoEmpeora : EstadoBase
{
    private float tiempoRestante;

    public EstadoEmpeora(NPCHeridoFSM fsm, NPCHerido npc)
        : base(fsm, npc)
    {
    }

    public override void Entrar()
    {
        tiempoRestante = npc.TiempoCritico;

        if (npc.Salud != null)
            npc.Salud.EstablecerMultiplicadorDeterioro(2f);

        Debug.Log($"{npc.name}: Empeorando. Tiempo crítico: {tiempoRestante:F0}s");
    }

    public override void Actualizar()
    {
        if (npc.EstaMuerto)
            return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0f)
        {
            Debug.Log($"{npc.name}: Se agotó el tiempo crítico.");
            fsm.CambiarEstado(EstadoHerido.NoRescatado);
            return;
        }

        if (npc.Salud != null && npc.Salud.PorcentajeVida <= 0f)
            fsm.CambiarEstado(EstadoHerido.NoRescatado);
    }

    public override void Salir()
    {
        if (npc.Salud != null && !npc.EstaMuerto)
            npc.Salud.EstablecerMultiplicadorDeterioro(1f);
    }
}
