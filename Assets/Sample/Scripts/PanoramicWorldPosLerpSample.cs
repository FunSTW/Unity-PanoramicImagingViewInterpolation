using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PanoramicWorldPosLerpSample : MonoBehaviour
{
    public ReflectionProbe[] reflectionProbes;
    public Material material;
    public Transform cameraTransfrom;

    private int current = 0;
    private float lerp = 0;

    static class ShaderIDs
    {
        internal static readonly int CubemapA = Shader.PropertyToID("_CubemapA");
        internal static readonly int PosA = Shader.PropertyToID("_PosA");
        internal static readonly int CubemapB = Shader.PropertyToID("_CubemapB");
        internal static readonly int PosB = Shader.PropertyToID("_PosB");
        internal static readonly int Lerp = Shader.PropertyToID("_Lerp");
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        cameraTransfrom = Camera.main.transform;
        //Init
        //A
        material.SetTexture(ShaderIDs.CubemapA, reflectionProbes[reflectionProbes.Length-1].texture);
        material.SetVector(ShaderIDs.PosA, reflectionProbes[reflectionProbes.Length-1].transform.position);
        //B
        material.SetTexture(ShaderIDs.CubemapB, reflectionProbes[0].texture);
        material.SetVector(ShaderIDs.PosB, reflectionProbes[0].transform.position);
        //Lerp(A,B)
        material.SetFloat(ShaderIDs.Lerp, 1);
        lerp = 1;
    }

    float clickDuration = 0.0f;
    bool clicking = false;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickDuration = 0.2f;
            clicking = true;
        }
        if (Input.GetMouseButton(0))
        {
            clickDuration -= Time.deltaTime;
        }
        if (clicking && Input.GetMouseButtonUp(0))
        {
            clicking = false;
            if (clickDuration > 0.0f)
            {
                lerp = 0.0f;
                current++;
            }
        }

        if (lerp <= 1.0f)
        {
            current = current % reflectionProbes.Length;
            //A
            material.SetTexture(ShaderIDs.CubemapA, reflectionProbes[current].texture);
            material.SetVector(ShaderIDs.PosA, reflectionProbes[current].transform.position);
            //B
            int next = (current + 1) % reflectionProbes.Length;
            material.SetTexture(ShaderIDs.CubemapB, reflectionProbes[next].texture);
            material.SetVector(ShaderIDs.PosB, reflectionProbes[next].transform.position);
            //Lerp(A,B)
            lerp += Time.deltaTime;
            material.SetFloat(ShaderIDs.Lerp, lerp);
            cameraTransfrom.position = Vector3.Lerp(reflectionProbes[current].transform.position, reflectionProbes[next].transform.position, lerp);
        }
    }
}