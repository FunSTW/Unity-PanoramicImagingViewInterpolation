using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAllRendererToMaterial : MonoBehaviour
{
    public Material material;
    public Renderer[] renderers;
    private void Awake()
    {
        for (int i = 0; i < renderers.Length; i++) {
            Renderer renderer = renderers[i];
            Material[] sharedMaterials = renderer.sharedMaterials;
            for (int j = 0; j < sharedMaterials.Length; j++)
            {
                sharedMaterials[j] = material;
            }
            renderer.sharedMaterials = sharedMaterials;

            renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        }
    }

    private void OnValidate()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }
}
