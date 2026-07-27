using UnityEngine;

public class SmokeVisibilityController : MonoBehaviour
{
    private float alphaNormal = 1f;
    private float alphaConLinterna = 0.15f;

    private ParticleSystem[] particleSystems;
    private ParticleSystemRenderer[] renderers;
    private MaterialPropertyBlock propertyBlock;
    private bool linternaActiva;
    private bool fueVerificado;

    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);
        renderers = GetComponentsInChildren<ParticleSystemRenderer>(true);
        propertyBlock = new MaterialPropertyBlock();

        foreach (var ps in particleSystems)
        {
            var main = ps.main;
            main.playOnAwake = true;
        }
    }

    private void Update()
    {
        bool tieneLinterna = LinternaController.linternaRecogida;

        if (!fueVerificado)
        {
            if (tieneLinterna)
            {
                AplicarAlpha(alphaConLinterna);
                linternaActiva = true;
                fueVerificado = true;
            }
        }
        else if (linternaActiva != tieneLinterna)
        {
            linternaActiva = tieneLinterna;
            AplicarAlpha(tieneLinterna ? alphaConLinterna : alphaNormal);
        }
    }

    private void AplicarAlpha(float alpha)
    {
        foreach (var r in renderers)
        {
            if (r == null) continue;

            r.GetPropertyBlock(propertyBlock);
            Color color = r.sharedMaterial != null ? r.sharedMaterial.GetColor(BaseColor) : Color.white;
            color.a = alpha;
            propertyBlock.SetColor(BaseColor, color);
            r.SetPropertyBlock(propertyBlock);
        }
    }

    private void OnDestroy()
    {
        if (propertyBlock != null)
            propertyBlock.Clear();
    }
}
