using UnityEngine;

public class EstadoSigueBombero : EstadoBase
{
    public EstadoSigueBombero(NPCHeridoFSM fsm, NPCHerido npc)
        : base(fsm, npc)
    {
    }

    public override void Entrar()
    {
        Debug.Log($"{npc.name}: Ahora sigue al bombero.");
    }
}