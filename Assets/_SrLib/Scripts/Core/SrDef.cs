using System.Collections.Generic;

namespace SrLib
{
    /// <summary>
    /// SrCanvasOrderId
    /// ★ ID を変えてはいけません！
    /// </summary>
    public enum SrCanvasOrderId
    {
        None = 0,
        LogView = 1,
        MessageDialog = 2,
        DebugMenu = 3,
        LanguageSelectDialog = 4,
    }

    /// <summary>
    /// SrDef
    /// </summary>
    public static class SrDef
    {
        /// <summary>
        /// CanvasOrderValues
        /// こっちは値を変えてもいいよ
        /// </summary>
        public static readonly Dictionary<SrCanvasOrderId, int> CanvasOrderValues = new ()
        {
            { SrCanvasOrderId.LanguageSelectDialog, 30001 },
            { SrCanvasOrderId.DebugMenu, 30002 },
            { SrCanvasOrderId.MessageDialog, 30003 },
            { SrCanvasOrderId.LogView, 30004 },
        };
    }
}
