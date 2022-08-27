Shader "Proyecto/Sweep" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _param("Param.", Float) = 0.5
    }
        SubShader{
        Pass {
        CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform float _param;


        float4 frag(v2f_img i) : COLOR {

            i.uv.x += sin(i.uv.y * 20 + _Time * 200 * _param) * 0.01;

            float4 color = tex2D(_MainTex, i.uv);

            return color;
        }
        ENDCG
        }//Pass
        }//SubShader
}//Shader
