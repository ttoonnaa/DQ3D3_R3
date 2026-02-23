using UnityEngine;
using Object = UnityEngine.Object;

namespace SrLib
{
    public static class SrUnityUtility
    {
        /// <summary>
        /// アプリケーションを終了する
        /// OnApplicationQuit が呼ばれます。
        /// </summary>
        public static void QuitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// プロセスを強制終了する
        /// Unity 6 (Input System 10) で、Windows ビルドでプロセスが残りっぱなしになるバグがあります。
        /// プロセスを強制的に終了するためのメソッドです。
        /// OnApplicationQuit の最後に呼んでください。
        /// </summary>
        public static void KillApplicationProcess()
        {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
            SrLogCore.Important("プロセスを強制終了します。");

            [DllImport("kernel32.dll",SetLastError=true)]
            static extern int TerminateProcess(IntPtr hProcess, uint exitCode);

            var id = System.Diagnostics.Process.GetCurrentProcess().Handle;
            TerminateProcess(id, 0);
#endif         
        }
        
        /// <summary>
        /// int から Color を作成
        /// </summary>
        public static Color ToColor(int r, int g, int b, int a = 255)
        {
            return new Color32((byte)r, (byte)g, (byte)b, (byte)a);
        }
        
        /// <summary>
        /// 白のスプライトを作成
        /// </summary>
        public static Sprite CreateWhiteSprite(Vector2? pivot = null)
        {
            pivot ??= new Vector2(0, 1);
            var texture = Texture2D.whiteTexture;
            var rect = new Rect(0, 0, texture.width, texture.height);
            var border = Vector4.zero;
            return Sprite.Create(texture, rect, pivot.Value, 100f, 1, SpriteMeshType.FullRect, border);
        }       
        
        /// <summary>
        /// CreateFontAsset で作ったアセットを解放する
        /// </summary>
        public static void DestroyFontAsset(TMPro.TMP_FontAsset fontAsset)
        {
            if (!fontAsset)
                return;

            if (fontAsset.material)
                Object.Destroy(fontAsset.material);

            if (fontAsset.atlasTextures != null)
                foreach (var texture in fontAsset.atlasTextures)
                    if (texture)
                        Object.Destroy(texture);

            Object.Destroy(fontAsset);
        }
    }
}



