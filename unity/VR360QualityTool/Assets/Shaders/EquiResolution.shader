Shader "Custom/EquiResolution"
{
    Properties
    {
    	_CameraPosition ("Camera Position", Vector) = (1, 1, 1, 1) // (R, G, B, A)
    	_UpDirection ("Initial Position", Vector) = (0, 0, 0, 0) // (R, G, B, A)
    	_ForwardDirection ("Final Position", Vector) = (1, 1, 1, 1) // (R, G, B, A)
        _FoV ("Final Position", Float) = 0.1
    }
    Subshader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom

            float3 _CameraPosition;
            float3 _UpDirection;
            float3 _ForwardDirection;

            float _FoV;

            struct vert_input
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vert_output
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 wPos : TEXCOORD1;    // World position
            };


            struct geo_output
            {
                float4 color : COLOR;
            };

            vert_output vert(vert_input i)
            {
                vert_output o;

                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
                o.wPos = mul(unity_ObjectToWorld, i.vertex).xyz;

                return o;
            }

            [maxvertexcount(24)]
            void geom(triangle vert_output IN[3], inout TriangleStream<geo_output> tristream)
            {
                geo_output o;

                o.color = float4(1,0,0,1);
                tristream.Append(o);

                o.color = float4(1,0,0,1);
                tristream.Append(o);

                o.color = float4(1,0,0,1);
                tristream.Append(o);
            }


            half4 frag(geo_output o) : COLOR
            {
            	return o.color;
            }

            ENDCG
        }
    }
}