using UnityEngine;
using UnityEngine.InputSystem;

public class ExtintorController : MonoBehaviour
{
    public static int incendiosApagados;
    public static int incendiosTotales;

    [Header("Configuración")]
    private float rangoApagado = 30f;
    [SerializeField] private Transform fuegoEdificio;
    
    [Header("Referencias")]
    private ParticleSystem[] fuegos;
    private Transform jugador;
    
    private void Start()
    {
        if (fuegoEdificio == null)
        {
            GameObject fuegoObj = GameObject.Find("FuegoEdificio");
            if (fuegoObj != null)
                fuegoEdificio = fuegoObj.transform;
        }
        
        if (fuegoEdificio != null)
        {
            fuegos = fuegoEdificio.GetComponentsInChildren<ParticleSystem>();
            incendiosTotales = fuegos.Length;
        }

        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
            jugador = jugadorObj.transform;
    }
    
    private void Update()
    {
        if (!EstaExtintorEquipado())
            return;
            
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Intentando apagar fuegos...");
            IntentarApagarFuegos();
        }
    }
    
    private bool EstaExtintorEquipado()
    {
        if (EquipmentManager.Instance == null)
            return false;
            
        var equipableObject = EquipmentManager.Instance.objetoActual;
        if (equipableObject == null || equipableObject.itemData == null)
            return false;
            
        return equipableObject.itemData.id == "extintor";
    }
    
    private void IntentarApagarFuegos()
    {
        if (fuegos == null || fuegos.Length == 0)
            return;
        
        if (jugador == null)
            return;
        
        foreach (ParticleSystem fuego in fuegos)
        {
            if (fuego == null || !fuego.gameObject.activeInHierarchy)
                continue;
            
            float distancia = Vector3.Distance(jugador.position, fuego.transform.position);
            
            if (distancia <= rangoApagado)
            {
                fuego.Stop();
                fuego.gameObject.SetActive(false);
                incendiosApagados++;
                Debug.Log("Fuego apagado: " + fuego.name);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        
        if (fuegoEdificio != null)
        {
            Gizmos.DrawWireSphere(fuegoEdificio.position, rangoApagado);
        }
    }
}