using _SrLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SrLib
{
    public class SrButtonComponent : Selectable, ISrComponent
    {
        public bool xxx;
        
        public bool pointerCanPress;
        public bool canSelect;
        
        public void Setup(SrContext context)
        {
        }
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!xxx)
            {
                base.OnPointerDown(eventData);
                
                Debug.Log("でふぉるととおす");
            }
            else
            {
                Debug.Log("でふぉるととおさない！！！");
            }
        }
    }
}



