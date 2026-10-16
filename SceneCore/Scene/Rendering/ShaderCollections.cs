using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.Scene
{
    /// <summary>
    /// Класс для представления исходных кодов шейдеров в виде массива строк
    /// </summary>
    public static class ShaderCollections
    {
        /*/// <summary>
        /// averageColorRenderer вершинный шейдер для объектов типа Surface, LineSurface
        /// </summary>
        public static string[] averageColorSurfaceVertex = new string[]///БОЛЬШЕ НЕ НУЖЕН
        {
            "#version 120\n",
            "varying vec3 vNormal;\n",
            "varying vec3 vPos;\n",
            "void main(){\n",
            "   gl_Position = ftransform();\n",
            "   vNormal = normalize(gl_NormalMatrix * gl_Normal);\n",
            "   float diffuse = abs(vNormal.z);\n",
            "   gl_TexCoord[0].xyz = vec3(gl_Vertex.xy, diffuse);\n",
            "   vec4 pos = gl_ModelViewMatrix * gl_Vertex;\n",
            "   vPos = pos.xyz / pos.w;\n",
            "   gl_FrontColor = gl_Color;\n",
            "   gl_BackColor = gl_Color;\n",
            "   gl_ClipVertex = gl_ModelViewMatrix * gl_Vertex;\n",
            "}"
        };
        /// <summary>
        /// averageColorRenderer фрагментный шейдер для объектов типа Surface, LineSurface
        /// </summary>
        public static string[] averageColorSurfaceFragment = new string[]///БОЛЬШЕ НЕ НУЖЕН
        {
            "#version 120\n",
            "vec4 CalculateLighting(int index);\n",
            "varying vec3 vNormal;\n",
            "varying vec3 vPos;\n",
            "uniform float isLighting;\n",
            "void main(){\n",
            "   vec3 finalFrag = mix(gl_Color.rgb * gl_TexCoord[0].z, CalculateLighting(0).rgb, isLighting);\n",
            "   gl_FragData[0] = vec4(finalFrag * gl_Color.a, gl_Color.a);\n",
            "   gl_FragData[1].r = 1.0;\n",
            "}",
            "vec4 CalculateLighting(int index){\n",
            "   vec3 lightVec = gl_LightSource[index].position.xyz - vPos.xyz;\n",
            "   float dist = length(lightVec);\n",
            "   lightVec = normalize(lightVec);\n",
            "   float attenuation = 1.0 / (gl_LightSource[index].constantAttenuation + \n",
            "                              gl_LightSource[index].linearAttenuation * dist + \n",
            "                              gl_LightSource[index].quadraticAttenuation * dist * dist);\n",
            "   vec3 normal = gl_FrontFacing ? vNormal : -vNormal;\n",
            "   float dInt = max(0.0, dot(normal, lightVec));\n",
            "   vec3 ambient = vec3(0.2) * gl_LightSource[index].diffuse.rgb;\n",
            "   vec3 diffuse = gl_LightSource[index].diffuse.rgb * dInt * attenuation;\n",
            "   return vec4((ambient + diffuse) * gl_Color.rgb, gl_Color.a);\n",
            "   }"
        };*/
        /// <summary>
        /// averageColorRenderer вершинный шейдер для финального смешивания всех видов объектов полученных в кадре
        public static string[] averageColorFinalBlendVertex = new string[]
        {
            "#version 120\n",
            "void main(){\n",
            "   gl_Position = gl_ModelViewMatrix * gl_Vertex;\n",
            "}"
        };
        /// <summary>
        /// averageColorRenderer фрагментный шейдер для финального смешивания всех видов объектов полученных в кадре
        /// Исходники тут:
        /// https://developer.download.nvidia.com/SDK/10/opengl/src/dual_depth_peeling/doc/DualDepthPeeling.pdf
        /// </summary>
        public static string[] averageColorFinalBlendFragment = new string[]
        {
            "#version 120\n",
            "void sort(in float depths[5], inout vec4 colors[5]);\n",
            "uniform sampler2DRect nodesDepth;\n",
            "uniform sampler2DRect nodesColor;\n",
            "uniform sampler2DRect linesDepth;\n",
            "uniform sampler2DRect linesColor;\n",
            "uniform sampler2DRect frameDepth;\n",
            "uniform sampler2DRect frameColor;\n",
            "uniform sampler2DRect opaqueDepth;\n",
            "uniform sampler2DRect opaqueColor;\n",
            "uniform sampler2DRect transpDepth;\n",
            "uniform sampler2DRect transpColor;\n",
            "uniform sampler2DRect transpCount;\n",
            "uniform vec3 backColor;\n",
            "void main(){\n",
            "   float nDepth = texture2DRect(nodesDepth, gl_FragCoord.xy).r;\n",
            "   vec4 nColor = texture2DRect(nodesColor, gl_FragCoord.xy);\n",
            "   float lDepth = texture2DRect(linesDepth, gl_FragCoord.xy).r;\n",
            "   vec4 lColor = texture2DRect(linesColor, gl_FragCoord.xy);\n",
            "   float fDepth = texture2DRect(frameDepth, gl_FragCoord.xy).r;\n",
            "   vec4 fColor = texture2DRect(frameColor, gl_FragCoord.xy);\n",
            "   float oDepth = texture2DRect(opaqueDepth, gl_FragCoord.xy).r;\n",
            "   vec4 oColor = texture2DRect(opaqueColor, gl_FragCoord.xy);\n",
            "   float tDepth = texture2DRect(transpDepth, gl_FragCoord.xy).r;\n",
            "   vec4 tColor = texture2DRect(transpColor, gl_FragCoord.xy);\n",
            "   float tCount = texture2DRect(transpCount, gl_FragCoord.xy).r;\n",
            "   if(tCount != 0){\n",
            "       tColor.a = max(0.000001, tColor.a - 0.00001);\n",
            "       tColor = vec4(tColor.rgb / tColor.a, 1 - pow(1 - tColor.a / tCount, log(tCount)));\n",//Более резкое затухание
            //"     tColor = vec4(tColor.rgb / tColor.a, 1 - pow(1 - tColor.a / tCount, tCount));\n",
            "   }\n",
            "   float depths[5] = float[5](fDepth, tDepth, lDepth, nDepth, oDepth);\n",
            "   vec4 colors[5] = vec4[5](fColor, tColor, lColor, nColor, oColor);\n",
            "   sort(depths, colors);\n",
            "   gl_FragColor.rgb = colors[0].rgb * colors[0].a + (1 - colors[0].a) * ",
            "                     (colors[1].rgb * colors[1].a + (1 - colors[1].a) * ",
            "                     (colors[2].rgb * colors[2].a + (1 - colors[2].a) * ",
            "                     (colors[3].rgb * colors[3].a + (1 - colors[3].a) * ",
            "                     (colors[4].rgb * colors[4].a + (1 - colors[4].a) * backColor))));\n",
            // Кадр непрозрачный: Avalonia смешивает кадр OpenGL с окном по альфа-каналу,
            // и незаданная альфа делала всю сцену прозрачной (GLControl в WinForms её игнорировал).
            "   gl_FragColor.a = 1.0;\n",
            "}\n",
            "   void sort(in float depths[5], inout vec4 colors[5]){\n",
            "       for(int i = 0; i < 5; ++i){\n",
            "           float cDepth = depths[i];\n",
            "           vec4 cColor = colors[i];\n",
            "           int j = i;\n",
            "           for(; j - 1 >= 0 && cDepth < depths[j - 1]; --j){\n",
            "               depths[j] = depths[j - 1];\n",
            "               colors[j] = colors[j - 1];\n",
            "           }\n",
            "           depths[j] = cDepth\n;",
            "           colors[j] = cColor\n;",
            "       }\n",
            "   }\n",
        };
        /// <summary>
        /// Вершинный шейдер клиппера
        /// </summary>
        public static string[] clipPlaneVertex = new string[]
        {
            "#version 120\n",
            "uniform mat4 modelMatrix;\n",
            "void main(){\n",
            "   gl_Position = modelMatrix * gl_Vertex;\n",
            "}"
        };
        /// <summary>
        /// Геометрический шейдер клиппера
        /// </summary>
        public static string[] clipPlaneGeometry = new string[]
        {
            "#version 150 compatibility\n",
            "layout(triangles) in;\n",
            "layout(line_strip, max_vertices = 14) out;\n",
            "const vec4 clipPlane = vec4(0, 0, -1, 0);\n",
            "uniform float normalSize;\n",
            "void drawArrow(vec3 begin);\n",
            "void main(){\n",
            "   vec4 clipEquat;\n",
            "   clipEquat.xyz = gl_NormalMatrix * clipPlane.xyz;\n",
            "   vec3 cameraPos = gl_ModelViewMatrix[3].xyz;\n",
            "   clipEquat.w = -dot(clipEquat.xyz, cameraPos);\n",
            "   int emitVert[4] = int[4](-1,-1,-1,-1);\n",
            "   vec3 begin[4];\n",
            "   vec3 end[4];\n",
            "   begin[0] = gl_in[0].gl_Position.xyz;\n",
            "   end[0] = gl_in[1].gl_Position.xyz - gl_in[0].gl_Position.xyz;\n",
            "   begin[1] = gl_in[1].gl_Position.xyz;\n",
            "   end[1] = gl_in[2].gl_Position.xyz - gl_in[1].gl_Position.xyz;\n",
            "   begin[2] = gl_in[2].gl_Position.xyz;\n",
            "   end[2] = -end[0];\n",
            "   begin[3] = begin[2] + end[2];\n",
            "   end[3] = -end[1];\n",
            "   for(int i = 0; i < 4; ++i){\n",
            "       if(abs(dot(end[i], clipEquat.xyz)) > 1e-4){\n",
            "           float clipV = dot(clipEquat.xyz, begin[i]) + clipEquat.w;\n",
            "           float dotDir = dot(clipEquat.xyz, end[i]);\n",
            "           float t0 = -clipV / dotDir;\n",
            "           bvec2 status = bvec2(t0 < 0, t0 > 1);\n",
            "           if(!any(status)){\n",
            "               begin[i] = begin[i] + t0 * end[i];\n",
            "               gl_Position = gl_ProjectionMatrix * vec4(begin[i], 1);\n",
            "               EmitVertex();\n",
            "               emitVert[i] = i;\n",
            "           }\n",
            "       }\n",
            "   }\n",
            "   EndPrimitive();\n",
            "   for(int i = 0; i < 4; ++i){\n",
            "       int index = emitVert[i];\n",
            "       if(index != -1){\n",
            "           gl_Position = gl_ProjectionMatrix * vec4(begin[index],1);\n",
            "           EmitVertex();\n",
            "           vec3 endNormal = clipPlane.xyz  * mat3(gl_ModelViewMatrixTranspose);\n",
            "           vec3 pNormal = begin[index] + endNormal * normalSize;\n",
            "           gl_Position = gl_ProjectionMatrix * vec4(pNormal, 1);\n",
            "           EmitVertex();\n",
            "           EndPrimitive();\n",
            "           drawArrow(pNormal);\n",
            "       }\n",
            "   }\n",
            "}\n",
            "void drawArrow(vec3 begin){\n",
            "   vec3 rightDir = vec3(0.354, 0, 0.935) * mat3(gl_ModelViewMatrixTranspose) * normalSize * 0.250f;\n",
            "   gl_Position = gl_ProjectionMatrix * vec4(begin, 1);\n",
            "   EmitVertex();\n",
            "   gl_Position = gl_ProjectionMatrix * vec4(begin + rightDir, 1);\n",
            "   EmitVertex();\n",
            "   EndPrimitive();\n",
            "   vec3 leftDir = vec3(-0.354, 0, 0.935) * mat3(gl_ModelViewMatrixTranspose) * normalSize * 0.250f;\n",
            "   gl_Position = gl_ProjectionMatrix * vec4(begin, 1);\n",
            "   EmitVertex();\n",
            "   gl_Position = gl_ProjectionMatrix * vec4(begin + leftDir, 1);\n",
            "   EmitVertex();\n",
            "   EndPrimitive();\n",
            "}"
        };
        /// <summary>
        /// Фрагментый шейдер клиппера
        /// </summary>
        public static string[] clipPlaneFragment = new string[]
        {
            "#version 120\n",
            "void main(){\n",
            "   gl_FragColor = vec4(0, 1, 0, 1);\n",
            "}"
        };
        /// <summary>
        /// Базовый вершинный шейдер
        /// </summary>
        public static string[] baseVertex = new string[]
        {
            "#version 130\n",
            "#define CLIP_MODE\n",
            "#define STANDART_CLIPPING\n",
            "vec4 CalculateLighting(int index);\n",
            "#ifdef CLIP_MODE\n",
            "   attribute vec3 inLeftUp;\n",
            "   attribute vec3 inRightDown;\n",
            "   attribute float wire;\n",
            "   varying float outWire;\n",
            "   varying vec4 leftUp;\n",
            "   varying vec4 rightDown;\n",
            "   varying int inVertexId;\n",
            "#endif\n",
            "varying vec4 colors;\n",
            "#ifdef CLIP_3D_POINTS\n",
            "   uniform vec4 pointsColor;\n",
            "#endif\n",
            "   uniform float isLighting;\n",
            "void main(){\n",
            "#ifdef CLIP_MODE\n",
            "   leftUp = gl_ModelViewMatrix * vec4(inLeftUp, 1);\n",
            "   rightDown = gl_ModelViewMatrix * vec4(inRightDown, 1);\n",
            "   outWire = wire;\n",
            "   inVertexId = gl_VertexID;\n",
            "#endif\n",
            "   gl_ClipVertex = gl_ModelViewMatrix * gl_Vertex;\n",
            "   gl_Position = ftransform();\n",
            "   gl_FrontColor = gl_Color;\n",
            "   gl_BackColor = gl_Color;\n",
            "#ifdef CLIP_3D_POINTS\n",
            "   colors = pointsColor;\n",
            "#else\n",
            "   colors = mix(gl_Color, CalculateLighting(0), isLighting);\n",
            "#endif\n",
            "}\n",
            "vec4 CalculateLighting(int index){\n",
            "   vec3 vPos = (gl_ModelViewMatrix * gl_Vertex).xyz;\n",
            "   vec3 lightVec = gl_LightSource[index].position.xyz - vPos.xyz;\n",
            "   float dist = length(lightVec);\n",
            "   lightVec = normalize(lightVec);\n",
            "   vec3 normal = normalize(gl_NormalMatrix * gl_Normal);\n",
            "   float dir = dot(normalize(-vPos), normal);\n",//Эти две строки отвечают за разворот нормалей
            "   normal = mix(-normal,normal,float(dir >= 0));\n",//Для bpf приходят неправильные нормали(для остального все работает), поэтому эти две строки оставляю
            "   float attenuation = 1.0 / (gl_LightSource[index].constantAttenuation + \n",
            "                              gl_LightSource[index].linearAttenuation * dist + \n",
            "                              gl_LightSource[index].quadraticAttenuation * dist * dist);\n",
            "   float dInt = max(0.0, dot(normal, lightVec));\n",
            "   vec3 ambient = vec3(0.2) * gl_LightSource[index].diffuse.rgb;\n",
            "   vec3 diffuse = gl_LightSource[index].diffuse.rgb * dInt * attenuation;\n",
            "   return vec4((ambient + diffuse) * gl_Color.rgb, gl_Color.a);\n",
            "}"
        };
        /// <summary>
        /// Геометрический шейдер сохранения 3д элементов в слое
        /// </summary>
        public static string[] keepElementsGeometry = new string[]
        {
            "#version 150 compatibility\n",
            "#define SURFACE\n",
            "#define KEEP_ELEMENT\n",
            "layout(triangles) in;\n",
            "#ifdef SURFACE\n",
            "   layout(triangle_strip, max_vertices = 3) out;\n",
            "#elif defined(WIREFRAME)\n",
            "   layout(line_strip, max_vertices = 6) out;\n",
            "   in float outWire[];\n",
            "#else\n",//Points case
            "   layout(points, max_vertices = 3) out;\n",
            "#endif\n",
            "in vec4 colors[];\n",
            "in vec4 leftUp[];\n",
            "in vec4 rightDown[];\n",
            "in int inVertexId[];\n",
            "out int vertexId;\n",
            "out vec4 outColor;\n",
            "uniform vec4 clipEquat;\n",
            "#ifndef KEEP_ELEMENT\n",
            "   uniform float layerThickness;\n",
            "   uniform float scaleFactor;\n",
            "#endif\n",
            "void main(){\n",
            "   float luDist = dot(leftUp[0], clipEquat);\n",
            "   float rdDist = dot(rightDown[0], clipEquat);\n",
            "#ifdef KEEP_ELEMENT\n",
            "   bool status = luDist >= 0 || rdDist >= 0;\n",
            "#else\n",
            "   float nLen = length(clipEquat.xyz) * scaleFactor;\n",
            "   bool status = abs(luDist) < nLen * layerThickness || abs(rdDist) < nLen * layerThickness;\n",
            "#endif\n",
            "#ifdef SURFACE\n",
            "   if(status){\n",
            "       for(int i = 0; i < 3; ++i){\n",
            "           outColor = colors[i];\n",
            "           vertexId = inVertexId[i];\n",
            "           gl_Position = gl_in[i].gl_Position;\n",
            "           EmitVertex();\n",
            "       }\n",
            "       EndPrimitive();\n",
            "   }\n",
            "#elif defined(WIREFRAME)\n",
            "   if(status){\n",
            "       for(int i = 1; i < 3; ++i){\n",
            "           if(outWire[i - 1] > 0.5)\n",
            "           {\n",
            "               outColor = colors[i - 1];\n",
            "               vertexId = inVertexId[i];\n",
            "               gl_Position = gl_in[i - 1].gl_Position;\n",
            "               EmitVertex();\n",
            "               outColor = colors[i];\n",
            "               vertexId = inVertexId[i];\n",
            "               gl_Position = gl_in[i].gl_Position;\n",
            "               EmitVertex();\n",
            "               EndPrimitive();\n",
            "           }\n",
            "       }\n",
            "       if(outWire[2] > 0.5)\n",
            "       {\n",
            "           outColor = colors[2];\n",
            "           vertexId = inVertexId[2];\n",
            "           gl_Position = gl_in[2].gl_Position;\n",
            "           EmitVertex();\n",
            "           outColor = colors[0];\n",
            "           vertexId = inVertexId[0];\n",
            "           gl_Position = gl_in[0].gl_Position;\n",
            "           EmitVertex();\n",
            "           EndPrimitive();\n",
            "       }\n",
            "   }\n",
            "#else\n",
            "   if(status){\n",
            "       for(int i = 0; i < 3; ++i){\n",
            "           outColor = colors[i];\n",
            "           vertexId = inVertexId[i];\n",
            "           gl_Position = gl_in[i].gl_Position;\n",
            "           EmitVertex();\n",
            "           EndPrimitive();\n",
            "       }\n",
            "   }\n",
            "#endif\n",
            "}"
        };
        /*/// <summary>
        /// Фрагментый шейдер сохранения 3д элементов в слое
        /// </summary>
        public static string[] keepElementsFragment = new string[]
        {
            "#version 120\n",
            "varying vec4 outColor;\n",
            "void main(){\n",
            "   gl_FragColor = outColor;\n",
            "}"
        };*/
        /// <summary>
        /// Фрагментый шейдер сохранения 3д элементов в слое
        /// </summary>
        public static string[] baseFragment = new string[]
        {
            "#version 120\n",
            "#define TRANSPARENT\n",
            "#ifdef TRANSPARENT\n", //Режим прозрачности без сечений, передача напрямую из вершинного шейдера
            "   varying vec4 colors;\n",
            "#else\n",//Случай обработки геометрическим шейдером
            "   varying vec4 outColor;\n",
            "#endif\n",
            "void main(){\n",
            "#ifdef TRANSPARENT\n",
            "   gl_FragData[0] = vec4(colors.rgb * colors.a, colors.a);\n",
            "   gl_FragData[1].r = 1.0;\n",
            "#elif defined(TRANSPARENT_WITH_CLIP)\n",
            "   gl_FragData[0] = vec4(outColor.rgb * outColor.a, outColor.a);\n",
            "   gl_FragData[1].r = 1.0;\n",
            "#else\n",
            "   gl_FragColor = outColor;\n",
            "#endif\n",
            "}"
        };
    }
}
