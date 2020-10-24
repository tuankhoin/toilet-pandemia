Shader "Custom/TransparentShader"
{
    Properties {
        _Alpha ("Alpha", Range(0,1)) = 0.3
       _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
       _CutOff("Cut off", Range(0,1)) = 0.1
    }
    SubShader {
       Tags { "Queue"="Transparent" "RenderType"="Transparent" }
       Blend SrcAlpha OneMinusSrcAlpha
       AlphaTest Greater 0.1
       Pass {   
          CGPROGRAM 
 
          #pragma vertex vert  
          #pragma fragment frag 
          #include "UnityCG.cginc"
  
          // User-specified uniforms            
          sampler2D _MainTex;
          float4 _MainTex_ST;
          uniform float _CutOff;
          uniform float _Alpha; 
  
          struct vertIn {
            float4 vertex : POSITION;
            float2 tex : TEXCOORD0;
          };
          struct vertOut {
            float4 pos : SV_POSITION;
            float2 tex : TEXCOORD0;
          };
  
          vertOut vert(vertIn input) 
          {
            vertOut output;
  
            output.pos = UnityObjectToClipPos(input.vertex);
  
            output.tex = TRANSFORM_TEX(input.tex, _MainTex);
  
            return output;
          }
  
          float4 frag(vertOut input) : COLOR
          {
  
            float4 color = tex2D(_MainTex, float2(input.tex.xy));   
            
            if(color.a < _CutOff) discard;
            else color.a = _Alpha;
            
            return color;
          }
  
          ENDCG
       }
    }
}
