using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface IContactSectionService : IDisposable
    {
        IEnumerable<ContactSection> GetAll();
        IPagedList<ContactSection> GetPageList(int pageNo, int rowNo, string searchString);
        int GetCount();
        ContactSection GetDetails(int Id);
        bool Add(ContactSection model);
        bool Update(ContactSection model);
        bool Delete(int Id);
        bool IsExistItem();
        bool IsExistItemForUpdate(int id);
        ContactSection GetTopOne();
    }
    public class ContactSectionService : IContactSectionService
    {
        private readonly AppEntities _context;

        public ContactSectionService(AppEntities context)
        {
            _context = context;
        }
        public bool IsExistItem()
        {
            var existCount = _context.ContactSections.Count(a => a.IsActive);
            if (existCount > 0)
                return true;
            return false;
        }
        public bool IsExistItemForUpdate(int id)
        {
            var existCount = _context.ContactSections.Count(a => a.IsActive && a.Id != id);
            if (existCount > 0)
                return true;
            return false;
        }
        public IEnumerable<ContactSection> GetAll()
        {
            return _context.ContactSections;
        }

        public ContactSection GetTopOne()
        {
            return _context.ContactSections.FirstOrDefault();
        }
        public IPagedList<ContactSection> GetPageList(int pageNo, int rowNo, string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                int totalRows = _context.ContactSections.Where(a => a.IsActive).Count();
                var data = _context.ContactSections.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<ContactSection>(data.OrderBy(o => o.Id), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = _context.ContactSections.Where(a => a.IsActive && a.Title.ToLower().Equals(searchString)).Count();
                var data = _context.ContactSections.Where(a => a.IsActive && a.Title.ToLower().Equals(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<ContactSection>(data.OrderBy(o => o.Id), pageNo, rowNo, totalRows);
            }
        }
        public int GetCount()
        {
            return _context.ContactSections.Count();
        }
        public ContactSection GetDetails(int Id)
        {
            return _context.ContactSections.Find(Id);
        }
        public bool Add(ContactSection model)
        {
            if (model != null)
            {
                try
                {
                    _context.ContactSections.Add(model);
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

        public bool Update(ContactSection model)
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
                ContactSection obj = _context.ContactSections.Find(Id);
                _context.ContactSections.Remove(obj);
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