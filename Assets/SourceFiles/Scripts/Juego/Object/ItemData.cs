using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Item", menuName = "Juego/Item")]
public class ItemData : ScriptableObject
{
    [Header("Información")]
    public string nombre;

    [TextArea]
    public string descripcion;

    [Header("Visual")]
    public Sprite icono;

    [Header("Inventario")]
    public bool esApilable;
}