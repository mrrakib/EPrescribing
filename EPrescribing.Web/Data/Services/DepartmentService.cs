using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface IDepartmentService : IDisposable
    {
        IEnumerable<Department> GetAll();
        int GetCount();
        Department GetDetails(int Id);
        bool Add(Department model);
        bool Update(Department model);
        bool Delete(int Id);
        IPagedList<Department> GetAllPageList(int pageNo, int rowNo, string searchString);
        bool IsExistItem(string name);
        bool IsExistItemForUpdate(int id, string name);
    }
    public class DepartmentService : IDepartmentService
    {
        private readonly AppEntities _context;

        public DepartmentService(AppEntities context)
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
        public IEnumerable<Department> GetAll()
        {
            return _context.Departments;
        }
        public int GetCount()
        {
            return _context.Departments.Count();
        }
        public Department GetDetails(int Id)
        {
            return _context.Departments.Find(Id);
        }
        public bool Add(Department model)
        {
            if (model != null)
            {
                try
                {
                    _context.Departments.Add(model);
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

        public bool Update(Department model)
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
                Department obj = _context.Departments.Find(Id);
                _context.Departments.Remove(obj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public IPagedList<Department> GetAllPageList(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = _context.Departments.Where(a => a.IsActive).Count();
                var data = _context.Departments.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new PagedList.StaticPagedList<Department>(data.OrderBy(a => a.Name), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = _context.Departments.Where(a => a.IsActive && a.Name.Contains(searchString)).Count();
                var data = _context.Departments.Where(a => a.IsActive && a.Name.Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new PagedList.StaticPagedList<Department>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);
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