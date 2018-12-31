Shader "Marco/Mandelbrot"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Area("Area",vector) = (0,0,4,4) // first two numbers are center of area we are oing to render the other two numbers are size
		_Angle("Angle",range(-3.141,3.10415)) = 0
		_Iter("Iterations",range(1,510)) = 255
		_BreakCondition("Break Condition",range(0,3)) = 2
		_ComplexMultiplier("Complex Multiplier",range(1,5)) = 2
	}
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			float _ComplexMultiplier;
			float _BreakCondition;
			float _Iter;
			float4 _Area;
			float _Angle;
            sampler2D _MainTex;

			float2 rot(float2 p,float2 pivot,float a)
			{
			    float s = sin(a);
				float c = cos(a);

				p -= pivot;
				p =float2(p.x*c-p.y*s,p.x*s+p.y*c);
				p += pivot;

				return p;
			}

            fixed4 frag (v2f i) : SV_Target
            {
                float2 c = _Area.xy + (i.uv-0.5)*_Area.zw; // start position ( uv cordintates goes from zero to one)
				c = rot(c,_Area.xy,_Angle);
				float2 z; // to keep track of pixel jmping along the screen
				float iter; // to keep track of iteration we are on

				for(iter=0; iter < _Iter; iter++)
				{
					z = float2(z.x*z.x-z.y*z.y, _ComplexMultiplier*z.x*z.y)+ c;
					if(length(z) > _BreakCondition)
					{
						break;
					}
				}

				return iter/_Iter; // get score of steps done divides max numbere to win 
                //return col;
            }
            ENDCG
        }
    }
}
