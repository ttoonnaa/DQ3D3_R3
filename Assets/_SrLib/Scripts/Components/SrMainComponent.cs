
using UnityEngine.UI;

namespace SrLib
{
    public class SrMainComponent : SrComponentBase
    {
        public Selectable dummyButton;

        private SrContext _context;
#if UNITY_EDITOR
        //private SrSelectableHistoryTracker _historyTracker;
#endif

        public override void Setup(SrContext context)
        {
            _context = context;

#if UNITY_EDITOR
            // 選択履歴トラッカーを追加
            //_historyTracker = gameObject.AddComponent<SrSelectableHistoryTracker>();
            //_historyTracker.Setup(context);
#endif
        }
        
        public void OnApplicationQuit()
        {
            _context?.Listener?.OnApplicationQuit();
        }
    }
}
