using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface IAboutSectionService : IDisposable
    {
        IEnumerable<AboutSection> GetAll();
        IPagedList<AboutSection> GetPageList(int pageNo, int rowNo, string searchString);
        int GetCount();
        AboutSection GetDetails(int Id);
        bool Add(AboutSection model);
        bool Update(AboutSection model);
        bool Delete(int Id);
        bool IsExistItem();
        bool IsExistItemForUpdate(int id);
        AboutSection GetTopOne();
    }
    public class AboutSectionService : IAboutSectionService
    {
        private readonly AppEntities _context;

        public AboutSectionService(AppEntities context)
        {
            _context = context;
        }
        public bool IsExistItem()
        {
            var existCount = _context.AboutSections.Count(a => a.IsActive);
            if (existCount > 0)
                return true;
            return false;
        }
        public bool IsExistItemForUpdate(int id)
        {
            var existCount = _context.AboutSections.Count(a => a.IsActive && a.Id != id);
            if (existCount > 0)
                return true;
            return false;
        }
        public IEnumerable<AboutSection> GetAll()
        {
            return _context.AboutSections;
        }

        public AboutSection GetTopOne()
        {
            return _context.AboutSections.FirstOrDefault();
        }
        public IPagedList<AboutSection> GetPageList(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = _context.AboutSections.Where(a => a.IsActive).Count();
                var data = _context.AboutSections.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<AboutSection>(data.OrderBy(o => o.Id), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = _context.AboutSections.Where(a => a.IsActive).Count();
                var data = _context.AboutSections.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<AboutSection>(data.OrderBy(o => o.Id), pageNo, rowNo, totalRows);
            }
        }
        public int GetCount()
        {
            return _context.AboutSections.Count();
        }
        public AboutSection GetDetails(int Id)
        {
            return _context.AboutSections.Find(Id);
        }
        public bool Add(AboutSection model)
        {
            if (model != null)
            {
                try
                {
                    _context.AboutSections.Add(model);
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

        public bool Update(AboutSection model)
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
                AboutSection obj = _context.AboutSections.Find(Id);
                _context.AboutSections.Remove(obj);
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