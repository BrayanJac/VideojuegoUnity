using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public EquipableObject[] objetos;

    private EquipableObject objetoActual;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (EquipableObject obj in objetos)
        {
            obj.gameObject.SetActive(false);
        }
    }

    public void Equipar(ItemData item)
    {
        if (objetoActual != null)
        {
            objetoActual.gameObject.SetActive(false);
        }

        foreach (EquipableObject obj in objetos)
        {
            if (obj.itemData == item)
            {
                objetoActual = obj;
                objetoActual.gameObject.SetActive(true);

                Debug.Log("Equipado: " + item.nombre);

                return;
            }
        }
    }

    public void Desequipar()
    {
        if (objetoActual != null)
        {
            objetoActual.gameObject.SetActive(false);
            objetoActual = null;
        }
    }
}