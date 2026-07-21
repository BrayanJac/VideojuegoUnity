using System;
using UnityEngine;

public class ContadorRescates : MonoBehaviour
{
    public static ContadorRescates Instance { get; private set; }

    public static event Action<int, int> OnRescatesChanged;

    public int Total { get; private set; }
    public int Salvados { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Total = FindObjectsByType<NPCHerido>().Length;
        Salvados = 0;
        OnRescatesChanged?.Invoke(Salvados, Total);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void RegistrarRescate()
    {
        Salvados = Mathf.Min(Salvados + 1, Total);
        OnRescatesChanged?.Invoke(Salvados, Total);
    }
}
