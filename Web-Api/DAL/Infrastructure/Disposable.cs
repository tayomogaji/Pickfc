namespace Pickfc.DAL.Infrastructure
{
    public class Disposable : IDisposable
    {
        private bool isDisposed;
        ~Disposable()
        {
            Dispose(false);
        }

        //removes unsued resorces
        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) 
        {
            if (!isDisposed && disposing)
                DisposeCore();
            
            isDisposed = true;
        }

        protected virtual void DisposeCore() { }
    }
}
