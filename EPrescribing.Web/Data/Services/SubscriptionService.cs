using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface ISubscriptionService : IDisposable
    {
        IEnumerable<Subscription> GetAll();
        int GetCount();
        Subscription GetDetails(int Id);
        bool Add(Subscription model);
        bool Update(Subscription model);
        bool Delete(int Id);
        IPagedList<Subscription> GetAllPageList(int pageNo, int rowNo, string searchString);
        bool IsExistItem(string name);
        bool IsExistItemForUpdate(int id, string name);
    }
    public class SubscriptionService : ISubscriptionService
    {
        private readonly AppEntities _context;

        public SubscriptionService(AppEntities context)
        {
            _context = context;
        }

        public bool IsExistItem(string name)
        {
            var existCount = _context.Departments.Count(a => a.IsActive && a.Name == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public bool IsExistItemForUpdate(int id, string name)
        {
            var existCount = _context.Departments.Count(a => a.IsActive && a.Id != id && a.Name == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public IEnumerable<Subscription> GetAll()
        {
            return _context.Subscriptions;
        }
        public int GetCount()
        {
            return _context.Subscriptions.Count();
        }
        public Subscription GetDetails(int Id)
        {
            return _context.Subscriptions.Find(Id);
        }
        public bool Add(Subscription model)
        {
            if (model != null)
            {
                try
                {
                    _context.Subscriptions.Add(model);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }

            }
            return false;
        }

        public bool Update(Subscription model)
        {


            if (model != null)
            {
                try
                {
                    _context.Entry(model).State = EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {

                    return false;
                }

            }
            return false;
        }

        public bool Delete(int Id)
        {
            try
            {
                Subscription obj = _context.Subscriptions.Find(Id);
                _context.Subscriptions.Remove(obj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
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

        public IPagedList<Subscription> GetAllPageList(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = _context.Subscriptions.Where(a => a.IsActive).Count();
                var data = _context.Subscriptions.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new PagedList.StaticPagedList<Subscription>(data.OrderBy(a => a.Name), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = _context.Subscriptions.Where(a => a.IsActive && a.Name.Contains(searchString)).Count();
                var data = _context.Subscriptions.Where(a => a.IsActive && a.Name.Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new PagedList.StaticPagedList<Subscription>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);
            }
        }
        #endregion
    }
}