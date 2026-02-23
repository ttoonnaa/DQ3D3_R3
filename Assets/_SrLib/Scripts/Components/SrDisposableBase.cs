using System;

namespace SrLib
{
    public class SrDisposableBase : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
        }
        
        ~SrDisposableBase()
        {
            Dispose(false);
        }
    }
}