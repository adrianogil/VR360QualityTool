Shader "Custom/ToneRange"
{
    Properties
    {
    	_ToneValues ("Number of Tones", Float) = 10
        _MainTex("Image", 2D) = "white"
    }
    Subshader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float _ToneValues;

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

            float4 frag(vert_output o) : COLOR
            {
                float rangeSize = 1 / _ToneValues;  
                float c = max(0, ceil(o.uv.y * _ToneValues) - 1) / _ToneValues;
                c = c + 0.2 * rangeSize;

                return float4(c,c,c,1);
            }

            ENDCG
        }
    }
}