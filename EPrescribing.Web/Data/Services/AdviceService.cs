using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EPrescribing.Web.Data.Services
{
    public interface IAdviceService : ICommonService<Advice>, IDisposable
    {

    }
    public class AdviceService : IAdviceService
    {
        private readonly AppEntities _context;

        public AdviceService()
        {
            _context = new AppEntities();
        }

        public async Task<IEnumerable<Advice>> GetAllAsync()
        {
            return await _context.Advice.Where(a => a.IsActive).ToListAsync();
        }
        public async Task<int> GetCountAsync()
        {
            return await _context.Advice.Where(a => a.IsActive).CountAsync();
        }
        public async Task<Advice> FindAsync(int? id)
        {
            return await _context.Advice.FindAsync(id ?? 0);
        }
        public async Task<bool> AddAsync(Advice model)
        {
            if (model != null)
            {
                try
                {
                    _context.Advice.Add(model);
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
        public async Task<bool> UpdateAsync(Advice model)
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
                Advice obj = _context.Advice.Find(Id);
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
            var existCount = await _context.Advice.CountAsync(a => a.IsActive && a.AdviceName == name.Trim());
            if (existCount > 0)
                return true;

            return false;
        }
        public async Task<bool> IsExistItemForUpdateAsync(int id, string name = "")
        {
            var existCount = await _context.Advice.CountAsync(a => a.IsActive && a.Id != id && a.AdviceName == name.Trim());
            if (existCount > 0)
                return true;

            return false;
        }
        public async Task<PagedList.IPagedList<Advice>> GetAllPageListAsync(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = await _context.Advice.Where(a => a.IsActive).CountAsync();
                var data = await _context.Advice.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new StaticPagedList<Advice>(data.OrderBy(a => a.AdviceName), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = await _context.Advice.Where(a => a.IsActive && a.AdviceName.Contains(searchString)).CountAsync();
                var data = await _context.Advice.Where(a => a.IsActive && a.AdviceName.Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new StaticPagedList<Advice>(data.OrderBy(o => o.AdviceName), pageNo, rowNo, totalRows);
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