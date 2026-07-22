using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCHerido : MonoBehaviour
{
    [Header("Configuracion")]

    [SerializeField] private bool pacienteCritico = false;

    [SerializeField] private float tiempoCritico = 60f;

    [SerializeField] private float radioDeteccion = 90f;

    [SerializeField] private float velocidadSeguir = 28f;

    [SerializeField] private float distanciaSeguimiento = 2f;

    [SerializeField] private float distanciaRescate = 12f;

    private Animator animator;
    private NavMeshAgent agente;

    public bool PacienteCritico => pacienteCritico;
    public float TiempoCritico => tiempoCritico;
    public float RadioDeteccion => radioDeteccion;
    public float VelocidadSeguir => velocidadSeguir;
    public float DistanciaSeguimiento => distanciaSeguimiento;
    public float DistanciaRescate => distanciaRescate;
    public Animator Animator => animator;
    public NavMeshAgent Agente => agente;

    public bool PuedeSerRescatado { get; set; }
    public bool EstaMuerto { get; private set; }

    [Header("Primeros Auxilios")]
    [SerializeField] private float tiempoCuracion = 5f;

    private float progresoCuracion = 0f;

    public float TiempoCuracion => tiempoCuracion;

    public float ProgresoCuracion
    {
        get => progresoCuracion;
        set => progresoCuracion = value;
    }

    [Header("Audio")]
    [SerializeField] private AudioClip sonidoAyuda;

    public AudioClip SonidoAyuda => sonidoAyuda;

    private NPCSalud salud;
    private NPCHeridoFSM fsm;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        salud = GetComponent<NPCSalud>();
        agente = GetComponent<NavMeshAgent>();
    }

    public NPCSalud Salud => salud;

    private void Start()
    {
        fsm = GetComponent<NPCHeridoFSM>();
    }

    public void IniciarPrimerosAuxilios()
    {
        if (EstaMuerto || fsm == null)
            return;

        fsm.CambiarEstado(EstadoHerido.RecibePrimerosAuxilios);
    }

    public void Curar(float cantidad)
    {
        if (EstaMuerto || salud == null)
            return;

        salud.Curar(cantidad);

        if (salud.EstaCurado())
        {
            salud.DetenerDeterioro();
            fsm.CambiarEstado(EstadoHerido.SigueBombero);
        }
    }

    public void MarcarMuerto()
    {
        EstaMuerto = true;
    }

    public void NotificarMuerte()
    {
        if (EstaMuerto || fsm == null)
            return;

        fsm.CambiarEstado(EstadoHerido.NoRescatado);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PuedeSerRescatado || EstaMuerto)
            return;

        if (other.CompareTag("Ambulancia"))
            fsm.CambiarEstado(EstadoHerido.Rescatado);
    }
}
