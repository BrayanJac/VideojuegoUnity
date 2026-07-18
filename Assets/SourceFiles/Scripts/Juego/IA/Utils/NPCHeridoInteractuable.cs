using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NPCHerido))]
public class NPCHeridoInteractuable : MonoBehaviour
{
    private bool jugadorCerca = false;

    private NPCHerido npc;

    private void Awake()
    {
        npc = GetComponent<NPCHerido>();
    }

    private void Update()
    {
        if (!jugadorCerca)
            return;

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            npc.IniciarPrimerosAuxilios();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        jugadorCerca = true;

        UIManager.Instance.MostrarTextoRecoger("Atender herido");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        jugadorCerca = false;

        UIManager.Instance.OcultarTextoRecoger();
    }
}