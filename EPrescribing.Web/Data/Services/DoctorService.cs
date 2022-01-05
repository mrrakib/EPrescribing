using EPrescribing.Web.Models;
using EPrescribing.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EPrescribing.Web.Data.Services
{
    public interface IDoctorService : IDisposable
    {
        Task<PagedList.IPagedList<Doctor>> GetAllPageListAsync(int pageNo, int rowNo, string searchString);
        IEnumerable<Doctor> GetAll();
        int GetCount();
        Doctor GetDetails(int id);
        Doctor GetDoctorByAppUserID(string userId);
        bool Add(Doctor model);
        bool Update(Doctor model);
        bool Delete(int Id);
        bool IsExistItem(string name);
        bool IsExistItemForUpdate(int id, string name);
        VMStatistics GetStatistics(int doctorId);
    }

    public class DoctorService : IDoctorService
    {
        private readonly AppEntities _context;

        public DoctorService(AppEntities context)
        {
            _context = context;
        }

        public async Task<PagedList.IPagedList<Doctor>> GetAllPageListAsync(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = await _context.Doctors.CountAsync();
                var data = await _context.Doctors.OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<Doctor>(data.OrderBy(a => a.Name), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = await _context.Doctors.Where(a => a.Name.Contains(searchString)).CountAsync();
                var data = await _context.Doctors.Where(a => a.Name.Contains(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToListAsync();
                return new PagedList.StaticPagedList<Doctor>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);
            }
        }
        public bool IsExistItem(string name)
        {
            var existCount = _context.Departments.Count(a => a.IsActive && a.Name == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public bool IsExistItemForUpdate(int id, string name)
        {
            var existCount = _context.Departments.Count(a => a.IsActive && a.Id != id && a.Name == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public IEnumerable<Doctor> GetAll()
        {
            return _context.Doctors.Where(a => a.IsActive);
        }
        public int GetCount()
        {
            return _context.Doctors.Count();
        }
        public Doctor GetDetails(int id)
        {
            return _context.Doctors.FirstOrDefault(a => a.Id == id);
        }

        public Doctor GetDoctorByAppUserID(string userId)
        {
            return _context.Doctors.Where(d => d.AppUserID == userId).FirstOrDefault();
        }
        public bool Add(Doctor model)
        {
            if (model != null)
            {
                try
                {
                    _context.Doctors.Add(model);
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

        public bool Update(Doctor model)
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
                Doctor obj = _context.Doctors.Find(Id);
                _context.Doctors.Remove(obj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public VMStatistics GetStatistics(int doctorId)
        {
            int month = DateTime.Now.Month;
            string query = string.Format(@"EXEC GetDashboardReports {0}, {1}", doctorId, month);
            VMStatistics result = _context.Database.SqlQuery<VMStatistics>(query).FirstOrDefault();

            int months = 12;
            List<double> yearlyPaidData = new List<double>();
            List<double> yearlyDueData = new List<double>();
            for (int i = 1; i <= months; i++)
            {
                if (doctorId > 0)
                {
                    var patient = _context.Patients.Where(p => p.DoctorId == doctorId && p.CreatedDate.Month == i).FirstOrDefault();
                    yearlyPaidData.Add(patient != null ? patient.TotalPaid : 0);
                    yearlyDueData.Add(patient != null ? patient.TotalDue : 0);
                }
                else
                {
                    var patient = _context.Patients.Where(p => p.CreatedDate.Month == i).FirstOrDefault();
                    yearlyPaidData.Add(patient != null ? patient.TotalPaid : 0);
                    yearlyDueData.Add(patient != null ? patient.TotalDue : 0);
                }
                
            }
            string paidAmount = string.Join(",", yearlyPaidData);
            string dueAmount = string.Join(",", yearlyDueData);
            result.YearlyPaid = paidAmount;
            result.YearlyDue = dueAmount;

            return result;
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