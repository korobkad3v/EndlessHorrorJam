Shader "Custom/RetroPaletteOnTextureWithStencilAndDither"
{
    Properties
    {
        
        _MainTex ("Base (RGB)", 2D) = "white" {}

       
        _Color0 ("Palette Color 0", Color) = (1, 0, 0, 1)   
        _Color1 ("Palette Color 1", Color) = (0, 1, 0, 1)   
        _Color2 ("Palette Color 2", Color) = (0, 0, 1, 1)   
        _Color3 ("Palette Color 3", Color) = (1, 1, 0, 1)   

        
        _Stencil ("Stencil Value", Float) = 1

       
        _DitherStrength ("Dither Strength", Range(0,1)) = 0.1
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

            
            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color0;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float _DitherStrength;

            
            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            
            float4 frag(v2f i) : SV_Target
            {
                
                float4 col = tex2D(_MainTex, i.uv);

                
                
                float2 screenPos = i.uv * _ScreenParams.xy;
                
                float dither = (frac(sin(dot(screenPos, float2(12.9898,78.233)))*43758.5453) - 0.5) * _DitherStrength;
            
                col.rgb = saturate(col.rgb + dither);

                
                float bestDistance = 1e6; 
                float4 bestColor = col;

                
                float4 palette[4];
                palette[0] = _Color0;
                palette[1] = _Color1;
                palette[2] = _Color2;
                palette[3] = _Color3;

                
                for (int j = 0; j < 4; j++)
                {
                    float4 palColor = palette[j];
                    float d = dot((col.rgb - palColor.rgb), (col.rgb - palColor.rgb));
                    if(d < bestDistance)
                    {
                        bestDistance = d;
                        bestColor = palColor;
                    }
                }

                return bestColor;
            }
            ENDCG

            
            Stencil
            {
                Ref [_Stencil]   
                Comp Equal       
                Pass Replace     
            }
        }
    }
    FallBack "Diffuse"
}
