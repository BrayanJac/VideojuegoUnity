using UnityEngine;

public class ObjetoMedico : MonoBehaviour
{
    public static bool CurarNPC(NPCHerido npc)
    {
        if (npc == null)
            return false;

        if (InventoryManager.Instance == null)
            return false;

        if (InventoryManager.Instance.objetoEquipado == null)
            return false;

        if (InventoryManager.Instance.objetoEquipado.item == null)
            return false;

        TipoItem tipo = InventoryManager.Instance.objetoEquipado.item.tipoItem;

        float curacion;

        switch (tipo)
        {
            case TipoItem.Analgesico:
                curacion = 15;
                break;

            case TipoItem.Vendaje:
                curacion = 30;
                break;

            case TipoItem.Adrenalina:
                curacion = 50;
                break;

            case TipoItem.MedKit:
                curacion = 90;
                break;

            default:
                return false;
        }

        npc.Curar(curacion);
        InventoryManager.Instance.ConsumirObjetoEquipado();

        return true;
    }
}
