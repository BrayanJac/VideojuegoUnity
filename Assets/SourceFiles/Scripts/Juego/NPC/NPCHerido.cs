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
    private float posYAcostado;

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
    public bool EstaRescatado { get; set; }

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
    [SerializeField] private AudioClip sonidoGracias;

    public AudioClip SonidoAyuda => sonidoAyuda;
    public AudioClip SonidoGracias => sonidoGracias;

    private NPCSalud salud;
    private NPCHeridoFSM fsm;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        salud = GetComponent<NPCSalud>();
        agente = GetComponent<NavMeshAgent>();

        rotacionOriginal = transform.rotation;
        estaAcostado = true;
        IniciarAcostado();

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
            AplicarRotacionAcostado();
            if (animator != null)
                animator.enabled = false;
        }
    }

    void Update()
    {
        if (estaAcostado)
        {
            AplicarRotacionAcostado();
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

        IniciarAcostado();
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
        Vector3 posInicio = transform.position;
        float duracion = 1f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;
            transform.rotation = Quaternion.Slerp(inicio, rotacionOriginal, t);
            transform.position = Vector3.Lerp(posInicio, new Vector3(posInicio.x, posYAcostado, posInicio.z), t);
            yield return null;
        }

        transform.rotation = rotacionOriginal;
        transform.position = new Vector3(transform.position.x, posYAcostado, transform.position.z);
        estaAcostado = false;

        if (animator != null)
            animator.enabled = true;

        if (agente != null)
            agente.updateRotation = true;
    }

    public void OnFootstep()
    {
    }

    private void IniciarAcostado()
    {
        transform.rotation = rotacionOriginal * Quaternion.Euler(-90f, 0f, 0f);

        SkinnedMeshRenderer mr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (mr == null)
        {
            posYAcostado = transform.position.y;
            return;
        }

        float bodyBottom = mr.bounds.min.y;
        float bodyTop = mr.bounds.max.y;
        float bodyCenter = (bodyBottom + bodyTop) * 0.5f;
        float bodyHalfHeight = (bodyTop - bodyBottom) * 0.5f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 100f, NavMesh.AllAreas))
        {
            posYAcostado = hit.position.y + bodyHalfHeight;
        }
        else
        {
            posYAcostado = bodyCenter;
        }

        transform.position = new Vector3(transform.position.x, posYAcostado, transform.position.z);
    }

    private void AplicarRotacionAcostado()
    {
        transform.rotation = rotacionOriginal * Quaternion.Euler(-90f, 0f, 0f);
        transform.position = new Vector3(
            transform.position.x,
            posYAcostado,
            transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PuedeSerRescatado || EstaMuerto)
            return;

        if (other.CompareTag("Ambulancia"))
            fsm.CambiarEstado(EstadoHerido.Rescatado);
    }
}
