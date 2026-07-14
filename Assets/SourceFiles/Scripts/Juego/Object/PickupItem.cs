using UnityEngine;
using UnityEngine.InputSystem;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;
    private bool jugadorCerca = false;

    void Update()
    {
        if (jugadorCerca && Keyboard.current.eKey.wasPressedThisFrame)
        {
            InventoryManager.Instance.AgregarItem(itemData);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            UIManager.Instance.MostrarTextoRecoger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            UIManager.Instance.OcultarTextoRecoger();
        }
    }
}