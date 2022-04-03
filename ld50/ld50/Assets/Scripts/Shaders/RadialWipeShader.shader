Shader "Unlit/RadialWipeShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (1,1,1,1)
        _WipeProgress("Wipe Progress", Float) = 0
    }
    SubShader
    {
        Tags {
            "Queue" = "Transparent"
        }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        // make fog work
        #pragma multi_compile_fog

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

        sampler2D _MainTex;
        float4 _MainTex_ST;
        fixed4 _TintColor;
        float _WipeProgress;

        static const float pi = 3.141592653589793238462;

        v2f vert(appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            UNITY_TRANSFER_FOG(o,o.vertex);
            return o;
        }

        fixed4 frag(v2f i) : SV_Target
        {
            //step(tex2D(_MainTex, i.uv).a, 0.0001)

            //float2 denom = float2(0.5, 0.5) - float2(0.5, i.uv[1]) + (step(-1 * abs(0.5 - i.uv[1])))
            //float2 center
            //float numerator = 0.5 - step(-1 * abs(0.5 - i.uv[1]), 0) * 0.00001 // step BS is to avoid NaN
            float angle = atan2(
                0.5 - i.uv[0],
                0.5 - i.uv[1]
            ) + pi;

        // sample the texture
        fixed4 col = fixed4(
            _TintColor.r,
            _TintColor.g,
            _TintColor.b,
            //step(0.0001, tex2D(_MainTex, i.uv).a)
            //_WipeProgress
            //step(_WipeProgress, 0.25) * ()
            step(_WipeProgress, angle / (2 * pi)) * tex2D(_MainTex, i.uv).a
        );
        // apply fog
        UNITY_APPLY_FOG(i.fogCoord, col);
        return col;
    }
    ENDCG
    }
}
}