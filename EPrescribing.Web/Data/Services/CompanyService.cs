using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface ICompanyService : IDisposable
    {
        IEnumerable<Company> GetAll();
        int GetCount();
        Company GetDetails(int Id);
        bool Add(Company model);
        bool Update(Company model);
        bool Delete(int Id);
        IPagedList<Company> GetCompanyPageList(int pageNo, int rowNo, string searchString);
        bool IsExistItem(string name);
        bool IsExistItemForUpdate(int id, string name);
    }
    public class CompanyService : ICompanyService
    {
        private readonly AppEntities _context;

        public CompanyService(AppEntities context)
        {
            _context = context;
        }

        public IEnumerable<Company> GetAll()
        {
            return _context.Companies;
        }
        public int GetCount()
        {
            return _context.Companies.Count();
        }
        public Company GetDetails(int Id)
        {
            return _context.Companies.Find(Id);
        }
        public bool Add(Company model)
        {
            if (model != null)
            {
                try
                {
                    _context.Companies.Add(model);
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

        public bool Update(Company model)
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
                Company obj = _context.Companies.Find(Id);
                _context.Companies.Remove(obj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public IPagedList<Company> GetCompanyPageList(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = _context.Companies.Where(a => a.IsActive).Count();
                var data = _context.Companies.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<Company>(data.OrderBy(o => o.CompanyName), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = _context.Companies.Where(a => a.IsActive && a.CompanyName.Contains(searchString)).Count();
                var data = _context.Companies.Where(a => a.IsActive && a.CompanyName.Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<Company>(data.OrderBy(o => o.CompanyName), pageNo, rowNo, totalRows);
            }
        }
        public bool IsExistItem(string name)
        {
            var existCount = _context.Companies.Count(a => a.IsActive && a.CompanyName == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public bool IsExistItemForUpdate(int id, string name)
        {
            var existCount = _context.Companies.Count(a => a.IsActive && a.Id != id && a.CompanyName == name.Trim());
            if (existCount > 0)
                return true;
            return false;
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