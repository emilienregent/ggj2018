// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "GGJ2018/VertexLit Blended" {
Properties {
    _Color ("Emissive Color", Color) = (.2,.2,.2,0)
    _MainTex ("Particle Texture", 2D) = "white" {}
}

SubShader {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
    Tags { "LightMode" = "Vertex" }
    Cull Off
    Lighting On
    Material { Emission [_Color] }
    ColorMaterial AmbientAndDiffuse
    ZWrite Off
    ColorMask RGB
    Blend SrcAlpha OneMinusSrcAlpha
    Pass {
        SetTexture [_MainTex] { combine primary * texture }
    }
}
}
