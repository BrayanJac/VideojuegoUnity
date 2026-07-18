using UnityEngine;
using UnityEngine.InputSystem;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;
    private bool jugadorCerca = false;
    public AudioClip sonidoRecoger;

    void Update()
    {
        if (jugadorCerca && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (itemData == null)
            {
                Debug.LogError("El PickupItem no tiene un ItemData asignado: " + gameObject.name);
                return;
            }

            InventoryManager.Instance.AgregarItem(itemData);

            UIManager.Instance.OcultarTextoRecoger();

            AudioManager.instancia.ReproducirEfecto(sonidoRecoger);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            UIManager.Instance.MostrarTextoRecoger(itemData.nombre);
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