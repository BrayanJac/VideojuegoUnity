using UnityEngine;
using UnityEngine.AI;

public class EstadoSigueBombero : EstadoBase
{
    private Transform bombero;
    private Transform ambulancia;
    private NavMeshAgent agente;
    private bool tieneParametroSpeed;

    public EstadoSigueBombero(NPCHeridoFSM fsm, NPCHerido npc)
        : base(fsm, npc)
    {
    }

    public override void Entrar()
    {
        Debug.Log($"{npc.name}: Ahora sigue al bombero.");

        npc.StandUp();
        npc.PuedeSerRescatado = true;
        agente = npc.Agente;

        if (npc.Salud != null)
            npc.Salud.DetenerDeterioro();

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
            bombero = jugador.transform;

        GameObject ambulanciaObj = GameObject.FindGameObjectWithTag("Ambulancia");
        if (ambulanciaObj != null)
            ambulancia = ambulanciaObj.transform;

        if (agente != null)
        {
            agente.enabled = true;
            agente.speed = npc.VelocidadSeguir;
            agente.acceleration = Mathf.Max(agente.acceleration, npc.VelocidadSeguir * 2f);
            agente.stoppingDistance = npc.DistanciaSeguimiento;
            agente.isStopped = false;
        }

        tieneParametroSpeed = TieneParametroAnimacion("Speed");
    }

    public override void Actualizar()
    {
        if (bombero != null && agente != null && agente.enabled)
        {
            agente.SetDestination(bombero.position);

            if (agente.pathStatus == NavMeshPathStatus.PathInvalid || agente.pathStatus == NavMeshPathStatus.PathPartial)
            {
                WarpCercanoAlJugador();
            }

            if (tieneParametroSpeed && npc.Animator != null)
            {
                float velocidad = agente.velocity.magnitude;
                npc.Animator.SetFloat("Speed", velocidad);
            }
        }

        if (ambulancia == null)
        {
            GameObject ambulanciaObj = GameObject.FindGameObjectWithTag("Ambulancia");
            if (ambulanciaObj != null)
                ambulancia = ambulanciaObj.transform;
            else
                return;
        }

        float distancia = Vector3.Distance(npc.transform.position, ambulancia.position);
        if (distancia <= npc.DistanciaRescate)
            fsm.CambiarEstado(EstadoHerido.Rescatado);
    }

    private void WarpCercanoAlJugador()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(bombero.position, out hit, npc.RadioDeteccion, NavMesh.AllAreas))
        {
            agente.Warp(hit.position);
            agente.SetDestination(bombero.position);
        }
    }

    public override void Salir()
    {
        npc.PuedeSerRescatado = false;

        if (agente != null && agente.enabled)
        {
            agente.ResetPath();
            agente.isStopped = true;
        }

        if (tieneParametroSpeed && npc.Animator != null)
            npc.Animator.SetFloat("Speed", 0f);
    }

    private bool TieneParametroAnimacion(string nombre)
    {
        if (npc.Animator == null)
            return false;

        foreach (AnimatorControllerParameter parametro in npc.Animator.parameters)
        {
            if (parametro.name == nombre)
                return true;
        }

        return false;
    }
}
