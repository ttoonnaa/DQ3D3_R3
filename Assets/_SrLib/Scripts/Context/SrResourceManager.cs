using System.Collections.Generic;

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
            public SrDisposableBase Resource;
            public string Name;
            public int RefCount;
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
        public void Register(string key, SrDisposableBase resource, string name)
        {
            if (_resourceHash.TryGetValue(key, out var value))
            {
                value.RefCount++;
            }
            else
            {
                _resourceHash.Add(key, new Value { Resource = resource, Name = name, RefCount = 1 });
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
                    value.Resource.Dispose();
                    _resourceHash.Remove(key);
                }
            }
        }
    }

    /// <summary>
    /// これを使ってください。
    /// このリソースはコンストラクターでマネージャーに登録され、Dispose でマネージャーから解放されます。
    /// マネージャーはデフォルトでは SrContext.ResourceManager が使われますが、指定することもできます。
    /// </summary>
    public class SrSharedRes<T> : SrDisposableBase where T : SrDisposableBase
    {
        private readonly SrContext _context;
        private readonly SrResourceManager _manager;
        private readonly string _key;
        private T _resource;
        private readonly string _name;
        private bool _disposed;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public SrSharedRes(SrContext context, T resource, string name)
        {
            _context = context;
            _manager = context.ResourceManager;
            _resource = resource;
            _key = $"{_resource.GetHashCode()}";
            _name = name;

            // マネージャーに登録
            _manager.Register(_key, _resource, _name);
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
        public SrSharedRes<T> AddRef()
        {
            return new SrSharedRes<T>(_context, _resource, _name);
        }
    }
}












