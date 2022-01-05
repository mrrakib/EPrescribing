using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface ISingleServiceSectionService : IDisposable
    {
        IEnumerable<SingleServiceSection> GetAll();
        IPagedList<SingleServiceSection> GetPageList(int pageNo, int rowNo, string searchString);
        int GetCount();
        SingleServiceSection GetDetails(int Id);
        bool Add(SingleServiceSection model);
        bool Update(SingleServiceSection model);
        bool Delete(int Id);
        bool IsExistItem(string title);
        bool IsExistItemForUpdate(int id, string title);
    }
    public class SingleServiceSectionService : ISingleServiceSectionService
    {
        private readonly AppEntities _context;

        public SingleServiceSectionService(AppEntities context)
        {
            _context = context;
        }
        public bool IsExistItem(string title)
        {
            var existCount = _context.SingleServiceSections.Count(a => a.IsActive && a.Title.ToLower().Trim().Equals(title));
            if (existCount > 0)
                return true;
            return false;
        }
        public bool IsExistItemForUpdate(int id, string title)
        {
            var existCount = _context.SingleServiceSections.Count(a => a.IsActive && a.Title.ToLower().Trim().Equals(title) && a.Id != id);
            if (existCount > 0)
                return true;
            return false;
        }
        public IEnumerable<SingleServiceSection> GetAll()
        {
            return _context.SingleServiceSections.Where(a => a.IsActive);
        }
        public IPagedList<SingleServiceSection> GetPageList(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = _context.SingleServiceSections.Where(a => a.IsActive).Count();
                var data = _context.SingleServiceSections.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<SingleServiceSection>(data.OrderBy(o => o.Title), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = _context.SingleServiceSections.Where(a => a.IsActive && a.Title.ToLower().Trim().Contains(searchString)).Count();
                var data = _context.SingleServiceSections.Where(a => a.IsActive && a.Title.ToLower().Trim().Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<SingleServiceSection>(data.OrderBy(o => o.Title), pageNo, rowNo, totalRows);
            }
        }
        public int GetCount()
        {
            return _context.SingleServiceSections.Count();
        }
        public SingleServiceSection GetDetails(int Id)
        {
            return _context.SingleServiceSections.Find(Id);
        }
        public bool Add(SingleServiceSection model)
        {
            if (model != null)
            {
                try
                {
                    _context.SingleServiceSections.Add(model);
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

        public bool Update(SingleServiceSection model)
        {


            if (model != null)
            {
                try
                {
                    model.UpdatedDate = DateTime.Now;
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
                SingleServiceSection obj = _context.SingleServiceSections.Find(Id);
                _context.SingleServiceSections.Remove(obj);
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
        #endregion
    }
}