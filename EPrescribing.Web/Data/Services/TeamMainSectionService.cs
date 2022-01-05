using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface ITeamMainSectionService : IDisposable
    {
        IEnumerable<TeamMainSection> GetAll();
        int GetCount();
        TeamMainSection GetDetails(int Id);
        bool Add(TeamMainSection model);
        bool Update(TeamMainSection model);
        bool Delete(int Id);
        bool IsExistItem();
        bool IsExistItemForUpdate(int id);
        TeamMainSection GetTopOne();
    }
    public class TeamMainSectionService : ITeamMainSectionService
    {
        private readonly AppEntities _context;

        public TeamMainSectionService(AppEntities context)
        {
            _context = context;
        }
        public bool IsExistItem()
        {
            var existCount = _context.TeamMainSections.Count(a => a.IsActive);
            if (existCount > 0)
                return true;
            return false;
        }
        public bool IsExistItemForUpdate(int id)
        {
            var existCount = _context.TeamMainSections.Count(a => a.IsActive && a.Id != id);
            if (existCount > 0)
                return true;
            return false;
        }
        public IEnumerable<TeamMainSection> GetAll()
        {
            return _context.TeamMainSections;
        }

        public TeamMainSection GetTopOne()
        {
            return _context.TeamMainSections.FirstOrDefault();
        }
        public int GetCount()
        {
            return _context.TeamMainSections.Count();
        }
        public TeamMainSection GetDetails(int Id)
        {
            return _context.TeamMainSections.Find(Id);
        }
        public bool Add(TeamMainSection model)
        {
            if (model != null)
            {
                try
                {
                    _context.TeamMainSections.Add(model);
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

        public bool Update(TeamMainSection model)
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
                TeamMainSection obj = _context.TeamMainSections.Find(Id);
                _context.TeamMainSections.Remove(obj);
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