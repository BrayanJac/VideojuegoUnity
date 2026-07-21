using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Estado terminal: el NPC no fue rescatado a tiempo (muerto).
/// </summary>
public class EstadoNoRescatado : EstadoBase
{
    public EstadoNoRescatado(NPCHeridoFSM fsm, NPCHerido npc)
        : base(fsm, npc)
    {
    }

    public override void Entrar()
    {
        Debug.Log($"{npc.name}: No rescatado. Muerto.");

        npc.MarcarMuerto();
        npc.PuedeSerRescatado = false;

        if (npc.Salud != null)
            npc.Salud.DetenerDeterioro();

        NavMeshAgent agente = npc.Agente;
        if (agente != null && agente.enabled)
        {
            agente.ResetPath();
            agente.isStopped = true;
            agente.enabled = false;
        }

        if (npc.Animator != null)
            npc.Animator.enabled = false;

        if (UIManager.Instance != null)
            UIManager.Instance.OcultarTextoRecoger();
    }
}
