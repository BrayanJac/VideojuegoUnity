using UnityEngine;

public class EstadoRecibePrimerosAuxilios : EstadoBase
{
    public EstadoRecibePrimerosAuxilios(NPCHeridoFSM fsm, NPCHerido npc)
        : base(fsm, npc)
    {
    }

    public override void Entrar()
    {
        npc.ProgresoCuracion = 0;
        Debug.Log($"{npc.name}: Recibiendo primeros auxilios");
    }

    public override void Actualizar()
    {
        npc.ProgresoCuracion += Time.deltaTime;

        float porcentaje = npc.ProgresoCuracion / npc.TiempoCuracion;

        Debug.Log($"Curando: {(porcentaje * 100):F0}%");

        if (npc.ProgresoCuracion >= npc.TiempoCuracion)
        {
            fsm.CambiarEstado(EstadoHerido.SigueBombero);
        }
    }
}