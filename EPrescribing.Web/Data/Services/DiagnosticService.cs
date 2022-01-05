using EPrescribing.Web.Models;
using EPrescribing.Web.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EPrescribing.Web.Data.Services
{
    public interface IDiagnosticService : ICommonService<Diagnostic>, IDisposable
    {
        List<VMDDLItems> GetAllDiagnosisDDL(int? Id = null);
        Task<List<Diagnostic>> GetAllParentDiagnosisAsync();
        Task<IEnumerable<Diagnostic>> GetAllDiagnosticByParentIdAsync(int parentId);
    }
    public class DiagnosticService : IDiagnosticService
    {
        private readonly AppEntities _context;

        public DiagnosticService()
        {
            _context = new AppEntities();
        }

        public async Task<IEnumerable<Diagnostic>> GetAllAsync()
        {
            return await _context.Diagnostics.Where(a => a.IsActive).ToListAsync();
        }
        public async Task<List<Diagnostic>> GetAllParentDiagnosisAsync()
        {
            return await _context.Diagnostics.Where(a => a.IsActive && (a.ParentId == null || a.ParentId == 0)).ToListAsync();
        }
        public async Task<IEnumerable<Diagnostic>> GetAllDiagnosticByParentIdAsync(int parentId)
        {

            return await _context.Diagnostics.Where(a => a.IsActive && a.ParentId != null && a.ParentId > 0 && (parentId > 0 ? a.ParentId == parentId : true)).ToListAsync();



        }
        public async Task<int> GetCountAsync()
        {
            return await _context.Diagnostics.Where(a => a.IsActive).CountAsync();
        }
        public async Task<Diagnostic> FindAsync(int? id)
        {
            return await _context.Diagnostics.FindAsync(id ?? 0);
        }
        public async Task<bool> AddAsync(Diagnostic model)
        {
            if (model != null)
            {
                try
                {
                    _context.Diagnostics.Add(model);
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
        public async Task<bool> UpdateAsync(Diagnostic model)
        {
            if (model != null)
            {
                try
                {
                    model.UpdatedDate = DateTime.Now;
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
                Diagnostic obj = _context.Diagnostics.Find(Id);
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
            var existCount = await _context.Diagnostics.CountAsync(a => a.IsActive && a.TestName == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public async Task<bool> IsExistItemForUpdateAsync(int id, string name = "")
        {
            var existCount = await _context.Diagnostics.CountAsync(a => a.IsActive && a.Id != id && a.TestName == name.Trim());
            if (existCount > 0)
                return true;

            return false;
        }
        public async Task<IPagedList<Diagnostic>> GetAllPageListAsync(int pageNo, int rowNo, string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                int totalRows = await _context.Diagnostics.Where(a => a.IsActive).CountAsync();
                var totalData=_context.Diagnostics.ToList();
                var parentData=totalData.Where(m => m.ParentId == null).ToList();
                List<Diagnostic> diagnosticsList = new List<Diagnostic>();
                foreach (var item in parentData)
                {
                    diagnosticsList.Add(item);
                    diagnosticsList.AddRange(totalData.Where(l => l.ParentId == item.Id).ToList());
                }
                var data = diagnosticsList.Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<Diagnostic>(data, pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = await _context.Diagnostics.Where(a => a.IsActive && a.TestName.Contains(searchString)).CountAsync();
                var totalData = _context.Diagnostics.Where(a => a.IsActive && a.TestName.Contains(searchString)).ToList();
                var parentData = totalData.Where(m => m.ParentId == null).ToList();
                List<Diagnostic> diagnosticsList = new List<Diagnostic>();
                foreach (var item in parentData)
                {
                    diagnosticsList.Add(item);
                    diagnosticsList.AddRange(totalData.Where(l => l.ParentId == item.Id).ToList());
                }
                var data = diagnosticsList.Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();               
                return new StaticPagedList<Diagnostic>(data, pageNo, rowNo, totalRows);
            }
        }
        public List<VMDDLItems> GetAllDiagnosisDDL(int? Id = null)
        {
            if (!Id.HasValue)
            {
                return _context.Diagnostics.Where(d => d.IsActive && d.Id != Id.Value).Select(d => new VMDDLItems
                {
                    Id = d.Id,
                    Name = d.TestName
                }).ToList();
            }
            else
            {
                return _context.Diagnostics.Where(d => d.IsActive).Select(d => new VMDDLItems
                {
                    Id = d.Id,
                    Name = d.TestName
                }).ToList();
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