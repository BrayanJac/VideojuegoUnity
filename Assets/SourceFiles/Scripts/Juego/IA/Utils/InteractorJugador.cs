using UnityEngine;
using UnityEngine.InputSystem;

public class InteractorJugador : MonoBehaviour
{
    [SerializeField] private float distanciaInteraccion = 4f;

    [SerializeField] private LayerMask capaNPC;

    public void Interactuar()
    {
        Collider[] npcs = Physics.OverlapSphere(
            transform.position,
            distanciaInteraccion,
            capaNPC);

        foreach (Collider c in npcs)
        {
            NPCHerido npc = c.GetComponent<NPCHerido>();

            if (npc != null)
            {
                npc.IniciarPrimerosAuxilios();
                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanciaInteraccion);
    }
}