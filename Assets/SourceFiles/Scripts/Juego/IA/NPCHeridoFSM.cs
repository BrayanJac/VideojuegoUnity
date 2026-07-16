using UnityEngine;

public class NPCHeridoFSM : MonoBehaviour
{
    [Header("Estado Actual")]
    public EstadoHerido estadoActual = EstadoHerido.Esperando;

    void Start()
    {
        CambiarEstado(EstadoHerido.Esperando);
    }

    void Update()
    {
        switch (estadoActual)
        {
            case EstadoHerido.Esperando:
                EstadoEsperando();
                break;

            case EstadoHerido.PideAyuda:
                EstadoPideAyuda();
                break;

            case EstadoHerido.RecibePrimerosAuxilios:
                EstadoPrimerosAuxilios();
                break;

            case EstadoHerido.SigueBombero:
                EstadoSeguirBombero();
                break;

            case EstadoHerido.EstadoEmpeora:
                EstadoEmpeora();
                break;

            case EstadoHerido.Rescatado:
                EstadoRescatado();
                break;

            case EstadoHerido.NoRescatado:
                EstadoNoRescatado();
                break;
        }
    }

    void CambiarEstado(EstadoHerido nuevoEstado)
    {
        estadoActual = nuevoEstado;
        Debug.Log(name + " -> " + nuevoEstado);
    }

    void EstadoEsperando()
    {

    }

    void EstadoPideAyuda()
    {

    }

    void EstadoPrimerosAuxilios()
    {

    }

    void EstadoSeguirBombero()
    {

    }

    void EstadoEmpeora()
    {

    }

    void EstadoRescatado()
    {

    }

    void EstadoNoRescatado()
    {

    }
}