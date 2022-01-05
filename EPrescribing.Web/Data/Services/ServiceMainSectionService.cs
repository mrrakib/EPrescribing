using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface IServiceMainSectionService : IDisposable
    {
        IEnumerable<ServiceMainSection> GetAll();
        int GetCount();
        ServiceMainSection GetDetails(int Id);
        bool Add(ServiceMainSection model);
        bool Update(ServiceMainSection model);
        bool Delete(int Id);
        bool IsExistItem();
        bool IsExistItemForUpdate(int id);
        ServiceMainSection GetTopOne();
    }
    public class ServiceMainSectionService : IServiceMainSectionService
    {
        private readonly AppEntities _context;

        public ServiceMainSectionService(AppEntities context)
        {
            _context = context;
        }
        public bool IsExistItem()
        {
            var existCount = _context.ServiceMainSections.Count(a => a.IsActive);
            if (existCount > 0)
                return true;
            return false;
        }
        public bool IsExistItemForUpdate(int id)
        {
            var existCount = _context.ServiceMainSections.Count(a => a.IsActive && a.Id != id);
            if (existCount > 0)
                return true;
            return false;
        }
        public IEnumerable<ServiceMainSection> GetAll()
        {
            return _context.ServiceMainSections;
        }

        public ServiceMainSection GetTopOne()
        {
            return _context.ServiceMainSections.FirstOrDefault();
        }
        public int GetCount()
        {
            return _context.ServiceMainSections.Count();
        }
        public ServiceMainSection GetDetails(int Id)
        {
            return _context.ServiceMainSections.Find(Id);
        }
        public bool Add(ServiceMainSection model)
        {
            if (model != null)
            {
                try
                {
                    _context.ServiceMainSections.Add(model);
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

        public bool Update(ServiceMainSection model)
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
                ServiceMainSection obj = _context.ServiceMainSections.Find(Id);
                _context.ServiceMainSections.Remove(obj);
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