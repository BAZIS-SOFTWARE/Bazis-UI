using BazisGUI.SceneCore.VBO;
using Geometry;
using BazisGUI.SceneCore.Coordinates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace BazisGUI.SceneCore.Interfaces
{
    /// <summary>
    /// GLObj type
    /// </summary>
    public enum GLObjType
    {
        /// <summary>
        /// Узлы
        /// </summary>
        point,
        /// <summary>
        /// Линии
        /// </summary>
        line,
        /// <summary>
        /// Треугольники
        /// </summary>
        triangle = 4,
        /// <summary>
        /// Не распознан
        /// </summary>
        none
    }

    /// <summary>
    /// ViewProjection
    /// </summary>
    public enum ViewProjection
    {
        /// <summary>
        /// Parallel
        /// </summary>
        Parallel,
        /// <summary>
        /// Perspective
        /// </summary>
        Perspective,
    }


    /// <summary>
    /// ViewAxis
    /// </summary>
    public enum ViewAxis
    {
        /// <summary>
        /// X
        /// </summary>
        X, 
        /// <summary>
        /// Y
        /// </summary>
        Y, 
        /// <summary>
        /// Z
        /// </summary>
        Z, 
        /// <summary>
        /// XYZ
        /// </summary>
        XYZ
    }
    /// <summary>
    /// ViewPlane
    /// </summary>
    public enum ViewPlane
    {
        /// <summary>
        /// XY
        /// </summary>
        XY,
        /// <summary>
        /// XZ
        /// </summary>
        XZ,
        /// <summary>
        /// YZ
        /// </summary>
        YZ
    }
    /// <summary>
    /// ObjView
    /// </summary>
    public enum ObjView
    {
        /// <summary>
        /// surface
        /// </summary>
        Surface,
        /// <summary>
        /// linesSurface
        /// </summary>
        LinesSurface,
        /// <summary>
        /// lines
        /// </summary>
        Lines,
        /// <summary>
        /// points
        /// </summary>
        Points,
        /// <summary>
        /// none
        /// </summary>
        None
    }


    /// <summary>
    /// ISceneControl
    /// </summary>
    public interface ISceneControl
    {
        /// <summary>
        /// Initialization
        /// </summary>
        void Initialization();
        /// <summary>
        /// SceneControlExpandEvent
        /// </summary>
        event Action SceneControlExpandEvent;
        /// <summary>
        /// SceneControlFoldEvent
        /// </summary>
        event Action SceneControlFoldEvent;
        /// <summary>
        /// Camera
        /// </summary>
        ISceneCamera GetCamera();
        /// <summary>
        /// Угол сглаживания
        /// </summary>
        float ShadowAngle { get; set; }
        /// <summary>
        /// Включить сглаживание теней
        /// </summary>
        bool IsSmoothShadow { get; set; }
        /// <summary>
        /// Преобразует вектор в плоскости камеры в вектор координат модели.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        ModelVector3D GetSceneCoordOfScreenVector(float x, float y);
        /// <summary>
        /// Projection
        /// </summary>
        ViewProjection Projection { get; set; }
        /// <summary>
        /// RotationAxis
        /// </summary>
        ViewAxis RotationAxis { get; set; }
        /// <summary>
        /// MouseMoveFlag
        /// </summary>
        bool MouseMoveFlag { get; }
        /// <summary>
        /// RotationAngle
        /// </summary>
        float RotationAngle { get; set; }
        /// <summary>
        /// BackGroundColor
        /// </summary>
        Color BackGroundColor { get; set; }
        /// <summary>
        /// DisplayBasis
        /// </summary>
        bool DisplayBasis { get; set; }
        /// <summary>
        /// DisplayCompass
        /// </summary>
        bool DisplayCompass { get; set; }
        /// <summary>
        /// SelectionColor
        /// </summary>
        Color SelectionColor { get; set; }
        /// <summary>
        /// IsCutting
        /// </summary>
        bool IsCutting { get; set; }
        /// <summary>
        /// IsLighting
        /// </summary>
        bool IsLighting { get; set; }
        /// <summary>
        /// IsClipPlane
        /// </summary>
        bool IsClipPlane { get; set; }
        /// <summary>
        /// CreateClipPlane
        /// </summary>
        /// <param name="clipPlane"></param>
        void ChangeClipPlane(Plane clipPlane);

        /// <summary>
        /// IsBlending
        /// </summary>
        bool IsBlending { get; set; }
        /// <summary>
        /// SceneWidth
        /// </summary>
        int SceneWidth { get; }
        /// <summary>
        /// SceneHeigth
        /// </summary>
        int SceneHeight { get;}
        /// <summary>
        /// ScaleFactor
        /// </summary>
        /// <summary>
        /// ScaleFactor
        /// </summary>
        float ScaleFactor { get; }
        /// <summary>
        /// SetRotationCentre
        /// </summary>
        /// <param name="modelPoint"></param>
        void SetRotationCentre(ModelPoint3D modelPoint);
        /// <summary>
        /// IsVBObjectShown
        /// </summary>
        /// <param name="objsName"></param>
        /// <returns></returns>
        bool IsVBObjectShown(string objsName);
        /// <summary>
        /// SwitchOffVBObject
        /// </summary>
        /// <param name="objsName"></param>
        void SwitchOffVBObject(string objsName);
        /// <summary>
        /// SwitchOnVBObject
        /// </summary>
        /// <param name="objsName"></param>
        void SwitchOnVBObject(string objsName);
        /// <summary>
        /// FitObjectsToScreen
        /// </summary>
        void FitObjectsToScreen();
        /// <summary>
        /// RotateObjs
        /// </summary>
        void RotateObjs();
        /// <summary>
        /// ScaleObjs
        /// </summary>
        /// <param name="scaleFactor"></param>
        void ScaleObjs(float scaleFactor);
        /// <summary>
        /// PlaneObjs
        /// </summary>
        /// <param name="plane"></param>
        void PlaneObjs(ViewPlane plane);
        /// <summary>
        /// DisplayObjects
        /// </summary>
        void DisplayObjects();
        /// <summary>
        /// GetVBObjsID
        /// </summary>
        /// <returns></returns>       
        IEnumerable<IVBObject> GetVBObjs();
        /// <summary>
        /// DeleteVBObjects
        /// </summary>
        /// <param name="objsName"></param>
        /// <returns></returns>
        bool DeleteVBObjects(string objsName);
        /// <summary>
        /// DeleteAllVBObjects
        /// </summary>
        void DeleteAllVBObjects();
        /// <summary>
        /// ChangeViewModeVBObjects
        /// </summary>
        /// <param name="objsName"></param>
        /// <param name="objView"></param>
        void ChangeViewModeVBObjects(string objsName, ObjView objView);
        /// <summary>
        /// HideGeometryObj
        /// </summary>
        /// <param name="searchMethod"></param>
        void HideGeometryObj(string searchMethod);
        /// <summary>
        /// FindGeometryObj
        /// </summary>
        /// <param name="searchMethod"></param>
        /// <returns></returns>
        bool FindGeometryObj(string searchMethod);
        /// <summary>
        /// HideAllGeometryObjs
        /// </summary>
        void HideAllGeometryObjs();
        /// <summary>
        /// HideDisplayText3D
        /// </summary>
        void HideDisplayText3D();
        /// <summary>
        /// DisplayText3D
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        /// <param name="coord"></param>
        void DisplayText3D(string str, Color color, Point3D coord);
        /// <summary>
        /// DisplayText2D
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        /// <param name="coord"></param>
        void DisplayText2D(string str, Color color, Point2D coord);
        /// <summary>
        /// HideDisplayText2D
        /// </summary>
        void HideDisplayText2D();
        /// <summary>
        /// CreateScaleObject
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="ranges"></param>
        /// <param name="title"></param>
        /// <param name="comments"></param>
        ISceneScale CreateScaleObject(float min, float max, decimal ranges, string title, string comments);
        /// <summary>
        /// Display geometry object
        /// </summary>
        /// <param name="scale"></param>
        void DisplaySceneScale(ISceneScale scale);
        /// <summary>
        /// Display local frame
        /// </summary>
        /// <param name="frame"></param>
        void DisplayLocalFrame(Frame frame);
        /// <summary>
        /// Display path
        /// </summary>
        /// <param name="points"></param>
        void DisplayPath(Point3D[] points);
        /// <summary>
        /// Display line
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="objColor"></param>
        void DisplayLine(Point3D p0, Point3D p1, Color objColor);
        /// <summary>
        /// Display spiral
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="objColor"></param>
        void DisplaySpiral(Point3D p0, Point3D p1, Color objColor);
        /// <summary>
        /// DisplayConus
        /// </summary>
        /// <param name="UpperDiam"></param>
        /// <param name="BottomDiam"></param>
        /// <param name="length"></param>
        /// <param name="frame"></param>
        void DisplayConus(float UpperDiam, float BottomDiam, float length, Frame frame);
        /// <summary>
        /// Display sphere
        /// </summary>
        /// <param name="width"></param>
        /// <param name="frame"></param>
        void DisplaySphere(float width, Frame frame);
        /// <summary>
        /// Display distance
        /// </summary>
        /// <param name="line"></param>
        void DisplayDistance(Segment3D line);
        /// <summary>
        /// FindObj
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        IVBObject FindVBObj(string objName);
        /// <summary>
        /// LightTranslateX
        /// </summary>
        float LightTranslateX { get; set; }
        /// <summary>
        /// LightTranslateY
        /// </summary>
        float LightTranslateY { get; set; }
        /// <summary>
        /// LightTranslateZ
        /// </summary>
        float LightTranslateZ { get; set; }
        /// <summary>
        /// LightAttenuation
        /// </summary>
        float LightAttenuation { get; set; }
        /// <summary>
        /// Обновляем матрицу проекции
        /// </summary>
        void UpdateProjection();
        /// <summary>
        /// SetTransparency
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="alpha"></param>
        void SetTransparency(string objName, int alpha);
        /// <summary>
        /// HideReflectionPlane
        /// </summary>
        void HideReflectionPlane();
        /// <summary>
        /// Обновляет функцию-рендера плоскости отражения
        /// </summary>
        /// <param name="objName">[In]Оригинальный VBObject</param>
        /// <param name="coeff">[In]Плоскость отражения</param>
        void DisplayReflectionPlane(string objName, float[] coeff);
        /// <summary>
        /// Копирует VBObject, все VBO-буфферы в копии имеют уникальные значения
        /// </summary>
        /// <param name="original">[In]Оригинальный VBObject</param>
        /// <param name="copyName">[In]Имя для объекта копии</param>
        void CopyVBObjects(VBObject original, string copyName);
    }
}
