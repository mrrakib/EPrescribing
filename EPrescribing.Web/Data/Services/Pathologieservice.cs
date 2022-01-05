using EPrescribing.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EPrescribing.Web.Data.Services
{
    public interface IPathologieservice : ICommonService<Pathology>, IDisposable
    {

    }
    public class Pathologieservice : IPathologieservice
    {
        private readonly AppEntities _context;

        public Pathologieservice()
        {
            _context = new AppEntities();
        }

        public async Task<IEnumerable<Pathology>> GetAllAsync()
        {
            return await _context.Pathologies.Where(a => a.IsActive).ToListAsync();
        }
        public async Task<int> GetCountAsync()
        {
            return await _context.Pathologies.Where(a => a.IsActive).CountAsync();
        }
        public async Task<Pathology> FindAsync(int? id)
        {
            return await _context.Pathologies.FindAsync(id ?? 0);
        }
        public async Task<bool> AddAsync(Pathology model)
        {
            if (model != null)
            {
                try
                {
                    _context.Pathologies.Add(model);
                    var rowAffect = await _context.SaveChangesAsync();
                    return rowAffect > 0 ? true : false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
        public async Task<bool> UpdateAsync(Pathology model)
        {
            if (model != null)
            {
                try
                {
                    model.UpdatedDate = System.DateTime.Now;
                    _context.Entry(model).State = EntityState.Modified;
                    var rowAffect = await _context.SaveChangesAsync();
                    return rowAffect > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            try
            {
                Pathology obj = _context.Pathologies.Find(Id);
                obj.IsActive = false;

                var isDelete = await UpdateAsync(obj);
                return isDelete;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> IsExistItemAsync(string name = "")
        {
            var existCount = await _context.Pathologies.CountAsync(a => a.IsActive && a.Name == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public async Task<bool> IsExistItemForUpdateAsync(int id, string name = "")
        {
            var existCount = await _context.Pathologies.CountAsync(a => a.IsActive && a.Id != id && a.Name == name.Trim());
            if (existCount > 0)
                return true;

            return false;
        }
        public async Task<PagedList.IPagedList<Pathology>> GetAllPageListAsync(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = await _context.Pathologies.Where(a => a.IsActive).CountAsync();
                var data = await _context.Pathologies.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<Pathology>(data.OrderBy(a => a.Name), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = await _context.Pathologies.Where(a => a.IsActive && a.Name.Contains(searchString)).CountAsync();
                var data = await _context.Pathologies.Where(a => a.IsActive && a.Name.Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<Pathology>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);
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