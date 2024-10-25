

namespace SanGuoSha.BaseClass
{
    public partial class AskAnswer
    {
        private bool disposed = false;

        ~AskAnswer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                }
                // 释放非托管资源
                SemaphoreSlim.Dispose();
                disposed = true;
            }
        }
    }
}
