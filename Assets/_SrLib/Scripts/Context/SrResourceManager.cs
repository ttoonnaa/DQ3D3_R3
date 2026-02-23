using System;
using System.Collections.Generic;
using Object = System.Object;

namespace SrLib
{
    /// <summary>
    /// リソースマネージャー
    /// ・新規リソースの自動解放
    /// ・共有リソースの参照カウンター
    /// を実装します。
    /// 直接は使わず、SrRes の中で使われます。 
    /// </summary>
    public class SrResourceManager : SrDisposableBase
    {
        private class Value
        {
            public Object Resource;
            public string Name;
            public int RefCount;
            public Action<Object> Disposer;
        }
        
        private SrContext _context;
        private readonly Dictionary<string, Value> _resourceHash = new ();
        private bool _disposed;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public SrResourceManager(SrContext context)
        {
            _context = context;
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
                // リソースが残っている場合はログを出す
                if (_resourceHash.Count > 0)
                {
                    foreach (var value in _resourceHash.Values)
                    {
                        SrLogCore.Error($"未解放のリソース : {value.Name}");
                    }
                }
                else
                {
                    SrLogCore.Important("未解放のリソースはありません。");
                }
            }

            _disposed = true;
            
            base.Dispose();
        }
        
        /// <summary>
        /// リソースを登録する
        /// </summary>
        public void Register(string key, Object resource, string name, Action<Object> disposer)
        {
            if (_resourceHash.TryGetValue(key, out var value))
            {
                value.RefCount++;
            }
            else
            {
                _resourceHash.Add(key, new Value { Resource = resource, Name = name, RefCount = 1, Disposer = disposer });
                
                SrLogCore.Log($"{name} が登録されました。");
            }
        }
        
        /// <summary>
        /// リソースを解放する
        /// </summary>
        public void Release(string key)
        {
            // 参照カウントを減らす
            if (_resourceHash.TryGetValue(key, out var value))
            {
                value.RefCount--;
                if (value.RefCount <= 0)
                {
                    value.Disposer?.Invoke(value.Resource);
                    _resourceHash.Remove(key);
                    
                    SrLogCore.Log($"{value.Name} が解放されました。");
                }
            }
        }
    }

    /// <summary>
    /// リソースマネージャーに管理されるリソースのベースクラスです。
    /// このリソースはコンストラクターでマネージャーに登録され、Dispose でマネージャーから解放されます。
    /// マネージャーはデフォルトでは SrContext.ResourceManager が使われますが、指定することもできます。
    /// </summary>
    public class SrResBase<T> : SrDisposableBase where T : class
    {
        private readonly SrContext _context;
        private readonly SrResourceManager _manager;
        private readonly string _key;
        private T _resource;
        private readonly string _name;
        private readonly Action<Object> _disposer;
        private bool _disposed;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public SrResBase(SrContext context, T resource, string name, Action<Object> disposer)
        {
            _context = context;
            _manager = context.ResourceManager;
            _resource = resource;
            _key = $"{_resource.GetHashCode()}";
            _name = name;
            _disposer = disposer;

            // マネージャーに登録
            _manager.Register(_key, _resource, _name, _disposer);
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
                if (_resource != null)
                {
                    // マネージャーから解放
                    _manager.Release(_key);
                    _resource = null;
                }
            }
            
            _disposed = true;
            
            base.Dispose();
        }
        
        /// <summary>
        /// Body
        /// </summary>
        public T Body()
        {
            return _resource;
        }
        
        /// <summary>
        /// 参照カウントを増やす
        /// </summary>
        public SrResBase<T> AddRef()
        {
            return new SrResBase<T>(_context, _resource, _name, _disposer);
        }
    }
    
    /// <summary>
    /// これを使ってください。Dispose 可能なリソースに対応しています。
    /// このリソースはコンストラクターでマネージャーに登録され、Dispose でマネージャーから解放されます。
    /// マネージャーはデフォルトでは SrContext.ResourceManager が使われます。
    /// </summary>
    public class SrRes<T> : SrResBase<T> where T : SrDisposableBase
    {
        /// <summary>
        /// コンストラクター
        /// </summary>
        public SrRes(SrContext context, T resource, string name) : base(context, resource, name, target => ((T)target).Dispose())
        {
        }
    }

    /// <summary>
    /// これを使ってください。UnityEngine.Object.Destroy 可能なリソースに対応しています。
    /// このリソースはコンストラクターでマネージャーに登録され、UnityEngine.Object.Destroy でマネージャーから解放されます。
    /// マネージャーはデフォルトでは SrContext.ResourceManager が使われます。
    /// </summary>
    public class SrUnityRes<T> : SrResBase<T> where T : UnityEngine.Object
    {
        /// <summary>
        /// コンストラクター
        /// </summary>
        public SrUnityRes(SrContext context, T resource, string name) : base(context, resource, name, target => UnityEngine.Object.Destroy((UnityEngine.Object)target))
        {
        }
    }
}













