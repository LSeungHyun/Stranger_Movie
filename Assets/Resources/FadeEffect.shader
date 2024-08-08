Shader "Custom/FadeEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CharacterPos("Character Position", Vector) = (0,0,0,0)
        _Radius("Radius", Float) = 0.5
        _Smoothness("Smoothness", Float) = 0.1
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _CharacterPos;
            float _Radius;
            float _Smoothness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float2 screenPos = i.uv;
                float2 charPos = _CharacterPos.xy;

                float dist = distance(screenPos, charPos);
                float fade = smoothstep(_Radius, _Radius - _Smoothness, dist);

                // Apply fade to black color with transparency in the center
                return lerp(half4(0, 0, 0, 1), half4(0, 0, 0, 0), fade);
            }
            ENDCG
        }
    }
}
