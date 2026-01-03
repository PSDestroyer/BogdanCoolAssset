Shader "Custom/CollectableGlow"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,0,1)
        _GlowColor ("Glow Color", Color) = (1,1,0,1)
        _GlowStrength ("Glow Strength", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float glow : TEXCOORD0;
            };

            fixed4 _BaseColor;
            fixed4 _GlowColor;
            float _GlowStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Fresnel-like glow
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
                o.glow = pow(1 - saturate(dot(worldNormal, viewDir)), 2);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 baseCol = _BaseColor;
                fixed4 glow = _GlowColor * i.glow * _GlowStrength;

                return baseCol + glow;
            }
            ENDCG
        }
    }
}
