using EPrescribing.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EPrescribing.Web.Data.Services
{
    public interface ITreatmentTempleteService : ICommonService<TreatmentTemplete>, IDisposable
    {
        Task<IEnumerable<TreatmentTemplete>> GetAllByDoctorIdAsync(int doctorId);
        Task<bool> IsExistingTemplate(int doctorId, string name);
        Task<bool> IsExistingTemplateForUpdateAsync(int id, int doctorId, string name = "");

        Task<PagedList.IPagedList<TreatmentTemplete>> GetAllPageListByDoctorIdAsync(int pageNo, int rowNo, string searchString, int doctorId);
    }
    public class TreatmentTempleteService : ITreatmentTempleteService
    {
        private readonly AppEntities _context;

        public TreatmentTempleteService()
        {
            _context = new AppEntities();
        }

        public async Task<IEnumerable<TreatmentTemplete>> GetAllAsync()
        {
            return await _context.TreatmentTempletes.Where(a => a.IsActive).ToListAsync();
        }
        public async Task<IEnumerable<TreatmentTemplete>> GetAllByDoctorIdAsync(int doctorId)
        {
            return await _context.TreatmentTempletes.Where(a => a.IsActive && a.DoctorId == doctorId).ToListAsync();
        }
        public async Task<int> GetCountAsync()
        {
            return await _context.TreatmentTempletes.Where(a => a.IsActive).CountAsync();
        }
        public async Task<TreatmentTemplete> FindAsync(int? id)
        {
            return await _context.TreatmentTempletes.FindAsync(id ?? 0);
        }
        public async Task<bool> AddAsync(TreatmentTemplete model)
        {
            if (model != null)
            {
                try
                {
                    _context.TreatmentTempletes.Add(model);
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
        public async Task<bool> UpdateAsync(TreatmentTemplete model)
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
                TreatmentTemplete obj = _context.TreatmentTempletes.Find(Id);
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
            var existCount = await _context.TreatmentTempletes.CountAsync(a => a.IsActive && a.TreatmentName == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public async Task<bool> IsExistItemForUpdateAsync(int id, string name = "")
        {
            var existCount = await _context.TreatmentTempletes.CountAsync(a => a.IsActive && a.Id != id && a.TreatmentName == name.Trim());
            if (existCount > 0)
                return true;

            return false;
        }
        public async Task<PagedList.IPagedList<TreatmentTemplete>> GetAllPageListAsync(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = await _context.TreatmentTempletes.Where(a => a.IsActive).CountAsync();
                var data = await _context.TreatmentTempletes.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<TreatmentTemplete>(data.OrderBy(a => a.TreatmentName), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = await _context.TreatmentTempletes.Where(a => a.IsActive && a.TreatmentName.Contains(searchString)).CountAsync();
                var data = await _context.TreatmentTempletes.Where(a => a.IsActive && a.TreatmentName.Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<TreatmentTemplete>(data.OrderBy(o => o.TreatmentName), pageNo, rowNo, totalRows);
            }
        }
        public async Task<PagedList.IPagedList<TreatmentTemplete>> GetAllPageListByDoctorIdAsync(int pageNo, int rowNo, string searchString, int doctorId)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = await _context.TreatmentTempletes.Where(a => a.IsActive && a.DoctorId == doctorId).CountAsync();
                var data = await _context.TreatmentTempletes.Where(a => a.IsActive && a.DoctorId == doctorId).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<TreatmentTemplete>(data.OrderBy(a => a.TreatmentName), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = await _context.TreatmentTempletes.Where(a => a.IsActive && a.DoctorId == doctorId && a.TreatmentName.Contains(searchString)).CountAsync();
                var data = await _context.TreatmentTempletes.Where(a => a.IsActive && a.DoctorId == doctorId && a.TreatmentName.Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<TreatmentTemplete>(data.OrderBy(o => o.TreatmentName), pageNo, rowNo, totalRows);
            }
        }

        public async Task<bool> IsExistingTemplate(int doctorId, string name)
        {
            var existCount = await _context.TreatmentTempletes.CountAsync(a => a.IsActive && a.DoctorId == doctorId && a.TreatmentName == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public async Task<bool> IsExistingTemplateForUpdateAsync(int id, int doctorId, string name = "")
        {
            var existCount = await _context.TreatmentTempletes.CountAsync(a => a.IsActive && a.Id != id && a.DoctorId == doctorId && a.TreatmentName == name.Trim());
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