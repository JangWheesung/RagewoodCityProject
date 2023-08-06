Shader "Custom/SpriteLitWithDefaultParticle" {
    Properties{
        _Color("Main Color", Color) = (1, 1, 1, 1)
        _MainTex("Sprite Texture", 2D) = "white" {}
    }

        SubShader{
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;
            fixed4 _Color;

            struct Input {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutput o) {
                fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = texColor.rgb;
                o.Alpha = texColor.a;
            }
            ENDCG
    }
}