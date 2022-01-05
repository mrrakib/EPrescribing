using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface IDesignationService : IDisposable
    {
        IEnumerable<Designation> GetAll();
        IPagedList<Designation> GetDesignationPageList(int pageNo, int rowNo, string searchString);
        int GetCount();
        Designation GetDetails(int Id);
        bool Add(Designation model);
        bool Update(Designation model);
        bool Delete(int Id);
        bool IsExistItem(string name);
        bool IsExistItemForUpdate(int id, string name);
    }
    public class DesignationService : IDesignationService
    {
        private readonly AppEntities _context;

        public DesignationService(AppEntities context)
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
        public IEnumerable<Designation> GetAll()
        {
            return _context.Designations;
        }
        public IPagedList<Designation> GetDesignationPageList(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = _context.Designations.Where(a => a.IsActive).Count();
                var data = _context.Designations.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<Designation>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = _context.Designations.Where(a => a.IsActive && a.Name.Contains(searchString)).Count();
                var data = _context.Designations.Where(a => a.IsActive && a.Name.Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<Designation>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);
            }
        }
        public int GetCount()
        {
            return _context.Designations.Count();
        }
        public Designation GetDetails(int Id)
        {
            return _context.Designations.Find(Id);
        }
        public bool Add(Designation model)
        {
            if (model != null)
            {
                try
                {
                    _context.Designations.Add(model);
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

        public bool Update(Designation model)
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
                Designation obj = _context.Designations.Find(Id);
                _context.Designations.Remove(obj);
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