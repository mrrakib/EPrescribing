using EPrescribing.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface ICheifComplaientService : IDisposable
    {
        List<ChiefComplaint> GetByPrescription(int prescriptionId);
        bool Update(ChiefComplaint model);
        bool Add(ChiefComplaint model);
        int Delete(List<ChiefComplaint> oldData, List<ChiefComplaint> newData);
    }
    public class CheifComplaientService : ICheifComplaientService
    {
        private readonly AppEntities _context;

        public CheifComplaientService()
        {
            _context = new AppEntities();
        }
        public List<ChiefComplaint> GetByPrescription(int prescriptionId)
        {
            return _context.ChiefComplaints.Where(a => a.IsActive && a.PrescriptionId == prescriptionId).OrderBy(a => a.Id).ToList();
        }
        public bool Update(ChiefComplaint model)
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
        public bool Add(ChiefComplaint model)
        {
            if (model != null)
            {
                try
                {
                    _context.ChiefComplaints.Add(model);
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
        public int Delete(List<ChiefComplaint> oldData, List<ChiefComplaint> newData)
        {
            if (oldData.Count == newData.Count)
                return 0;

            int rowDelete = 0;
            foreach (var item in oldData)
            {
                var rowAffect = newData.Count(a => a.Id == item.Id);
                if (rowAffect == 0)
                {
                    var dataDelete = _context.ChiefComplaints.FirstOrDefault(a => a.Id == item.Id);
                    dataDelete.IsActive = false;
                    rowDelete += Update(dataDelete) ? 1 : 0;
                }
            }
            return rowDelete;
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