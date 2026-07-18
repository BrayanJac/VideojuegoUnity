using UnityEngine;

public class EstadoPideAyuda : EstadoBase
{
    public EstadoPideAyuda(NPCHeridoFSM fsm,
                           NPCHerido npc) : base(fsm, npc)
    {

    }

    public override void Entrar()
    {
        Debug.Log($"{npc.name}: ¡Ayuda!");
    }

    public override void Actualizar()
    {

    }
}