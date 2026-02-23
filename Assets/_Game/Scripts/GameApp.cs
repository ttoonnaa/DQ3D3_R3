using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SrLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace _Game
{
    /// <summary>
    /// SrLib からのコールバックを受け取るリスナー
    /// </summary>
    public class GameListener : ISrListener
    {
        public void Restart()
        {
            GameApp.Restart().Forget();
        }

        public void OnApplicationQuit()
        {
            GameApp.OnApplicationQuit();
        }
    }

    /// <summary>
    /// 唯一のアプリケーション
    /// </summary>
    public class GameApp
    {
        public static CancellationTokenSource Canceler;
        public static CancellationToken CancellationToken;
        public static SrContext SrContext;
        public static GameListener Listener;
        //public static GameContext Context;

        private static bool _applicationQuited;
        
        /// <summary>
        /// 最初に実行される
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void EntryPoint()
        {
            _Main().Forget();
        }

        /// <summary>
        /// メイン関数
        /// </summary>
        private static async UniTask _Main()
        {
            Canceler = new CancellationTokenSource();
            CancellationToken = Canceler.Token;

            try
            {
                SrLogCore.Important("GameApp を開始します。");
                
                // エディター実行時の設定
#if UNITY_EDITOR
                QualitySettings.vSyncCount = 1;
#endif

                // シーンの全てのオブジェクトを削除
                foreach (var gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
                    Object.Destroy(gameObject);

                // リスナーを作成
                Listener = new GameListener();

                // SrLib を作成
                SrContext = new SrContext();
                await SrContext.Create(CancellationToken, Listener);

                // アプリケーション終了
                SrUnityUtility.QuitApplication();
            }
            catch (OperationCanceledException)
            {
                SrLogCore.Important("GameApp がキャンセルされました。");
            }
            catch (Exception e)
            {
                await _ShowExceptionError(e);
            }
        }
        
        /// <summary>
        /// リスタート
        /// </summary>
        public static async UniTask Restart()
        {
            try
            {
                SrLogCore.Important("GameApp をリスタートします。");

                // ゲームを解放
                //Context?.Dispose();
                //Context = null;
                
                // ゲームを作成
                //Context = new GameContext(SrContext);
                //await Context.Start();
            }
            catch (OperationCanceledException)
            {
                SrLogCore.Important("GameApp がキャンセルされました。");
            }
            catch (Exception e)
            {
                await _ShowExceptionError(e);
            }
        }

        /// <summary>
        /// アプリが終了された
        /// </summary>
        public static void OnApplicationQuit()
        {
            Canceler?.Cancel();
            Canceler?.Dispose();
            Canceler = null;

            // ゲームを解放
            //Context?.Dispose();
            //Context = null;

            // SrLib を解放
            SrContext?.Dispose();
            SrContext = null;
            
            SrLogCore.Important("GameApp は正常に終了しました。");
            
            SrUnityUtility.KillApplicationProcess();
        }
        
        /// <summary>
        /// 例外のエラーを表示する
        /// </summary>
        private static async UniTask _ShowExceptionError(Exception e)
        {
            // エンジンが存在する場合はエラーを表示
            //if (Context != null)
            //{
            //    await GameLogFlow.ShowError(Context, CancellationToken, e);
            //}
            if (SrContext != null)
            {
                //await SrLogFlow.ShowError(SrContext, CancellationToken, e);
            }
            else
            {
                SrLogCore.Error(e.ToString());
            }
        }
    }
}