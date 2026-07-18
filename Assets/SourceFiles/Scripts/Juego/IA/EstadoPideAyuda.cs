using UnityEngine;

public class EstadoPideAyuda : EstadoBase
{
    public EstadoPideAyuda(NPCHeridoFSM fsm,
                           NPCHerido npc) : base(fsm, npc)
    {

    }

    public override void Entrar()
    {
        //AudioManager.instancia.ReproducirEfecto(npc.SonidoAyuda);
        AudioManager.instancia.ReproducirEfectoEnPosicion(npc.SonidoAyuda, npc.transform.position);
        Debug.Log($"{npc.name}: ¡Ayuda!");
    }

    public override void Actualizar()
    {

    }
}