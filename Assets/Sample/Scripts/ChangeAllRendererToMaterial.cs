using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAllRendererToMaterial : MonoBehaviour
{
    public Material material;
    private void Awake()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++) { 
            renderers[i].sharedMaterial = material;
        }
    }
}
