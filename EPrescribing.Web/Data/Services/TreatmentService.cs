using EPrescribing.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EPrescribing.Web.Data.Services
{
    public interface ITreatmentService : ICommonService<Treatment>, IDisposable
    {

    }
    public class TreatmentService : ITreatmentService
    {
        private readonly AppEntities _context;

        public TreatmentService()
        {
            _context = new AppEntities();
        }
        public async Task<IEnumerable<Treatment>> GetAllAsync()
        {
            return await _context.Treatments.Where(a => a.IsActive).ToListAsync();
        }
        public async Task<int> GetCountAsync()
        {
            return await _context.Treatments.Where(a => a.IsActive).CountAsync();
        }
        public async Task<Treatment> FindAsync(int? id)
        {
            return await _context.Treatments.FindAsync(id ?? 0);
        }
        public async Task<bool> AddAsync(Treatment model)
        {
            if (model != null)
            {
                try
                {
                    _context.Treatments.Add(model);
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
        public async Task<bool> UpdateAsync(Treatment model)
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
                Treatment obj = _context.Treatments.Find(Id);
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
            var existCount = await _context.Treatments.CountAsync(a => a.IsActive && a.Name == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public async Task<bool> IsExistItemForUpdateAsync(int id, string name = "")
        {
            var existCount = await _context.Treatments.CountAsync(a => a.IsActive && a.Id != id && a.Name == name.Trim());
            if (existCount > 0)
                return true;

            return false;
        }
        public async Task<PagedList.IPagedList<Treatment>> GetAllPageListAsync(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = await _context.Treatments.Where(a => a.IsActive).CountAsync();
                var data = await _context.Treatments.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<Treatment>(data.OrderBy(a => a.Name), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = await _context.Treatments.Where(a => a.IsActive && a.Name.Contains(searchString)).CountAsync();
                var data = await _context.Treatments.Where(a => a.IsActive && a.Name.Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<Treatment>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);
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