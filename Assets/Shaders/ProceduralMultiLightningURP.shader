
Shader "Custom/ProceduralMultiLightningURP"
{
    Properties
    {
        _EnergyColor ("Energy Color", Color) = (1,0.05,0.05,1)
        _Intensity ("Intensity", Float) = 1.0
        _Speed ("Speed", Float) = 1.0
        _NoiseScale ("Noise Scale", Float) = 25.0
        _Chaos ("Chaos Amount", Float) = 0.5
        _BoltCount ("Bolt Density", Float) = 6.0
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
            float _Chaos;
            float _BoltCount;

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1,311.7))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                float a = hash(i);
                float b = hash(i + float2(1,0));
                float c = hash(i + float2(0,1));
                float d = hash(i + float2(1,1));

                float2 u = f*f*(3.0-2.0*f);
                return lerp(a,b,u.x) + (c-a)*u.y*(1.0-u.x) + (d-b)*u.x*u.y;
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

                float energy = 0.0;

                // Multiple lightning bolts
                for (int i = 0; i < 6; i++)
                {
                    float offset = (float)i / _BoltCount;
                    float n = noise(float2(uv.y * _NoiseScale + offset * 10.0, time + offset));
                    float bolt = smoothstep(0.55, 0.75, n);

                    float chaos = noise(float2(time * 2.0, uv.x * 10.0 + offset)) * _Chaos;
                    energy += bolt * chaos;
                }

                float flicker = noise(float2(time * 6.0, uv.x * 3.0)) * 0.6 + 0.4;
                float edgeFade = smoothstep(0.05, 0.2, uv.x) * smoothstep(0.05, 0.2, 1.0 - uv.x);

                energy = energy * flicker * edgeFade * _Intensity;

                return half4(_EnergyColor.rgb * energy * 2.5, energy);
            }
            ENDHLSL
        }
    }
}
