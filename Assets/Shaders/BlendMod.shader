Shader "Custom/BlendMod"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        _BlendTex("Blend Texture",2D)= "white" {}
        _AlphaVal("AlphaVal", Range(0,5)) = 1.0
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
		    "IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
        }

        Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color   : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 color   : COLOR;
            };

            //sampler2D _MainTex;
            fixed4 _Color;
            sampler2D _BlendTex;
            float _AlphaVal;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color =v.color*_Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 main=i.color;
                fixed4 sec = tex2D(_BlendTex, i.uv);

                main.a=sec.a*_AlphaVal;

                //fixed4 bland =main*(1-sec.a)+sec*(sec.a);
                fixed4 ifFlag=step(sec,fixed4(0.5,0.5,0.5,0.5));
                fixed4 bland=ifFlag*(main*sec*2+main*main*(1-sec*2))+(1-ifFlag)*(main*(1-sec)*2+sqrt(main)*(2*sec-1));
                //fixed4 bland =main-((1-main)*(1-sec))/sec;
                return bland;
            }
            ENDCG
        }
    }
}
