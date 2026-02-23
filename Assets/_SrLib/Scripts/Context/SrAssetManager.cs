using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = System.Object;

namespace SrLib
{
    /// <summary>
    /// Addressables リソースマネージャー
    /// 参照カウンターの管理は ResourceManager を使います。
    /// このクラスはファクトリーなどを担当します。 
    /// </summary>
    public class SrAssetManager : SrDisposableBase
    {
        private SrContext _context;
        private readonly List<string> _addressableKeyList = new ();

        public SrAssetManager(SrContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Addressables のキーリストを作成する
        /// </summary>
        public void CreateAddressablesKeyList()
        {
            _addressableKeyList.Clear();
            
            // Addressables のコンテンツカタログから全アセットキーを調べる
            foreach (var rc in Addressables.ResourceLocators)
            {
                foreach (var obj in rc.Keys)
                {
                    if (obj is string key)
                        _addressableKeyList.Add(key);
                }
            }
        }
        
        /// <summary>
        /// Addressables アセットを読み込む
        /// </summary>
        public AsyncOperationHandle<T> LoadAsset<T>(string key) where T : class
        {
            if (!_addressableKeyList.Contains(key))
            {
                throw new SrException($"ファイル {key} が見つかりません。");
            }
            
            var handle = SrAdrCore.LoadAsset<T>(key);

            // 存在するのに開けない場合、型が違う可能性が高い
            if (handle is { IsDone: true, OperationException: not null })
            {
                if (typeof(T) == typeof(Sprite))
                {
                    throw new SrException($"ファイル {key} が開けません。{typeof(T).Name} ではない可能性があります。\nよくある間違い：画像を Sprite にしていない");
                }
                else
                {
                    throw new SrException($"ファイル {key} が開けません。{typeof(T).Name} ではない可能性があります。");
                }
            }
            
            return handle;
        }
    }
    
    /// <summary>
    /// Addressables リソースは AsyncOperationHandle で管理しますが、
    /// これが struct なので SrResBase が使えません。
    /// SrResBase を継承せずに作成します。
    /// </summary>
    public class SrAdrRes<T> : SrDisposableBase where T : class
    {
        private readonly SrContext _context;
        private readonly SrResourceManager _manager;
        private readonly string _key;
        private AsyncOperationHandle<T> _handle;
        private readonly string _name;
        private bool _disposed;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public SrAdrRes(SrContext context, AsyncOperationHandle<T> handle, string name)
        {
            _context = context;
            _manager = context.ResourceManager;
            _handle = handle;
            _key = $"{_handle.GetHashCode()}";
            _name = name;

            // マネージャーに登録
            _manager.Register(_key, _handle, _name, target => SrAdrCore.ReleaseAsset((AsyncOperationHandle<T>)target));
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
                if (_handle.IsValid())
                {
                    // マネージャーから解放
                    _manager.Release(_key);
                    _handle = default;
                }
            }
            
            _disposed = true;
            
            base.Dispose();
        }
        
        /// <summary>
        /// Addressables アセットを読み込む
        /// </summary>
        public static async UniTask<SrAdrRes<T>> LoadAssetAsync(SrContext context, CancellationToken cancellationToken, string key)
        {
            var handle = context.AssetManager.LoadAsset<T>(key);

            try
            {
                await SrAdrCore.WaitHandleAsync(cancellationToken, handle);
                return new SrAdrRes<T>(context, handle, key);
            }
            catch (Exception)
            {
                SrAdrCore.ReleaseAsset(handle);
                throw;
            }
        }
        
        /// <summary>
        /// Body
        /// </summary>
        public T Body()
        {
            return _handle.Result;
        }
        
        /// <summary>
        /// 参照カウントを増やす
        /// </summary>
        public SrAdrRes<T> AddRef()
        {
            return new SrAdrRes<T>(_context, _handle, _name);
        }
    }
}