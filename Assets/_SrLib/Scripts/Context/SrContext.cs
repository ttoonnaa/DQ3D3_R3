using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using Object = UnityEngine.Object;

namespace SrLib
{
    public class SrContext : SrDisposableBase
    {
        public CancellationTokenSource Canceler;
        public ISrListener Listener;
        public GameObject MainObject;
        public SrResourceManager ResourceManager;
        public SrCameraManager CameraManager;
        public GameObject MessageDialogPrefab;
        public GameObject MessageDialogObject;

        // フォント
        public Font SystemFont;
        public TMP_FontAsset SystemTmpFont;

        // Disposable
        private bool _disposed = false;

        /// <summary>
        /// エンジンを作成
        /// </summary>
        public async UniTask Create(CancellationToken cancellationToken, ISrListener listener)
        {
            Canceler = new CancellationTokenSource();
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, Canceler.Token).Token;
            Listener = listener;

            // メインオブジェクトを作成
            var mainObjectPrefab = Resources.Load<GameObject>("SrRes_MainObject");
            MainObject = SrContextUtility.Instantiate(this, mainObjectPrefab, null);
            Object.DontDestroyOnLoad(MainObject);
            var mainComponent = MainObject.GetComponent<SrMainComponent>();
            mainComponent.Setup(this);
            
            // カメラを作成
            // 背景を黒く塗りつぶすために最優先
            CameraManager = new SrCameraManager(this);
            CameraManager.CreateObject();

            // フォントを作成
            SystemFont = Resources.Load<Font>("NotoSansJP-Regular");
            SystemTmpFont = TMP_FontAsset.CreateFontAsset(SystemFont
                , 60, 12, GlyphRenderMode.SDFAA, 1024, 1024
            );

            // フォントの行間隔変更
            var faceInfo = SystemTmpFont.faceInfo;
            faceInfo.lineHeight = faceInfo.ascentLine - faceInfo.descentLine;
            SystemTmpFont.faceInfo = faceInfo;
            
            // エラーダイアログを読み込み
            MessageDialogPrefab = Resources.Load<GameObject>("SrRes_MessageDialog");

            // リソースマネージャーを作成
            ResourceManager = new SrResourceManager(this);

            
            // 最初のローカライズの初期化を待つ
            //await LocalizationSettings.InitializationOperation.ToUniTask(cancellationToken: cancellationToken);

        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Canceler?.Cancel();
                Canceler?.Dispose();
                Canceler = null;
                
                ResourceManager?.Dispose();
                ResourceManager = null;
            }

            _disposed = true;
            
            base.Dispose(true);
        }

        /// <summary>
        /// PreUpdate
        /// </summary>
        public void PreUpdate()
        {
            
        }
        
        /// <summary>
        /// LateUpdate
        /// </summary>
        public void LastUpdate()
        {
        }

        /// <summary>
        /// メイン処理
        /// </summary>
        public void MainProc()
        {
        }
    }
}
