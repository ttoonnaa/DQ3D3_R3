using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SrLib
{
    public class SrContext
    {
        public CancellationTokenSource Canceler;
        public ISrListener Listener;
        public GameObject MainObject;

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
        public void Dispose()
        {
            
        }
    }
}