using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCHerido : MonoBehaviour
{
    [Header("Configuracion")]

    private bool pacienteCritico = false;

    private float tiempoCritico = 60f;

    private float radioDeteccion = 90f;

    private float velocidadSeguir = 28f;

    private float distanciaSeguimiento = 2f;

    private float distanciaRescate = 25f;

    private Animator animator;
    private NavMeshAgent agente;
    private Quaternion rotacionOriginal;
    private bool estaAcostado;
    private Coroutine rutinaLevantar;

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
    private float tiempoCuracion = 5f;

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

        rotacionOriginal = transform.rotation;
        estaAcostado = true;
        transform.rotation = rotacionOriginal * Quaternion.Euler(-90f, 0f, 0f);

        if (animator != null)
            animator.enabled = false;

        if (agente != null)
        {
            agente.updateRotation = false;
            agente.isStopped = true;
        }
    }

    public NPCSalud Salud => salud;

    private void Start()
    {
        fsm = GetComponent<NPCHeridoFSM>();

        if (estaAcostado)
        {
            transform.rotation = rotacionOriginal * Quaternion.Euler(-90f, 0f, 0f);
            if (animator != null)
                animator.enabled = false;
        }
    }

    void Update()
    {
        if (estaAcostado)
        {
            transform.rotation = rotacionOriginal * Quaternion.Euler(-90f, 0f, 0f);
            if (animator != null && animator.enabled)
                animator.enabled = false;
        }
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

    public void LieDown()
    {
        if (EstaMuerto) return;

        if (rutinaLevantar != null)
        {
            StopCoroutine(rutinaLevantar);
            rutinaLevantar = null;
        }

        if (!estaAcostado)
        {
            rotacionOriginal = transform.rotation;
            estaAcostado = true;

            if (agente != null)
            {
                agente.updateRotation = false;
                agente.isStopped = true;
            }
        }

        if (animator != null)
            animator.enabled = false;

        transform.rotation = rotacionOriginal * Quaternion.Euler(-90f, 0f, 0f);
    }

    public void StandUp()
    {
        if (!estaAcostado || EstaMuerto) return;

        if (rutinaLevantar != null)
            StopCoroutine(rutinaLevantar);

        rutinaLevantar = StartCoroutine(RutinaLevantar());
    }

    private System.Collections.IEnumerator RutinaLevantar()
    {
        Quaternion inicio = transform.rotation;
        float duracion = 1f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;
            transform.rotation = Quaternion.Slerp(inicio, rotacionOriginal, t);
            yield return null;
        }

        transform.rotation = rotacionOriginal;
        estaAcostado = false;
        rutinaLevantar = null;

        if (animator != null)
            animator.enabled = true;

        if (agente != null)
            agente.updateRotation = true;
    }

    public void OnFootstep()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PuedeSerRescatado || EstaMuerto)
            return;

        if (other.CompareTag("Ambulancia"))
            fsm.CambiarEstado(EstadoHerido.Rescatado);
    }
}
