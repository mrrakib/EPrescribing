using System;

namespace EPrescribing.Web.Data.Services
{
    public interface IReportService : IDisposable
    {

    }
    public class ReportService : IReportService
    {
        private readonly AppEntities _context;

        public ReportService()
        {
            _context = new AppEntities();
        }







        #region Disposed
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}