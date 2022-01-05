using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface ISubscribedService : IDisposable
    {
        SubscriptionFees Add(SubscriptionFees model);
        PagedList<SubscriptionFees> GetAll(string filter, string accountNo, string transactionNo, string isActive, int page, int pageSize);
    }
    public class SubscribedService : ISubscribedService
    {
        private readonly AppEntities _context;

        public SubscribedService()
        {
            _context = new AppEntities();
        }

        public SubscriptionFees Add(SubscriptionFees model)
        {
            var add = _context.SubscriptionFees.Add(model);
            var rowAffect = _context.SaveChanges();
            return rowAffect > 0 ? add : null;
        }
        public PagedList<SubscriptionFees> GetAll(string filter, string accountNo, string transactionNo, string isActive, int page, int pageSize)
        {
            var data = _context.SubscriptionFees.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
                data = data.Where(a => a.PaymentMethod.Contains(filter.Trim()));

            if (!string.IsNullOrWhiteSpace(accountNo))
                data = data.Where(a => a.DebitAccount.Contains(accountNo.Trim()));

            if (!string.IsNullOrWhiteSpace(transactionNo))
                data = data.Where(a => a.TransactionNo.Contains(transactionNo.Trim()));

            if (!string.IsNullOrWhiteSpace(isActive) && isActive == "Active")
                data = data.Where(a => a.Doctor.IsActive);

            if (!string.IsNullOrWhiteSpace(isActive) && isActive == "InActive")
                data = data.Where(a => !a.Doctor.IsActive);

            return (PagedList<SubscriptionFees>)data.OrderByDescending(a => a.Id).ToPagedList(page, pageSize);
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