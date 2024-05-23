Shader "Universal Render Pipeline/QuarterPlaneRing"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1,1,1,0.5)
        _OuterRadius("Outer Radius", Range(0.1, 10)) = 1.0
        _InnerRadius("Inner Radius", Range(0.0, 10)) = 0.5
        _QuarterAngle("Quarter Angle", Range(0.0, 180.0)) = 45.0
        _AddHighlight("Add Highlight", Range(0.0, 1.0)) = 1
        _HighlightColor("Highlight Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            HLSLPROGRAM
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

            float4 _MainColor;
            float _OuterRadius;
            float _InnerRadius;
            float _QuarterAngle;
            float _AddHighlight;
            float4 _HighlightColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float distanceToCenter = distance(i.uv, center);
                float angle = atan2(i.uv.y - center.y, i.uv.x - center.x) * (180.0 / 3.14159);

                // Check if the pixel is inside the quarter ring based on angle
                float startAngle = 90.0 - _QuarterAngle/2;
                float endAngle = 90 + _QuarterAngle/2;
                if (angle >= startAngle && angle <= endAngle && distanceToCenter >= _InnerRadius && distanceToCenter <= _OuterRadius)
                {
                    // Apply main color
                    fixed4 color = _MainColor;
                    float quarterDistance = (_OuterRadius - _InnerRadius) * 1;
                    // Add highlight if enabled
                    if (_AddHighlight > 0.0 && distanceToCenter >= _InnerRadius + quarterDistance * (1.0 - _AddHighlight))
                    {
                        return _HighlightColor;
                    }

                    return color;
                }
                else
                {
                    // Return transparent black if pixel is outside the quarter ring
                    return fixed4(0.0, 0.0, 0.0, 0.0);
                }
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
