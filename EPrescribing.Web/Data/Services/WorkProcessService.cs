using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface IWorkProcessService : IDisposable
    {
        IEnumerable<WorkProcess> GetAll();
        int GetCount();
        WorkProcess GetDetails(int Id);
        bool Add(WorkProcess model);
        bool Update(WorkProcess model);
        bool Delete(int Id);
        bool IsExistItem();
        bool IsExistItemForUpdate(int id);
        WorkProcess GetTopOne();
    }
    public class WorkProcessService : IWorkProcessService
    {
        private readonly AppEntities _context;

        public WorkProcessService(AppEntities context)
        {
            _context = context;
        }
        public bool IsExistItem()
        {
            var existCount = _context.WorkProcesses.Count(a => a.IsActive);
            if (existCount > 0)
                return true;
            return false;
        }
        public bool IsExistItemForUpdate(int id)
        {
            var existCount = _context.WorkProcesses.Count(a => a.IsActive && a.Id != id);
            if (existCount > 0)
                return true;
            return false;
        }
        public IEnumerable<WorkProcess> GetAll()
        {
            return _context.WorkProcesses;
        }

        public WorkProcess GetTopOne()
        {
            return _context.WorkProcesses.FirstOrDefault();
        }
        public int GetCount()
        {
            return _context.WorkProcesses.Count();
        }
        public WorkProcess GetDetails(int Id)
        {
            return _context.WorkProcesses.Find(Id);
        }
        public bool Add(WorkProcess model)
        {
            if (model != null)
            {
                try
                {
                    _context.WorkProcesses.Add(model);
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

        public bool Update(WorkProcess model)
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
                WorkProcess obj = _context.WorkProcesses.Find(Id);
                _context.WorkProcesses.Remove(obj);
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