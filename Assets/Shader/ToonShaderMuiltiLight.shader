/* Custom toon shader is based on toon shader of  Erik Roystan Ross
** releases on the website https://roystan.net/articles/toon-shader.html
** modified by JFrap12 from Reddit community 
** modified further by Hoang Anh Huy Luu
*/

Shader "Custom/ToonShaderMuiltiLight" 
{
    Properties{
        [Header(Base Parameters)]
        _Color("Tint", Color) = (0, 0, 0, 1)
        _MainTex("Texture", 2D) = "white" {}
        [HDR] _Emission("Emission", color) = (0, 0, 0, 1)

        [HDR]
        _SpecularColor("Specular Color", Color) = (0.9, 0.9, 0.9, 1)
            
            // Controls the size of the specular reflection.
            _Glossiness("Glossiness", Float) = 32
            [HDR]
            _RimColor("Rim Color", Color) = (1,1,1,1)
            _RimAmount("Rim Amount", Range(0, 1)) = 0.716
                // Control how smoothly the rim blends when approaching unlit
                // parts of the surface.
                _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
    }
        SubShader{
            Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}

            CGPROGRAM

            #pragma surface surf Stepped fullforwardshadows
            #pragma target 3.0

            sampler2D _MainTex;
            fixed4 _Color;
            half3 _Emission;

            float4 _SpecularColor;
            float _Glossiness;

            float4 _RimColor;
            float _RimAmount;
            float _RimThreshold;

            float3 worldPos;

            float4 LightingStepped(SurfaceOutput s, float3 lightDir, half3 viewDir, float shadowAttenuation) {
                float shadow = shadowAttenuation;

                s.Normal = normalize(s.Normal);

                //calculate the lighting based on multiple sources of lights
                float diff = dot(s.Normal, lightDir);

                float towardsLightChange = fwidth(diff);

                // Partition the intensity into light and dark, smoothly interpolated
                // between the two to avoid a jagged break.
                float lightIntensity = smoothstep(0, towardsLightChange, diff);
                float3 diffuse = _LightColor0.rgb * lightIntensity * s.Albedo;

                float diffussAvg = (diffuse.r + diffuse.g + diffuse.b) / 3;

                //Calculate the specular reflection
                float3 halfVector = normalize(viewDir + lightDir);
                float NdotH = dot(s.Normal, halfVector);

                // Adjust the size of _Glossiness 
                float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
                float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
                float3 specular = specularIntensitySmooth * _SpecularColor.rgb * diffussAvg;

                //Calculate rim lighting 
                float rimDot = 1 - dot(viewDir, s.Normal);

                //Make sure the rim lighting smootly blend to the outside of object
                float rimIntensity = rimDot * pow(dot(lightDir, s.Normal), _RimThreshold);
                rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
                float3 rim = rimIntensity * _RimColor.rgb * diffussAvg;

                //Caculate the color of the object
                float4 color;
                color.rgb = (diffuse + specular + rim) * shadow;
                color.a = s.Alpha;
                return color;
            }

            struct Input {
                float2 uv_MainTex;
                float3 worldPos;
            };

            void surf(Input i, inout SurfaceOutput o) {
                worldPos = i.worldPos;
                fixed4 col = tex2D(_MainTex, i.uv_MainTex);
                col *= _Color;
                o.Albedo = col.rgb;
                o.Alpha = col.a;

                o.Emission = _Emission;
            }
            ENDCG
            }
                FallBack "Standard"
}
