using UnityEngine;

public class LinternaController : MonoBehaviour
{
    public static bool linternaRecogida { get; private set; }

    private static GameObject linternaObjeto;
    private static GameObject iconoLinternaUI;
    private static LinternaController instancia;

    public static void RecogerLinterna(GameObject objetoLinterna)
    {
        AsegurarInstancia();
        linternaRecogida = true;
        linternaObjeto = objetoLinterna;
        linternaObjeto.SetActive(true);
    }

    public static void RegistrarIconoUI(GameObject icono)
    {
        iconoLinternaUI = icono;
        if (iconoLinternaUI != null)
            iconoLinternaUI.SetActive(linternaRecogida);
    }

    void Update()
    {
        if (linternaRecogida && linternaObjeto != null && !linternaObjeto.activeInHierarchy)
            linternaObjeto.SetActive(true);
    }

    public static void Reset()
    {
        linternaRecogida = false;
        linternaObjeto = null;
        iconoLinternaUI = null;
    }

    private static void AsegurarInstancia()
    {
        if (instancia != null)
            return;
        var obj = new GameObject("LinternaController");
        DontDestroyOnLoad(obj);
        instancia = obj.AddComponent<LinternaController>();
    }
}
