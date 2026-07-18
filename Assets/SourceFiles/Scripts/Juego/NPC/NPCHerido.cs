using UnityEngine;

public class NPCHerido : MonoBehaviour
{
    [Header("Configuración")]

    [SerializeField] private bool pacienteCritico = false;

    [SerializeField] private float tiempoCritico = 60f;

    [SerializeField] private float radioDeteccion = 20f;

    [SerializeField] private float velocidadSeguir = 2f;

    private Animator animator;

    public bool PacienteCritico => pacienteCritico;
    public float TiempoCritico => tiempoCritico;
    public float RadioDeteccion => radioDeteccion;
    public float VelocidadSeguir => velocidadSeguir;
    public Animator Animator => animator;

    [Header("Primeros Auxilios")]
    [SerializeField] private float tiempoCuracion = 5f;

    private float progresoCuracion = 0f;

    public float TiempoCuracion => tiempoCuracion;

    public float ProgresoCuracion
    {
        get => progresoCuracion;
        set => progresoCuracion = value;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private NPCHeridoFSM fsm;

    private void Start()
    {
        fsm = GetComponent<NPCHeridoFSM>();
    }

    public void IniciarPrimerosAuxilios()
    {
        fsm.CambiarEstado(EstadoHerido.RecibePrimerosAuxilios);
    }
}