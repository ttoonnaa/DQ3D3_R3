using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SrLib
{
    /// <summary>
    /// Addressables
    /// </summary>
    public static class SrAdrCore
    {
        private static bool _isInitialized = false;

        /// <summary>
        /// Addressables を初期化する
        /// </summary>
        public static async UniTask Initialize(CancellationToken cancellationToken)
        {
            // Addressables.InitializeAsync はアプリを通して１度しか呼んではいけない
            if (!_isInitialized)
            {
                _isInitialized = true;
                await Addressables.InitializeAsync().ToUniTask(cancellationToken: cancellationToken);
            }
        }
        
        /// <summary>
        /// LoadAsset
        /// </summary>
        public static AsyncOperationHandle<T> LoadAsset<T>(string key)
        {
            return Addressables.LoadAssetAsync<T>(key);
        }

        /// <summary>
        /// Release
        /// </summary>
        public static void ReleaseAsset(AsyncOperationHandle handle)
        {
            if (handle.IsValid())
                Addressables.Release(handle);
        }
        
        /// <summary>
        /// 読み込み待機用のコルーチン
        /// AsyncHandle を直接 UniTask で待機するとうまく行かないバグ対策
        /// https://github.com/Cysharp/UniTask/issues/649
        /// </summary>
        private static IEnumerator _CoroutineForWaitTask(AsyncOperationHandle handle)
        {
            while (!handle.IsDone)
                yield return null;
        }
        
        /// <summary>
        /// 上記バグを考慮した WaitLoadAsset
        /// </summary>
        public static async UniTask<T> WaitHandleAsync<T>(CancellationToken cancellationToken, AsyncOperationHandle<T> handle) where T : class
        {
            await _CoroutineForWaitTask(handle).WithCancellation(cancellationToken);

            if (!handle.IsValid())
                return null;
            
            if (handle.OperationException != null)
            {
                throw handle.OperationException;
            }
            if (handle.IsValid() && handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            return null;
        }
        
        /// <summary>
        /// 参照カウントを増やす
        /// </summary>
        public static AsyncOperationHandle<T> AddRef<T>(AsyncOperationHandle<T> handle) where T : class
        {
            if (!handle.IsValid())
                return handle;

            Addressables.ResourceManager.Acquire(handle);

            return handle;
        }
    }
}


