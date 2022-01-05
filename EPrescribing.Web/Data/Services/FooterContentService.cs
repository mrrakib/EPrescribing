using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface IFooterContentService : IDisposable
    {
        IEnumerable<FooterContent> GetAll();
        int GetCount();
        FooterContent GetDetails(int Id);
        bool Add(FooterContent model);
        bool Update(FooterContent model);
        bool Delete(int Id);
        bool IsExistItem();
        bool IsExistItemForUpdate(int id);
        FooterContent GetTopOne();
    }
    public class FooterContentService : IFooterContentService
    {
        private readonly AppEntities _context;

        public FooterContentService(AppEntities context)
        {
            _context = context;
        }
        public bool IsExistItem()
        {
            var existCount = _context.FooterContents.Count(a => a.IsActive);
            if (existCount > 0)
                return true;
            return false;
        }
        public bool IsExistItemForUpdate(int id)
        {
            var existCount = _context.FooterContents.Count(a => a.IsActive && a.Id != id);
            if (existCount > 0)
                return true;
            return false;
        }
        public IEnumerable<FooterContent> GetAll()
        {
            return _context.FooterContents;
        }

        public FooterContent GetTopOne()
        {
            return _context.FooterContents.FirstOrDefault();
        }
        public int GetCount()
        {
            return _context.FooterContents.Count();
        }
        public FooterContent GetDetails(int Id)
        {
            return _context.FooterContents.Find(Id);
        }
        public bool Add(FooterContent model)
        {
            if (model != null)
            {
                try
                {
                    _context.FooterContents.Add(model);
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

        public bool Update(FooterContent model)
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
                FooterContent obj = _context.FooterContents.Find(Id);
                _context.FooterContents.Remove(obj);
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