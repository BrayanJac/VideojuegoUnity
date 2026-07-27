using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject panelInventario;
    public TextMeshProUGUI listaItems;
    public PlayerMovement playerMovement;

    void Start()
    {
        RadialMenu radial = FindFirstObjectByType<RadialMenu>();
        if (radial != null && radial.menu != null && panelInventario != null)
        {
            Image fuente = radial.menu.GetComponent<Image>();
            Image destino = panelInventario.GetComponent<Image>();
            if (fuente != null && destino != null)
            {
                destino.sprite = fuente.sprite;
                destino.color = fuente.color;
                destino.material = fuente.material;
                destino.type = fuente.type;
            }
        }
    }

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            if (panelInventario.activeSelf)
            {
                panelInventario.SetActive(false);
                playerMovement.puedeMoverse = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                panelInventario.SetActive(true);
                playerMovement.puedeMoverse = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void CerrarInventario()
    {
        panelInventario.SetActive(false);

        playerMovement.puedeMoverse = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}