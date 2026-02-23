using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SrLib
{
    public class SrContext : SrDisposableBase
    {
        public CancellationTokenSource Canceler;
        public ISrListener Listener;
        public GameObject MainObject;

        private bool _disposed = false;

        /// <summary>
        /// エンジンを作成
        /// </summary>
        public async UniTask Create(CancellationToken cancellationToken, ISrListener listener)
        {
            Canceler = new CancellationTokenSource();
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, Canceler.Token).Token;
            Listener = listener;

            
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
