Shader "FunS/PanoramicImagingViewInterpolation"
{
    Properties
    {
        [NoScaleOffset] _CubemapA("Cubemap A", Cube) = "" {}
        _PosA("Position A", Vector) = (0,0,0,0)
        _RotA("Rotation A", Range(0,360)) = 0.0

        [NoScaleOffset] _CubemapB("Cubemap B", Cube) = "" {}
        _PosB("Position B", Vector) = (0,0,0,0)
        _RotB("Rotation B", Range(0,360)) = 0.0

        _Lerp("Lerp" , Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull OFF

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            samplerCUBE _CubemapA , _CubemapB;
            half4 _PosA, _PosB;
            half _RotA, _RotB;
            float _Lerp;

            struct appdata
            {
                float4 positionOS : POSITION;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                half3 vectorA : TEXCOORD1;
                half3 vectorB : TEXCOORD2;
            };
            
            #define TO_RADIANS 0.01745329251;
            inline half3 Cubemap_Rotate_Degrees (half3 UV, half Rotation)
            {
                Rotation = -Rotation * TO_RADIANS;
                half s = sin(Rotation);
                half c = cos(Rotation);
                half2x2 rMatrix = half2x2(c, -s, s, c);
                rMatrix *= 0.5;
                rMatrix += 0.5;
                rMatrix = rMatrix * 2 - 1;
                UV.xz = mul(UV.xz, rMatrix);
                return UV;
            }

            v2f vert (appdata v) 
            {
                v2f o; 
                o.positionCS = UnityObjectToClipPos(v.positionOS);
                float3 positionWS = mul(unity_ObjectToWorld,v.positionOS);
                o.vectorA = Cubemap_Rotate_Degrees(positionWS - _PosA,_RotA);
                o.vectorB = Cubemap_Rotate_Degrees(positionWS - _PosB,_RotB);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                half4 colorA = texCUBE(_CubemapA, normalize(i.vectorA));
                half4 colorB = texCUBE(_CubemapB, normalize(i.vectorB));

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
