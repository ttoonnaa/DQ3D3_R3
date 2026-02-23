using Unity.VisualScripting;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SrLib
{
    public class SrCanvasComponent : SrComponentBase
    {
        public SrCanvasOrderId canvasOrderId;

        public override void Setup(SrContext context)
        {
            UpdateStatus();
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        
        /// <summary>
        /// UpdateStatus
        /// </summary>
        public void UpdateStatus()
        {
            var canvas = GetComponent<Canvas>();
            if (!canvas)
                return;
            
            if (!SrDef.CanvasOrderValues.TryGetValue(canvasOrderId, out var canvasOrderValue))
                return;
            
            canvas.sortingOrder = canvasOrderValue;
        }
    }
    
#if UNITY_EDITOR

    [CustomEditor(typeof(SrCanvasComponent))]
    public class SrCanvasComponentEditor : Editor
    {
        /// <summary>
        /// Inspector の GUI を更新
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("[ Update ]"))
            {
                var srCanvasComponent = (SrCanvasComponent)target;
                if (!srCanvasComponent)
                    return;

                srCanvasComponent.UpdateStatus();
                
                EditorUtility.SetDirty(target);
            }  
        }
    }
#endif

}
