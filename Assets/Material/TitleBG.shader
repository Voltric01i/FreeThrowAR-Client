Shader "Unlit/TitleBG"
{
    Properties
    {
		_Split ("Split", float) = 8.0
		_Degree ("Degree", float) = 30.0
		_Color1 ("Color1", Color) = (1, 1, 0, 1)
		_Color2 ("Color2", Color) = (0, 1, 1, 1)
		_Color3 ("Color3", Color) = (1, 0, 1, 1)
    }

    SubShader
    {
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

			uniform float _Split;
			uniform float _Degree;
			uniform float4 _Color1;
			uniform float4 _Color2;
			uniform float4 _Color3;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : COLOR
            {
				i.uv.y += _Time.y / 4.;
				i.uv.y -= tan(radians(_Degree)) * i.uv.x;
				float id = fmod(floor(i.uv.y * _Split), 3.);
				
				float4 col = float4(1., 1., 1., 1.);

				if (id <= 0.1){
					col = smoothstep(0., 1., _Color1);
				}else if(id <= 1.1 && id >= 0.9){
					col = smoothstep(0., 1., _Color2);
				}else{
					col = smoothstep(0., 1., _Color3);
				}

				return col;
            }
            ENDCG
        }
    }
}
