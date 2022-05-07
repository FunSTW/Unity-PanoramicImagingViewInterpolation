Shader "FunS/PanoramaLerpProjection"
{
    Properties
    {
        [NoScaleOffset] _CubemapA("Cubemap A", Cube) = "" {}
        _PosA("Position A", Vector) = (0,0,0,0)

        [NoScaleOffset] _CubemapB("Cubemap B", Cube) = "" {}
        _PosB("Position B", Vector) = (0,0,0,0)

        _Lerp("Lerp" , Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull OFF

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            samplerCUBE _CubemapA;
            samplerCUBE _CubemapB;
            float4 _PosA;
            float4 _PosB;
            float _Lerp;

            struct appdata
            {
                float4 positionOS : POSITION;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
            };

            v2f vert (appdata v) 
            {
                v2f o; 
                o.positionCS = UnityObjectToClipPos(v.positionOS);
                o.positionWS = mul(unity_ObjectToWorld,v.positionOS);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 colorA = texCUBE(_CubemapA,normalize(i.positionWS - _PosA));
                float4 colorB = texCUBE(_CubemapB,normalize(i.positionWS - _PosB));
                float4 result = lerp(colorA, colorB, saturate(_Lerp));
#if defined(UNITY_COLORSPACE_GAMMA) && (SHADER_TARGET < 30)
                float4 output = result;
#else
                float4 output = pow(result, 2.2);
#endif
                return output;
            }
            ENDCG
        }
    }
}
