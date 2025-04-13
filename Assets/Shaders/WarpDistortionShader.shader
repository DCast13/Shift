Shader "Custom/WarpDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DistortionStrength ("Distortion Strength", Range(0, 1)) = 0.2
        _TimeOffset ("Time Offset", Float) = 0
        _Color ("Tint Color", Color) = (1, 1, 1, 1) // Add color property for tinting
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR; // Support particle vertex color
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR; // Pass vertex color to fragment
            };

            sampler2D _MainTex;
            float _DistortionStrength;
            float _TimeOffset;
            fixed4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color; // Pass particle color

                // Apply distortion effect
                float distortion = sin(_TimeOffset * 10 + v.uv.y * 5) * _DistortionStrength;
                o.uv.x += distortion;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texCol = tex2D(_MainTex, i.uv);
                return texCol * _Color * i.color; // Combine texture, tint, and particle color
            }
            ENDCG
        }
    }
}