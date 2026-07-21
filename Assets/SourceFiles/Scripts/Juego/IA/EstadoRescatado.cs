using UnityEngine;
using UnityEngine.AI;

public class EstadoRescatado : EstadoBase
{
    public EstadoRescatado(NPCHeridoFSM fsm, NPCHerido npc)
        : base(fsm, npc)
    {
    }

    public override void Entrar()
    {
        Debug.Log($"{npc.name}: Rescatado. Entrando a la ambulancia.");

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
        {
            foreach (AnimatorControllerParameter parametro in npc.Animator.parameters)
            {
                if (parametro.name == "Speed")
                {
                    npc.Animator.SetFloat("Speed", 0f);
                    break;
                }
            }
        }

        if (ContadorRescates.Instance != null)
            ContadorRescates.Instance.RegistrarRescate();
    }
}
