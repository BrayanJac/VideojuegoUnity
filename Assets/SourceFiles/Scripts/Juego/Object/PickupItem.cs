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

            if (itemData.id == "linterna")
            {
                EquipmentManager equipMgr = EquipmentManager.Instance;
                if (equipMgr != null)
                {
                    foreach (EquipableObject obj in equipMgr.objetos)
                    {
                        if (obj != null && obj.itemData != null && obj.itemData.id == "linterna")
                        {
                            LinternaController.RecogerLinterna(obj.gameObject);
                            break;
                        }
                    }
                }
                if (UIManager.Instance != null)
                    UIManager.Instance.ActualizarIconoLinterna();
            }
            else
            {
                InventoryManager.Instance.AgregarItem(itemData);
            }

            UIManager.Instance.OcultarTextoRecoger();

            if (AudioManager.instancia != null)
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