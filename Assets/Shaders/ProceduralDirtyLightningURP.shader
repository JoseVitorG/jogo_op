
Shader "Custom/ProceduralDirtyLightningURP"
{
    Properties
    {
        _EnergyColor ("Energy Color", Color) = (1,0.15,0.15,1)
        _Intensity ("Intensity", Float) = 1.0
        _Speed ("Speed", Float) = 1.0
        _NoiseScale ("Noise Scale", Float) = 30.0
        _Distortion ("Distortion", Float) = 0.2
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass
        {
            Blend One One
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _EnergyColor;
            float _Intensity;
            float _Speed;
            float _NoiseScale;
            float _Distortion;

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                float a = hash(i);
                float b = hash(i + float2(1,0));
                float c = hash(i + float2(0,1));
                float d = hash(i + float2(1,1));

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(a, b, u.x) +
                       (c - a) * u.y * (1.0 - u.x) +
                       (d - b) * u.x * u.y;
            }

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float time = _Time.y * _Speed;

                float2 uv = IN.uv;
                uv.y += noise(float2(uv.y * 5.0, time)) * _Distortion;

                float n = noise(uv * _NoiseScale + time);
                float lightning = step(0.6, n);

                float flicker = noise(float2(time * 5.0, uv.x)) * 0.5 + 0.5;

                float fade = smoothstep(0.0, 0.15, uv.x) * smoothstep(0.0, 0.15, 1.0 - uv.x);

                float energy = lightning * flicker * fade * _Intensity;

                return half4(_EnergyColor.rgb * energy, energy);
            }
            ENDHLSL
        }
    }
}
