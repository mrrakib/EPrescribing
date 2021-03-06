using EPrescribing.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EPrescribing.Web.Data.Services
{
    public interface IBrandService : ICommonService<Brand>, IDisposable
    {

    }

    public class BrandService : IBrandService
    {
        private readonly AppEntities _context;

        public BrandService()
        {
            _context = new AppEntities();
        }
        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _context.Brands.Where(a => a.IsActive).ToListAsync();
        }
        public async Task<PagedList.IPagedList<Brand>> GetAllPageListAsync(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = await _context.Brands.Where(a => a.IsActive).CountAsync();
                var data = await _context.Brands.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<Brand>(data.OrderBy(a => a.BrandName), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = await _context.Brands.Where(a => a.IsActive && a.BrandName.Contains(searchString)).CountAsync();
                var data = await _context.Brands.Where(a => a.IsActive && a.BrandName.Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<Brand>(data.OrderBy(o => o.BrandName), pageNo, rowNo, totalRows);
            }
        }
        public async Task<int> GetCountAsync()
        {
            return await _context.Brands.Where(a => a.IsActive).CountAsync();
        }
        public async Task<Brand> FindAsync(int? id)
        {
            return await _context.Brands.FindAsync(id ?? 0);
        }
        public async Task<bool> AddAsync(Brand model)
        {
            if (model != null)
            {
                try
                {
                    _context.Brands.Add(model);
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
        public async Task<bool> UpdateAsync(Brand model)
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
                Brand obj = _context.Brands.Find(Id);
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
            var existCount = await _context.Brands.CountAsync(a => a.IsActive && a.BrandName == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public async Task<bool> IsExistItemForUpdateAsync(int id, string name = "")
        {
            var existCount = await _context.Brands.CountAsync(a => a.IsActive && a.Id != id && a.BrandName == name.Trim());
            if (existCount > 0)
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