using EPrescribing.Web.Models;
using EPrescribing.Web.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface IPatientService : IDisposable
    {
        IEnumerable<Patient> GetAll();
        IEnumerable<Patient> GetPatientByMobileNo(string mobileNo, int doctorId);
        Patient GetPatientByPatientId(string patientId, int doctorId);
        Patient GetPatientByPatientId(int patientId, int doctorId);
        string GetPatientSerialNoByDate(DateTime date, int doctorId);
        IPagedList<Patient> GetPatientsList(int pageNo, int rowNo, string searchString, string SearchMobileNo, int doctorId);
        IPagedList<VMPatientRecord> GetAllPatientRecords(int pageNo, int rowNo, string searchString, int doctorId, string searchId);
        int GetCount();
        Patient GetDetails(int Id);
        bool Add(Patient model);
        bool Update(Patient model);
        bool Delete(int Id);
        bool IsExistItem(string name);
        bool IsExistItemForUpdate(int id, string name);
        VMPatientRecord GetPatientRecordById(int patientId);
        string GetLastSeenDateByPatientId(int patientId);
        PagedList<Patient> GetPatientsPayments(int patientId, int doctorId, string name, string sorting, int pageNo, int rowNo);
    }
    public class PatientService : IPatientService
    {
        private readonly AppEntities _context;

        public PatientService(AppEntities context)
        {
            _context = context;
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
            var existCount = _context.Patients.Count(a => a.IsActive && a.Id != id && a.Name == name.Trim());
            if (existCount > 0)
                return true;
            return false;
        }
        public IEnumerable<Patient> GetAll()
        {
            return _context.Patients;
        }
        public IPagedList<Patient> GetPatientsList(int pageNo, int rowNo, string searchString, string SearchMobileNo, int doctorId)
        {
            if (string.IsNullOrEmpty(searchString) && string.IsNullOrEmpty(SearchMobileNo))
            {
                int totalRows = _context.Patients.Where(a => a.IsActive && a.DoctorId == doctorId).Count();
                var data = _context.Patients.Where(a => a.IsActive && a.DoctorId == doctorId).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<Patient>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);
            }
            else
            {
                searchString = searchString.Trim();
                SearchMobileNo = SearchMobileNo.Trim();
                int totalRows = _context.Patients.Where(a => a.IsActive && a.DoctorId == doctorId && (!string.IsNullOrEmpty(searchString) ? a.Name.Contains(searchString) : true) && (!string.IsNullOrEmpty(SearchMobileNo) ? a.MobileNo.Contains(SearchMobileNo) : true)).Count();
                var data = _context.Patients.Where(a => a.IsActive && a.DoctorId == doctorId && (!string.IsNullOrEmpty(searchString) ? a.Name.Contains(searchString) : true) && (!string.IsNullOrEmpty(SearchMobileNo) ? a.MobileNo.Contains(SearchMobileNo) : true)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<Patient>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);
            }
        }
        public IPagedList<VMPatientRecord> GetAllPatientRecords(int pageNo, int rowNo, string searchString, int doctorId, string searchId)
        {
            List<VMPatientRecord> result = new List<VMPatientRecord>();
            var patients = _context.Patients.Where(p => p.DoctorId == doctorId).ToList();
            foreach (var patient in patients)
            {
                if (patient.Prescriptions != null)
                {
                    foreach (var prescription in patient.Prescriptions)
                    {
                        VMPatientRecord patientRecord = new VMPatientRecord();
                        patientRecord.PatientId = patient.Id;
                        patientRecord.PatientStringID = patient.PatientID;
                        patientRecord.Age = patient.Age;
                        patientRecord.MobileNo = patient.MobileNo;
                        patientRecord.PatientName = patient.Name;
                        patientRecord.PrescriptionId = prescription.Id;
                        patientRecord.Date = prescription.CreatedDate;
                        patientRecord.Status = prescription.Status;
                        patientRecord.NextVisitDate = prescription.NextAppointmentDate != null ? prescription.NextAppointmentDate.Value.ToString("dd MMM,yyyy") : "";
                        result.Add(patientRecord);
                    }
                }
            }

            int totalRows = 0;
            List<VMPatientRecord> data = new List<VMPatientRecord>();
            if (searchId == "ICR")
            {
                result = result.Where(a => (a.Status == "H" || a.Status == "E")).ToList();
            }
            else if (searchId == "TD")
            {
                result = result.Where(a => a.Date.Date == DateTime.Now.Date).ToList();
            }
            else if (searchId == "TM")
            {
                result = result.Where(a => (a.Date.Month == DateTime.Now.Month && a.Date.Year == DateTime.Now.Year)).ToList();
            }

            if (String.IsNullOrEmpty(searchString))
            {
                totalRows = result.Count();
                data = result.OrderByDescending(a => a.Date).ThenByDescending(a=>a.Status).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
            }
            else
            {
                totalRows = result.Where(a => a.PatientName.ToLower().StartsWith(searchString.ToLower())).Count();
                data = result.Where(a => a.PatientName.ToLower().StartsWith(searchString.ToLower())).OrderByDescending(a => a.Date).ThenByDescending(a => a.Status).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
            }

            return new StaticPagedList<VMPatientRecord>(data.OrderByDescending(o => o.Date), pageNo, rowNo, totalRows);
        }
        public IEnumerable<Patient> GetPatientByMobileNo(string mobileNo, int doctorId)
        {
            return _context.Patients.Where(p => p.MobileNo == mobileNo && p.DoctorId == doctorId);
        }

        public Patient GetPatientByPatientId(string patientId, int doctorId)
        {
            return _context.Patients.Where(p => p.PatientID == patientId && p.DoctorId == doctorId).FirstOrDefault();
        }
        public Patient GetPatientByPatientId(int patientId, int doctorId)
        {
            return _context.Patients.Where(p => p.Id == patientId && p.DoctorId == doctorId).FirstOrDefault();
        }
        public string GetLastSeenDateByPatientId(int patientId)
        {

            var prescriptions = _context.Prescriptions.Where(p => p.PatientId == patientId).ToList();
            if (prescriptions != null && prescriptions.Count > 0)
            {
                var lastSeenDate = prescriptions.OrderByDescending(p => p.Id).FirstOrDefault().CreatedDate;
                return lastSeenDate.ToString("dd MMM, yyyy");
            }
            return "";

        }
        public IPagedList<Patient> GetDesignationPageList(int pageNo, int rowNo, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                int totalRows = _context.Patients.Where(a => a.IsActive).Count();
                var data = _context.Patients.Where(a => a.IsActive).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<Patient>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);
            }
            else
            {
                int totalRows = _context.Patients.Where(a => a.IsActive && a.Name.StartsWith(searchString)).Count();
                var data = _context.Patients.Where(a => a.IsActive && a.Name.StartsWith(searchString)).OrderByDescending(a => a.Id).Skip((pageNo - 1) * rowNo).Take(rowNo).ToList();
                return new StaticPagedList<Patient>(data.OrderBy(o => o.Name), pageNo, rowNo, totalRows);

            }

        }
        public int GetCount()
        {
            return _context.Patients.Count();
        }

        public string GetPatientSerialNoByDate(DateTime date, int doctorId)
        {
            string serialNo = "";
            var res = _context.Patients.Where(m => m.DoctorId == doctorId && m.TretmentDate.Year == date.Year).OrderByDescending(m => m.Id).FirstOrDefault();
            if (res != null)
            {
                string maxPatientID = res.PatientID;
                var splitedId = maxPatientID.Split('-').LastOrDefault();

                int strInput = Convert.ToInt32(splitedId.TrimStart('0'));

                strInput += 1;
                serialNo = strInput.ToString("D4");
            }
            else
            {

                int splitedId = 1;
                serialNo = splitedId.ToString("D4");
            }
            return serialNo;
        }
        public Patient GetDetails(int Id)
        {
            return _context.Patients.Find(Id);
        }
        public bool Add(Patient model)
        {
            if (model != null)
            {
                try
                {
                    _context.Patients.Add(model);
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

        public bool Update(Patient model)
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
                Patient obj = _context.Patients.Find(Id);
                _context.Patients.Remove(obj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public VMPatientRecord GetPatientRecordById(int patientId)
        {
            VMPatientRecord record = new VMPatientRecord();
            Patient patient = _context.Patients.Find(patientId);
            if (patient != null)
            {
                record.PatientName = patient.Name;
                record.Age = patient.Age;
                record.MobileNo = patient.MobileNo;
                record.Gender = patient.Gender;
                record.PatientStringID = patient.PatientID;
            }
            return record;
        }

        public PagedList<Patient> GetPatientsPayments(int patientId, int doctorId, string name, string sorting, int pageNo, int rowNo)
        {
            var data = _context.Patients.Where(p => p.DoctorId == doctorId);
            if (patientId > 0)
            {
                data = data.Where(a => a.Id == patientId);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                data = data.Where(a => a.Name.Contains(name));
            }

            if (sorting == "Date")
            {
                data = data.OrderByDescending(a => a.CreatedDate);
            }
            else
            {
                data = data.OrderBy(a => a.Name);
            }
            return (PagedList<Patient>)data.ToPagedList(pageNo, rowNo);
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