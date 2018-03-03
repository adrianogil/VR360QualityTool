Shader "Custom/EquiTint"
{
    Properties
    {
    	_TintColor ("Tint Color", Color) = (1, 1, 1, 1) // (R, G, B, A)
    	_InitialPosition ("Initial Position", Vector) = (0, 0, 0, 0) // (R, G, B, A)
    	_FinalPosition ("Final Position", Vector) = (1, 1, 1, 1) // (R, G, B, A)
        _MainTex("Image", 2D) = "white"
    }
    Subshader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 _TintColor;
            float2 _InitialPosition;
            float2 _FinalPosition;

            sampler _MainTex;

            struct vert_input
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vert_output
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            vert_output vert(vert_input i)
            {
                vert_output o;

                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;

                return o;
            }

            half4 frag(vert_output o) : COLOR
            {
            	if (o.uv.x >= _InitialPosition.x && o.uv.x <= _FinalPosition.x &&
            		o.uv.y >= _InitialPosition.y && o.uv.y <= _FinalPosition.y) {
            		return _TintColor * tex2D(_MainTex, o.uv);
            	}

                return tex2D(_MainTex, o.uv);
            }

            ENDCG
        }
    }
}