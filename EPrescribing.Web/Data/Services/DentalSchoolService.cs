using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface IDentalSchoolService : IDisposable
    {
        IEnumerable<DentalSchool> GetAll();
        int GetCount();
        DentalSchool GetDetails(int Id);
        bool Add(DentalSchool model);
        bool Update(DentalSchool model);
        bool Delete(int Id);
        IPagedList<DentalSchool> GetAllPageList(int pageNo, int rowNo, string searchString);
        bool IsExistItem(string name);
        bool IsExistItemForUpdate(int id, string name);
        bool GetReferenceDoctor(int? id);
    }
    public class DentalSchoolService : IDentalSchoolService
    {
        private readonly AppEntities _context;

        public DentalSchoolService(AppEntities context)
        {
            _context = context;
        }
        public bool IsExistItem(string name)
        {
            var existCount = _context.DentalSchools.Count(a => a.IsActive && a.Name == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public bool IsExistItemForUpdate(int id, string name)
        {
            var existCount = _context.DentalSchools.Count(a => a.IsActive && a.Id != id && a.Name == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public IEnumerable<DentalSchool> GetAll()
        {
            return _context.DentalSchools;
        }
        public int GetCount()
        {
            return _context.DentalSchools.Count();
        }
        public DentalSchool GetDetails(int Id)
        {
            return _context.DentalSchools.Find(Id);
        }
        public bool Add(DentalSchool model)
        {
            if (model != null)
            {
                try
                {
                    _context.DentalSchools.Add(model);
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

        public bool Update(DentalSchool model)
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
                DentalSchool obj = _context.DentalSchools.Find(Id);
                _context.DentalSchools.Remove(obj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public IPagedList<DentalSchool> GetAllPageList(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = _context.DentalSchools.Where(a => a.IsActive).Count();
                var data = _context.DentalSchools.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new PagedList.StaticPagedList<DentalSchool>(data.OrderBy(a => a.Name), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = _context.DentalSchools.Where(a => a.IsActive && a.Name.Contains(searchString)).Count();
                var data = _context.DentalSchools.Where(a => a.IsActive && a.Name.Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new PagedList.StaticPagedList<DentalSchool>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);
            }
        }

        public bool GetReferenceDoctor(int? id)
        {
            var doctorAvailable = _context.Doctors.FirstOrDefault(a => a.DentalSchoolId == id);
            if (doctorAvailable != null)
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
