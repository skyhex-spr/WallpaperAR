Shader "Custom/AspectCorrectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AspectRatio ("Aspect Ratio", Range(0.1, 10)) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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
            float _AspectRatio;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 texSize = _MainTex_TexelSize.xy;
                fixed2 scaledSize = texSize / _AspectRatio;

                fixed2 texCoords = i.uv;
                texCoords -= 0.5;
                texCoords *= scaledSize;
                texCoords += 0.5;

                fixed4 col = tex2D(_MainTex, texCoords);
                return col;
            }
            ENDCG
        }
    }
}
