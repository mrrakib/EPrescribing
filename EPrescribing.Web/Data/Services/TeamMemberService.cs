using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface ITeamMemberService : IDisposable
    {
        IEnumerable<TeamMember> GetAll();
        IPagedList<TeamMember> GetPageList(int pageNo, int rowNo, string searchString);
        int GetCount();
        TeamMember GetDetails(int Id);
        bool Add(TeamMember model);
        bool Update(TeamMember model);
        bool Delete(int Id);
    }
    public class TeamMemberService : ITeamMemberService
    {
        private readonly AppEntities _context;

        public TeamMemberService(AppEntities context)
        {
            _context = context;
        }
        public IEnumerable<TeamMember> GetAll()
        {
            return _context.TeamMembers.Where(a => a.IsActive);
        }
        public IPagedList<TeamMember> GetPageList(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = _context.TeamMembers.Where(a => a.IsActive).Count();
                var data = _context.TeamMembers.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<TeamMember>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = _context.TeamMembers.Where(a => a.IsActive && a.Name.ToLower().Trim().Contains(searchString)).Count();
                var data = _context.TeamMembers.Where(a => a.IsActive && a.Name.ToLower().Trim().Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<TeamMember>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);
            }
        }
        public int GetCount()
        {
            return _context.TeamMembers.Count();
        }
        public TeamMember GetDetails(int Id)
        {
            return _context.TeamMembers.Find(Id);
        }
        public bool Add(TeamMember model)
        {
            if (model != null)
            {
                try
                {
                    _context.TeamMembers.Add(model);
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

        public bool Update(TeamMember model)
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
                TeamMember obj = _context.TeamMembers.Find(Id);
                _context.TeamMembers.Remove(obj);
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