using WinGUI.Scene.EventsArgs;
using WinGUI.Scene.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinGUI.Scene.VBO
{
    public class VBOController
    {
        public Action<object, MessageEventArgs> MessageEvent;

        Dictionary<string,VBObject> glObjs = new Dictionary<string,VBObject>();

        
        public bool Contains(string objName)
        {
            return glObjs.ContainsKey(objName);
        }
        
        public VBObject FindVBObj(string objName)
        {
            return glObjs.ContainsKey(objName) ? glObjs[objName] : null ;
        }

        public void AddVbo(VBObject vbObject)
        {

            glObjs.Add(vbObject.ObjName,vbObject);
        }

        public void DeleteAllVBObjects()
        {
            foreach (var glObj in glObjs)
                VBO.DeleteAllBuffers(glObj.Value);//Если удаляем объект, то чистим массивы во избежании утечки памяти на видеокарте
            glObjs.Clear();
            
        }


        public bool DeleteVBObjects(string objName)
        {
            if (glObjs.ContainsKey(objName))
            {
                VBO.DeleteAllBuffers(glObjs[objName]);//Если удаляем объект, то чистим массивы во избежании утечки памяти на видеокарте
                return glObjs.Remove(objName);
            }
            else return false;

        }
        [Obsolete("Не реккомендуется использовать," +
    "так как может привести к рассинхронизации данных")]
        public void ShowAllVBObjects()
        {
            foreach (var item in glObjs)
                item.Value.ViewState = true;
        }
        [Obsolete("Не реккомендуется использовать," +
    "так как может привести к рассинхронизации данных")]
        public void HideAllVBObjects()
        {
            foreach (var item in glObjs)
                item.Value.ViewState = false;
        }

        /// <summary>
        /// CreateSurfaceVBObjects
        /// </summary>
        /// <param name="ptrs"></param>
        /// <param name="coords"></param>
        /// <param name="colors"></param>
        /// <param name="normals"></param>
        /// <param name="edges"></param>
        /// <param name="objsName"></param>
        /// <param name="separs"></param>
        /// <param name="viewMode"></param>
        public SurfaceObjects CreateSurfaceVBObjects(int[] ptrs, float[] coords, float[] colors, float[] normals,
            bool[] edges, string objsName, int[] separs, ObjView viewMode)
        {
            var vbObj = new SurfaceObjects(objsName,edges, ptrs, coords, colors, normals);
            vbObj.CreateSeparators(separs);
            vbObj.Create3DBoundingBoxes(coords, separs);
            vbObj.ViewMode = viewMode;
            return vbObj;
            //glObjs.Add(vbObj);
            //vbObj.ActiveDrawingObject = AverageColorRenderer.IsEnable ? averageColorRenderer : null;
        }
        /// <summary>
        /// CreateLineVBObjects
        /// </summary>
        /// <param name="ptrs"></param>
        /// <param name="coords"></param>
        /// <param name="colors"></param>
        /// <param name="normals"></param>
        /// <param name="edges"></param>
        /// <param name="objsName"></param>
        public LineObjects CreateLineVBObjects(int[] ptrs, float[] coords, float[] colors, float[] normals, bool[] edges, string objsName)
        {
            return new LineObjects(objsName,edges, ptrs, coords, colors, normals);
            //glObjs.Add(obj);
            //obj.ActiveDrawingObject = AverageColorRenderer.IsEnable ? averageColorRenderer : null;
        }
        /// <summary>
        /// CreatePointVBObjects
        /// </summary>
        /// <param name="ptrs"></param>
        /// <param name="coords"></param>
        /// <param name="colors"></param>
        /// <param name="normals"></param>
        /// <param name="objsName"></param>
        public PointObjects CreatePointVBObjects(int[] ptrs, float[] coords, float[] colors, float[] normals, string objsName)
        {
            return new PointObjects(objsName,ptrs, coords, colors, normals);
            //glObjs.Add(obj);
            //obj.ActiveDrawingObject = AverageColorRenderer.IsEnable ? averageColorRenderer : null;
        }


        /// <inheritdoc/>


        public VBObject CopyVBObjects(VBObject original, string copyName)
        {
            VBObject copyVbo;
            var pointers = original.PointsIndexes;
            var coords = original.PointsCoords;
            var colors = original.PointsColors;
            var normals = original.NormalsCoords;

            if (original.GL_ObjType == GLObjType.point)
                copyVbo = CreatePointVBObjects(pointers, coords, colors, normals, copyName);
            else if (original.GL_ObjType == GLObjType.line)
                copyVbo = CreateLineVBObjects(pointers, coords, colors, normals, new bool[0], copyName);
            else
            {
                var sObj = original as SurfaceObjects;
                var edges = sObj.EdgeFlags;
                normals = normals.Select(v => -v).ToArray();

                copyVbo = CreateSurfaceVBObjects(pointers, coords, colors, normals, edges, copyName, sObj.Separators, ObjView.LinesSurface);
                
            }

            return copyVbo;
        }
        /// <summary>
        /// Смена режима прозрачности для vbo-объектов
        /// </summary>
        /// <param name="AverageColorRenderer"></param>
        public void ChangeVBOTransparentMode(AverageColorRenderer averageColorRenderer)
        {
            foreach (var globj in GetVBObjs())
                globj.ActiveDrawingObject = averageColorRenderer;
        }

        public void ChangeViewModeVBObjects(string objsName, ObjView objView)
        {
            var glObj = glObjs[objsName];
            if (glObj == null)
                MessageEvent?.Invoke(this, new MessageEventArgs("No objects of the specified type found!"));
            else
                glObj.ViewMode = objView;
        }

        public void ChangeSettingsVBObjects(string objsName, float pointsSize, float linesWith)
        {
            var glObj = glObjs[objsName];
            if (glObj == null)
                MessageEvent?.Invoke(this, new MessageEventArgs("No objects of the specified type found!"));
            else
            {
                glObj.Gl_PointSize = pointsSize;
                glObj.Gl_LineWidth = linesWith;
            }
        }

        [Obsolete("Не реккомендуется использовать," +
            "так как может привести к рассинхронизации данных")]
        public void SwitchVBObject(string objsName, bool viewState)
        {
            var glObj = glObjs[objsName];

            if (glObj != null)
                glObj.ViewState = viewState;
        }

        public bool IsVBObjectShown(string objsName)
        {
            var glObj = glObjs[objsName];
            return glObj?.ViewState == true ? true : false;
        }

        public IEnumerable<VBObject> GetVBObjs()
        {
            foreach (var item in glObjs.Values)
            {
                yield return item;
            }
        }
    }
}
