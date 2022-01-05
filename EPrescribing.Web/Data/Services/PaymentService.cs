using EPrescribing.Web.Models;
using EPrescribing.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EPrescribing.Web.Data.Services
{
    public interface IPaymentService : ICommonService<Payment>, IDisposable
    {
        Task<IEnumerable<Payment>> GetAllByPatientIdAsync(int patientId);
        Payment GetPayment(int prescriptionId);
        IEnumerable<DropdownList> GetPaymentMethods();
        IEnumerable<DropdownList> GetIsActiveStatus();
    }
    public class PaymentService : IPaymentService
    {
        private readonly AppEntities _context;

        public PaymentService()
        {
            _context = new AppEntities();
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments.Where(a => a.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetAllByPatientIdAsync(int patientId)
        {
            return await _context.Payments.Where(a => a.IsActive && a.PatientId == patientId).ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Payments.Where(a => a.IsActive).CountAsync();
        }
        public async Task<Payment> FindAsync(int? id)
        {
            return await _context.Payments.FindAsync(id ?? 0);
        }
        public async Task<bool> AddAsync(Payment model)
        {
            if (model != null)
            {
                try
                {
                    _context.Payments.Add(model);
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
        public async Task<bool> UpdateAsync(Payment model)
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
                Payment obj = _context.Payments.Find(Id);
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
            var existCount = await _context.Diseases.CountAsync(a => a.IsActive && a.Name == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public async Task<bool> IsExistItemForUpdateAsync(int id, string name = "")
        {
            var existCount = await _context.Diseases.CountAsync(a => a.IsActive && a.Id != id && a.Name == name.Trim());
            if (existCount > 0)
                return true;

            return false;
        }
        public async Task<PagedList.IPagedList<Payment>> GetAllPageListAsync(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = await _context.Payments.Where(a => a.IsActive).CountAsync();
                var data = await _context.Payments.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<Payment>(data, pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = await _context.Payments.Where(a => a.IsActive).CountAsync();
                var data = await _context.Payments.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<Payment>(data, pageNo, rowNo, totalRows);
            }
        }
        public Payment GetPayment(int prescriptionId)
        {
            return _context.Payments.OrderByDescending(a => a.CreatedDate).FirstOrDefault(a => a.PrescriptionId == prescriptionId);
        }

        public IEnumerable<DropdownList> GetPaymentMethods()
        {
            var methods = new List<DropdownList>()
            {
                new DropdownList(){Id="bKash", Name="bKash"},
                new DropdownList(){Id="Nagad", Name="Nagad"},
                new DropdownList(){Id="Rocket", Name="Rocket"}
            };
            return methods;
        }
        public IEnumerable<DropdownList> GetIsActiveStatus()
        {
            var methods = new List<DropdownList>()
            {
                new DropdownList(){Id="Active", Name="Active"},
                new DropdownList(){Id="InActive", Name="In Active"}
            };
            return methods;
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