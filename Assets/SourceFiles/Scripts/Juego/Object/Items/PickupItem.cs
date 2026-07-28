using UnityEngine;
using UnityEngine.InputSystem;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;
    public AudioClip sonidoRecoger;
    private bool jugadorCerca;
    private Transform jugador;

    void Update()
    {
        if (!jugadorCerca || jugador == null) return;

        if (Keyboard.current.eKey.wasPressedThisFrame && EsElMasCercano())
            Recoger();
    }

    bool EsElMasCercano()
    {
        float miDistancia = Vector3.Distance(jugador.position, transform.position);

        foreach (PickupItem otro in FindObjectsByType<PickupItem>())
        {
            if (otro == this || otro == null || !otro.jugadorCerca) continue;
            if (Vector3.Distance(jugador.position, otro.transform.position) < miDistancia)
                return false;
        }
        return true;
    }

    void Recoger()
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

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadorCerca = true;
        jugador = other.transform;

        if (EsElMasCercano())
            UIManager.Instance.MostrarTextoRecoger(itemData.nombre);
        else
            UIManager.Instance.OcultarTextoRecoger();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            jugador = null;
            UIManager.Instance.OcultarTextoRecoger();
        }
    }
}