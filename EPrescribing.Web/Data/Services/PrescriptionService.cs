using EPrescribing.Web.Enumerations;
using EPrescribing.Web.Models;
using EPrescribing.Web.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface IPrescriptionService : IDisposable
    {
        IEnumerable<Prescription> GetAll();
        int GetCount();
        Prescription GetDetails(int Id);
        VMPreviewPrescription GetPrescriptionForPreviewOrPdf(int prescriptionId);
        bool Add(Prescription model);
        bool AddOrUpdatesHistory(Prescription model, bool isContinueWithPrescription);
        bool Update(Prescription model);
        bool UpdateOnExamination(Prescription model);
        bool UpdateTreatMentDone(Prescription model);
        bool Delete(int Id);
        Prescription GetPrescriptionHistory(int prescriptionId);
        Prescription GetPrescriptionOnExamination(int prescriptionId);
        Prescription GetPrescriptionDone(int prescriptionId);
        IEnumerable<Prescription> GetAllDonePrescriptionByPatientId(int patientId);
        int GetPrescriptionDoneCountByPatientId(int patientId);
        PagedList<Prescription> GetPrescriptionByPatientId(int patientId, int doctorId, int page, int pageSize);
        Prescription GetLastPrescriptions(int patientIntId);
    }

    public class PrescriptionService : IPrescriptionService
    {
        private readonly AppEntities _context;

        public PrescriptionService()
        {
            _context = new AppEntities();
        }

        public IEnumerable<Prescription> GetAll()
        {
            return _context.Prescriptions;
        }

        public int GetCount()
        {
            return _context.Prescriptions.Count();
        }

        public Prescription GetDetails(int Id)
        {
            return _context.Prescriptions.Find(Id);
        }
        public VMPreviewPrescription GetPrescriptionForPreviewOrPdf(int prescriptionId)
        {
            var model = new VMPreviewPrescription();
            model.PrescriptionId = prescriptionId;
            model.Prescription = GetDetails(prescriptionId);
            model.DoctorInfo = model.Prescription?.Doctor;
            model.PatientInfo = model.Prescription?.Patient;

            model.CheifComplaients = _context.ChiefComplaints.Where(a => a.IsActive && a.PrescriptionId == prescriptionId).OrderBy(a => a.Id).ToList();
            model.MedicineHistories = _context.MedicineHistories.Where(a => a.IsActive && a.PrescriptionId == prescriptionId).OrderBy(a => a.Id).ToList();
            model.OnExaminationExtraoral = _context.OnExaminations.Where(a => a.IsActive && a.Status == OnExaminationStatusEnum.Extraoral.ToString() && a.PrescriptionId == prescriptionId).OrderBy(a => a.Id).ToList();
            model.OnExaminationIntraoral = _context.OnExaminations.Where(a => a.IsActive && a.Status == OnExaminationStatusEnum.Intraoral.ToString() && a.PrescriptionId == prescriptionId).OrderBy(a => a.Id).ToList();
            model.DiagnosticTests = _context.DiagnosticTests.Where(a => a.IsActive && a.PrescriptionId == prescriptionId).OrderBy(a => a.Id).ToList();
            model.TreatmentPlans = _context.TreatmentPlans.Where(a => a.IsActive && a.PrescriptionId == prescriptionId).OrderBy(a => a.Id).ToList();
            model.DifferentialDiagnosis = _context.DifferentialDiagnosis.Where(a => a.IsActive && a.PrescriptionId == prescriptionId).OrderBy(a => a.Id).ToList();
            model.TreatmentDones = _context.TreatmentDones.Where(a => a.IsActive && a.PrescriptionId == prescriptionId).OrderBy(a => a.Id).ToList();
            model.PrescribedMedicines = _context.PrescribedMedicines.Where(a => a.IsActive && a.PrescriptionId == prescriptionId).OrderBy(a => a.Id).ToList();
            model.PrescribedAdvices = _context.PrescribedAdvices.Where(a => a.IsActive && a.PrescriptionId == prescriptionId).OrderBy(a => a.Id).ToList();

            return model;
        }
        public bool Add(Prescription model)
        {
            if (model != null)
            {
                try
                {
                    _context.Prescriptions.Add(model);
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
        public bool AddOrUpdatesHistory(Prescription model, bool isContinueWithPrescription)
        {
            if (model != null)
            {
                using (var _context = new AppEntities())
                {
                    using (DbContextTransaction transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (isContinueWithPrescription)
                            {
                                model.ChiefComplaints = model.ChiefComplaints.Where(d => !string.IsNullOrWhiteSpace(d.Description)).ToList();
                                model.MedicineHistory = model.MedicineHistory.Where(d => !string.IsNullOrWhiteSpace(d.MedicineName)).ToList();
                                model.PreviousPrescriptionId = isContinueWithPrescription ? model.Id : 0;
                                model.CreatedDate = DateTime.Now;
                                model.NextAppointmentDate = null;
                                model.PaymentId = null;

                                _context.Prescriptions.Add(model);
                                _context.SaveChanges();

                                var previousPrescription = _context.Prescriptions.FirstOrDefault(a => a.Id == model.PreviousPrescriptionId);

                                #region Examination
                                model.OnExaminations = previousPrescription.OnExaminations.Select(a => new OnExamination() { Status = a.Status, Description = a.Description, UpperLeft = a.UpperLeft, UpperRight = a.UpperRight, BottomLeft = a.BottomLeft, BottomRight = a.BottomRight, PrescriptionId = model.Id }).ToList();
                                model.DiagnosticTest = previousPrescription.DiagnosticTest.Select(a => new DiagnosticTest() { DiagnosticName = a.DiagnosticName, PrescriptionId = model.Id }).ToList();
                                model.DifferentialDiagnosis = previousPrescription.DifferentialDiagnosis.Select(a => new DifferentialDiagnosis() { DiseaseName = a.DiseaseName, PrescriptionId = model.Id }).ToList();
                                model.TreatmentPlans = previousPrescription.TreatmentPlans.Select(a => new TreatmentPlan() { TreatmentName = a.TreatmentName, UpperLeft = a.UpperLeft, UpperRight = a.UpperRight, BottomLeft = a.BottomLeft, BottomRight = a.BottomRight, PrescriptionId = model.Id }).ToList(); ;
                                #endregion

                                #region TreatmentDone
                                model.TreatmentDones = previousPrescription.TreatmentDones.Select(a => new TreatmentDone() { TreatmentName = a.TreatmentName, UpperLeft = a.UpperLeft, UpperRight = a.UpperRight, BottomLeft = a.BottomLeft, BottomRight = a.BottomRight, PrescriptionId = model.Id }).ToList();
                                //model.PrescribedMedicines = null;
                                //model.PrescribedAdvices = null;
                                model.MedicinePrescribed = previousPrescription.MedicinePrescribed;
                                model.AdvicePrescribed = previousPrescription.AdvicePrescribed;
                                //model.Payment = null;
                                #endregion

                                model.UpdatedDate = DateTime.Now;
                                _context.Entry(model).State = EntityState.Modified;
                                _context.SaveChanges();

                                transaction.Commit();
                                return true;
                            }
                            else if (model.Id > 0)
                            {
                                Prescription prescription = model;

                                #region update ChiefComplaint

                                List<ChiefComplaint> ChiefComplaintList = _context.ChiefComplaints.AsNoTracking().Where(o => o.PrescriptionId == prescription.Id).ToList();

                                List<ChiefComplaint> addChiefComplaintList = new List<ChiefComplaint>();
                                List<ChiefComplaint> delChiefComplaintList = new List<ChiefComplaint>();
                                List<ChiefComplaint> editChiefComplaintList = new List<ChiefComplaint>();

                                if (prescription.ChiefComplaints != null)
                                {
                                    prescription.ChiefComplaints = prescription.ChiefComplaints.Where(d => !string.IsNullOrWhiteSpace(d.Description)).ToList();

                                    addChiefComplaintList = prescription.ChiefComplaints.Where(p => ChiefComplaintList.All(p2 => p2.Id != p.Id)).ToList();

                                    delChiefComplaintList = ChiefComplaintList.Where(p => prescription.ChiefComplaints.All(p2 => p2.Id != p.Id)).ToList();

                                    editChiefComplaintList = prescription.ChiefComplaints.Where(p => ChiefComplaintList.Any(p2 => p2.Id == p.Id)).ToList();
                                }

                                if (addChiefComplaintList != null)
                                {
                                    _context.ChiefComplaints.AddRange(addChiefComplaintList);
                                    _context.SaveChanges();

                                }
                                if (editChiefComplaintList != null)
                                {
                                    foreach (var item in editChiefComplaintList)
                                    {
                                        _context.Entry(item).State = EntityState.Modified;
                                        _context.SaveChanges();
                                    }

                                }
                                if (delChiefComplaintList != null)
                                {
                                    foreach (var item in delChiefComplaintList)
                                    {
                                        _context.Entry(item).State = EntityState.Deleted;
                                        _context.SaveChanges();
                                    }
                                }

                                #endregion

                                #region update MedicineHistory
                                List<MedicineHistory> MedicineHistoryList = _context.MedicineHistories.AsNoTracking().Where(o => o.PrescriptionId == prescription.Id).ToList();

                                List<MedicineHistory> addMedicineHistoryList = new List<MedicineHistory>();
                                List<MedicineHistory> delMedicineHistoryList = new List<MedicineHistory>();
                                List<MedicineHistory> editMedicineHistoryList = new List<MedicineHistory>();

                                if (prescription.MedicineHistory != null)
                                {
                                    prescription.MedicineHistory = prescription.MedicineHistory.Where(d => !string.IsNullOrWhiteSpace(d.MedicineName)).ToList();

                                    addMedicineHistoryList = prescription.MedicineHistory.Where(p => MedicineHistoryList.All(p2 => p2.Id != p.Id)).ToList();

                                    delMedicineHistoryList = MedicineHistoryList.Where(p => prescription.MedicineHistory.All(p2 => p2.Id != p.Id)).ToList();

                                    editMedicineHistoryList = prescription.MedicineHistory.Where(p => MedicineHistoryList.Any(p2 => p2.Id == p.Id)).ToList();
                                }

                                if (addMedicineHistoryList != null)
                                {
                                    _context.MedicineHistories.AddRange(addMedicineHistoryList);
                                    _context.SaveChanges();

                                }
                                if (editMedicineHistoryList != null)
                                {
                                    foreach (var item in editMedicineHistoryList)
                                    {
                                        _context.Entry(item).State = EntityState.Modified;
                                        _context.SaveChanges();
                                    }

                                }
                                if (delMedicineHistoryList != null)
                                {
                                    foreach (var item in delMedicineHistoryList)
                                    {
                                        _context.Entry(item).State = EntityState.Deleted;
                                        _context.SaveChanges();
                                    }
                                }

                                #endregion

                                prescription.ChiefComplaints = null;
                                prescription.MedicineHistory = null;
                                prescription.UpdatedDate = DateTime.Now;
                                _context.Entry(prescription).State = EntityState.Modified;
                                _context.SaveChanges();

                                transaction.Commit();
                                return true;
                            }
                            else
                            {
                                model.ChiefComplaints = model.ChiefComplaints.Where(d => !string.IsNullOrWhiteSpace(d.Description)).ToList();
                                model.MedicineHistory = model.MedicineHistory.Where(d => !string.IsNullOrWhiteSpace(d.MedicineName)).ToList();
                                model.PreviousPrescriptionId = isContinueWithPrescription ? model.Id : 0;
                                model.CreatedDate = DateTime.Now;
                                model.NextAppointmentDate = null;
                                model.PaymentId = null;

                                _context.Prescriptions.Add(model);
                                _context.SaveChanges();
                                transaction.Commit();
                                return true;
                            }




                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        public bool Update(Prescription model)
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

        public bool UpdateOnExamination(Prescription model)
        {
            if (model != null)
            {
                using (var _context = new AppEntities())
                {
                    using (DbContextTransaction transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            Prescription prescription = model;

                            #region Update OnExamination
                            List<OnExamination> OnExaminationsList = _context.OnExaminations.AsNoTracking().Where(o => o.PrescriptionId == prescription.Id).ToList();

                            List<OnExamination> addOnExaminationsList = new List<OnExamination>();
                            List<OnExamination> delOnExaminationsList = new List<OnExamination>();
                            List<OnExamination> editOnExaminationsList = new List<OnExamination>();

                            if (prescription.OnExaminations != null)
                            {
                                prescription.OnExaminations = prescription.OnExaminations.Where(e => !string.IsNullOrWhiteSpace(e.Description)).ToList();
                                addOnExaminationsList = prescription.OnExaminations.Where(p => OnExaminationsList.All(p2 => p2.Id != p.Id)).ToList();

                                delOnExaminationsList = OnExaminationsList.Where(p => prescription.OnExaminations.All(p2 => p2.Id != p.Id)).ToList();

                                editOnExaminationsList = prescription.OnExaminations.Where(p => OnExaminationsList.Any(p2 => p2.Id == p.Id)).ToList();
                            }
                            if (addOnExaminationsList.Count > 0)
                            {
                                _context.OnExaminations.AddRange(addOnExaminationsList);
                                _context.SaveChanges();

                            }
                            if (editOnExaminationsList.Count > 0)
                            {
                                foreach (var item in editOnExaminationsList)
                                {
                                    _context.Entry(item).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }

                            }
                            if (delOnExaminationsList.Count > 0)
                            {
                                foreach (var item in delOnExaminationsList)
                                {
                                    _context.Entry(item).State = EntityState.Deleted;
                                    _context.SaveChanges();
                                }
                            }
                            #endregion

                            #region Update DiagnosticTest
                            List<DiagnosticTest> DiagnosticTestList = _context.DiagnosticTests.AsNoTracking().Where(o => o.PrescriptionId == prescription.Id).ToList();

                            List<DiagnosticTest> addDiagnosticTestList = new List<DiagnosticTest>();
                            List<DiagnosticTest> delDiagnosticTestList = new List<DiagnosticTest>();
                            List<DiagnosticTest> editDiagnosticTestList = new List<DiagnosticTest>();

                            if (prescription.DiagnosticTest != null)
                            {
                                prescription.DiagnosticTest = prescription.DiagnosticTest.Where(d => !string.IsNullOrWhiteSpace(d.DiagnosticName)).ToList();
                                addDiagnosticTestList = prescription.DiagnosticTest.Where(p => DiagnosticTestList.All(p2 => p2.Id != p.Id)).ToList();

                                delDiagnosticTestList = DiagnosticTestList.Where(p => prescription.DiagnosticTest.All(p2 => p2.Id != p.Id)).ToList();

                                editDiagnosticTestList = prescription.DiagnosticTest.Where(p => DiagnosticTestList.Any(p2 => p2.Id == p.Id)).ToList();
                            }
                            if (addDiagnosticTestList.Count > 0)
                            {
                                _context.DiagnosticTests.AddRange(addDiagnosticTestList);
                                _context.SaveChanges();

                            }
                            if (editDiagnosticTestList.Count > 0)
                            {
                                foreach (var item in editDiagnosticTestList)
                                {
                                    _context.Entry(item).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                            }
                            if (delDiagnosticTestList.Count > 0)
                            {
                                foreach (var item in delDiagnosticTestList)
                                {
                                    _context.Entry(item).State = EntityState.Deleted;
                                    _context.SaveChanges();
                                }
                            }

                            #endregion

                            #region update Differential Diagnosis
                            List<DifferentialDiagnosis> DifferentialDiagnosisList = _context.DifferentialDiagnosis.AsNoTracking().Where(o => o.PrescriptionId == prescription.Id).ToList();

                            List<DifferentialDiagnosis> addDifferentialDiagnosisList = new List<DifferentialDiagnosis>();
                            List<DifferentialDiagnosis> delDifferentialDiagnosisList = new List<DifferentialDiagnosis>();
                            List<DifferentialDiagnosis> editDifferentialDiagnosisList = new List<DifferentialDiagnosis>();

                            if (prescription.DifferentialDiagnosis != null)
                            {
                                prescription.DifferentialDiagnosis = prescription.DifferentialDiagnosis.Where(e => !string.IsNullOrWhiteSpace(e.DiseaseName)).ToList();
                                addDifferentialDiagnosisList = prescription.DifferentialDiagnosis.Where(p => DifferentialDiagnosisList.All(p2 => p2.Id != p.Id)).ToList();

                                delDifferentialDiagnosisList = DifferentialDiagnosisList.Where(p => prescription.DifferentialDiagnosis.All(p2 => p2.Id != p.Id)).ToList();

                                editDifferentialDiagnosisList = prescription.DifferentialDiagnosis.Where(p => DifferentialDiagnosisList.Any(p2 => p2.Id == p.Id)).ToList();
                            }

                            if (addDifferentialDiagnosisList.Count > 0)
                            {
                                _context.DifferentialDiagnosis.AddRange(addDifferentialDiagnosisList);
                                _context.SaveChanges();

                            }
                            if (editDifferentialDiagnosisList.Count > 0)
                            {
                                foreach (var item in editDifferentialDiagnosisList)
                                {
                                    _context.Entry(item).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }

                            }
                            if (delDifferentialDiagnosisList.Count > 0)
                            {
                                foreach (var item in delDifferentialDiagnosisList)
                                {
                                    _context.Entry(item).State = EntityState.Deleted;
                                    _context.SaveChanges();
                                }
                            }
                            #endregion

                            #region update TreatmentPlan
                            List<TreatmentPlan> TreatmentPlanList = _context.TreatmentPlans.AsNoTracking().Where(o => o.PrescriptionId == prescription.Id).ToList();

                            List<TreatmentPlan> addTreatmentPlanList = new List<TreatmentPlan>();
                            List<TreatmentPlan> delTreatmentPlanList = new List<TreatmentPlan>();
                            List<TreatmentPlan> editTreatmentPlanList = new List<TreatmentPlan>();

                            if (prescription.TreatmentPlans != null)
                            {
                                prescription.TreatmentPlans = prescription.TreatmentPlans.Where(e => !string.IsNullOrWhiteSpace(e.TreatmentName)).ToList();
                                addTreatmentPlanList = prescription.TreatmentPlans.Where(p => TreatmentPlanList.All(p2 => p2.Id != p.Id)).ToList();

                                delTreatmentPlanList = TreatmentPlanList.Where(p => prescription.TreatmentPlans.All(p2 => p2.Id != p.Id)).ToList();

                                editTreatmentPlanList = prescription.TreatmentPlans.Where(p => TreatmentPlanList.Any(p2 => p2.Id == p.Id)).ToList();
                            }

                            if (addTreatmentPlanList.Count > 0)
                            {
                                _context.TreatmentPlans.AddRange(addTreatmentPlanList);
                                _context.SaveChanges();

                            }
                            if (editTreatmentPlanList.Count > 0)
                            {
                                foreach (var item in editTreatmentPlanList)
                                {
                                    _context.Entry(item).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }

                            }
                            if (delTreatmentPlanList.Count > 0)
                            {
                                foreach (var item in delTreatmentPlanList)
                                {
                                    _context.Entry(item).State = EntityState.Deleted;
                                    _context.SaveChanges();
                                }
                            }

                            #endregion

                            prescription.OnExaminations = null;
                            prescription.DiagnosticTest = null;
                            prescription.DifferentialDiagnosis = null;
                            prescription.TreatmentPlans = null;
                            prescription.UpdatedDate = DateTime.Now;
                            _context.Entry(prescription).State = EntityState.Modified;
                            _context.SaveChanges();

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        public bool UpdateTreatMentDone(Prescription model)
        {
            if (model != null)
            {
                using (var _context = new AppEntities())
                {
                    var patient = _context.Patients.Find(model.PatientId);
                    using (DbContextTransaction transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            Prescription prescription = model;

                            #region update TreatmentDone

                            List<TreatmentDone> treatmentDoneList = _context.TreatmentDones.AsNoTracking().Where(o => o.PrescriptionId == prescription.Id).ToList();

                            List<TreatmentDone> addTreatmentDoneList = new List<TreatmentDone>();
                            List<TreatmentDone> delTreatmentDoneList = new List<TreatmentDone>();
                            List<TreatmentDone> editTreatmentDoneList = new List<TreatmentDone>();

                            if (prescription.TreatmentDones != null)
                            {
                                prescription.TreatmentDones = prescription.TreatmentDones.Where(e => !string.IsNullOrWhiteSpace(e.TreatmentName)).ToList();
                                addTreatmentDoneList = prescription.TreatmentDones.Where(p => treatmentDoneList.All(p2 => p2.Id != p.Id)).ToList();

                                delTreatmentDoneList = treatmentDoneList.Where(p => prescription.TreatmentDones.All(p2 => p2.Id != p.Id)).ToList();

                                editTreatmentDoneList = prescription.TreatmentDones.Where(p => treatmentDoneList.Any(p2 => p2.Id == p.Id)).ToList();
                            }

                            if (addTreatmentDoneList.Count > 0)
                            {
                                _context.TreatmentDones.AddRange(addTreatmentDoneList);
                                _context.SaveChanges();
                            }
                            if (editTreatmentDoneList.Count > 0)
                            {
                                foreach (var item in editTreatmentDoneList)
                                {
                                    _context.Entry(item).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }

                            }
                            if (delTreatmentDoneList.Count > 0)
                            {
                                foreach (var item in delTreatmentDoneList)
                                {
                                    _context.Entry(item).State = EntityState.Deleted;
                                    _context.SaveChanges();
                                }
                            }

                            #endregion

                            #region update PrescribedMedicine

                            List<PrescribedMedicine> PrescribedMedicineList = _context.PrescribedMedicines.AsNoTracking().Where(o => o.PrescriptionId == prescription.Id).ToList();

                            List<PrescribedMedicine> addPrescribedMedicineList = new List<PrescribedMedicine>();
                            List<PrescribedMedicine> delPrescribedMedicineList = new List<PrescribedMedicine>();
                            List<PrescribedMedicine> editPrescribedMedicineList = new List<PrescribedMedicine>();

                            if (prescription.PrescribedMedicines != null)
                            {
                                prescription.PrescribedMedicines = prescription.PrescribedMedicines.Where(d => !string.IsNullOrWhiteSpace(d.MedicineName)).ToList();
                                addPrescribedMedicineList = prescription.PrescribedMedicines.Where(p => PrescribedMedicineList.All(p2 => p2.Id != p.Id)).ToList();

                                delPrescribedMedicineList = PrescribedMedicineList.Where(p => prescription.PrescribedMedicines.All(p2 => p2.Id != p.Id)).ToList();

                                editPrescribedMedicineList = prescription.PrescribedMedicines.Where(p => PrescribedMedicineList.Any(p2 => p2.Id == p.Id)).ToList();
                            }

                            if (addPrescribedMedicineList.Count > 0)
                            {
                                _context.PrescribedMedicines.AddRange(addPrescribedMedicineList);
                                _context.SaveChanges();

                            }
                            if (editPrescribedMedicineList.Count > 0)
                            {
                                foreach (var item in editPrescribedMedicineList)
                                {
                                    _context.Entry(item).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }

                            }
                            if (delPrescribedMedicineList.Count > 0)
                            {
                                foreach (var item in delPrescribedMedicineList)
                                {
                                    _context.Entry(item).State = EntityState.Deleted;
                                    _context.SaveChanges();
                                }
                            }

                            #endregion

                            #region update PrescribedAdvice
                            List<PrescribedAdvice> PrescribedAdviceList = _context.PrescribedAdvices.AsNoTracking().Where(o => o.PrescriptionId == prescription.Id).ToList();

                            List<PrescribedAdvice> addPrescribedAdviceList = new List<PrescribedAdvice>();
                            List<PrescribedAdvice> delPrescribedAdviceList = new List<PrescribedAdvice>();
                            List<PrescribedAdvice> editPrescribedAdviceList = new List<PrescribedAdvice>();

                            if (prescription.PrescribedAdvices != null)
                            {
                                prescription.PrescribedAdvices = prescription.PrescribedAdvices.Where(e => !string.IsNullOrWhiteSpace(e.AdviceName)).ToList();
                                addPrescribedAdviceList = prescription.PrescribedAdvices.Where(p => PrescribedAdviceList.All(p2 => p2.Id != p.Id)).ToList();

                                delPrescribedAdviceList = PrescribedAdviceList.Where(p => prescription.PrescribedAdvices.All(p2 => p2.Id != p.Id)).ToList();

                                editPrescribedAdviceList = prescription.PrescribedAdvices.Where(p => PrescribedAdviceList.Any(p2 => p2.Id == p.Id)).ToList();
                            }

                            if (addPrescribedAdviceList.Count > 0)
                            {
                                _context.PrescribedAdvices.AddRange(addPrescribedAdviceList);
                                _context.SaveChanges();

                            }
                            if (editPrescribedAdviceList.Count > 0)
                            {
                                foreach (var item in editPrescribedAdviceList)
                                {
                                    _context.Entry(item).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }

                            }
                            if (delPrescribedAdviceList.Count > 0)
                            {
                                foreach (var item in delPrescribedAdviceList)
                                {
                                    _context.Entry(item).State = EntityState.Deleted;
                                    _context.SaveChanges();
                                }
                            }

                            #endregion

                            #region Update PaymentInformation
                            var existingPayment = _context.Payments.Where(p => p.PatientId == prescription.PatientId && p.PrescriptionId == prescription.Id).FirstOrDefault();
                            if (existingPayment != null)
                            {
                                existingPayment.TreatmentCharge = prescription.TreatmentCharge;
                                existingPayment.PaidAmount = prescription.TodayPaid;
                                existingPayment.DueAmount = prescription.TodayDue;
                                existingPayment.TotalDueAmount = prescription.TotalDue;

                                _context.Entry(existingPayment).State = EntityState.Modified;
                                _context.SaveChanges();

                                patient.TotalCharge = prescription.TotalCharge;
                                patient.TotalPaid = prescription.TotalPaid;
                                patient.TotalDue = prescription.TotalDue;
                            }
                            else
                            {
                                Payment payment = new Payment();
                                payment.TreatmentCharge = prescription.TreatmentCharge;
                                payment.PaidAmount = prescription.TodayPaid;
                                payment.DueAmount = prescription.TodayDue;
                                payment.PatientId = prescription.PatientId;
                                payment.PrescriptionId = prescription.Id;
                                payment.PaymentDate = DateTime.Now;
                                payment.TotalDueAmount = prescription.TotalDue;

                                _context.Payments.Add(payment);
                                _context.SaveChanges();

                                patient.TotalCharge = prescription.TotalCharge;
                                patient.TotalPaid = prescription.TotalPaid;
                                patient.TotalDue = prescription.TotalDue;

                                prescription.PaymentId = payment.Id;
                            }
                            patient.NextAppointmentDate = prescription.NextAppointmentDate;
                            _context.Entry(patient).State = EntityState.Modified;
                            _context.SaveChanges();

                            //prescription.TreatmentDones = null;
                            //prescription.PrescribedMedicines = null;
                            //prescription.PrescribedAdvices = null;
                            #endregion

                            prescription.UpdatedDate = DateTime.Now;
                            _context.Entry(prescription).State = EntityState.Modified;
                            _context.SaveChanges();

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        public bool Delete(int Id)
        {
            try
            {
                Prescription obj = _context.Prescriptions.Find(Id);
                _context.Prescriptions.Remove(obj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public Prescription GetPrescriptionHistory(int prescriptionId)
        {
            var prescription = GetDetails(prescriptionId);
            if (prescription != null && prescription.Id > 0)
            {
                if (prescription.ChiefComplaints != null && prescription.ChiefComplaints.Count() > 0)
                {
                    prescription.ChiefComplaints = prescription.ChiefComplaints;
                }
                else
                {
                    List<ChiefComplaint> chiefComplaints = new List<ChiefComplaint>();
                    chiefComplaints.Add(new ChiefComplaint { PrescriptionId = prescriptionId });
                    prescription.ChiefComplaints = chiefComplaints;
                }


                if (prescription.MedicineHistory != null && prescription.MedicineHistory.Count() > 0)
                {
                    foreach (var item in prescription.MedicineHistory)
                    {
                        var medicine = _context.Medicines.Where(m => m.Name == item.MedicineName).FirstOrDefault();
                        if (medicine != null)
                        {
                            item.GenericId = medicine.GenericId ?? 0;
                            item.GenericName = medicine.Generic != null ? medicine.Generic.Formulation+"."+medicine.Generic.GenericName+"."+ medicine.Generic.DoseAmount : "";
                        }
                    }
                    prescription.MedicineHistory = prescription.MedicineHistory;
                }
                else
                {
                    List<MedicineHistory> medicineHistory = new List<MedicineHistory>();
                    medicineHistory.Add(new MedicineHistory { PrescriptionId = prescriptionId });
                    prescription.MedicineHistory = medicineHistory;
                }

            }
            else
            {
                prescription = new Prescription();
                List<ChiefComplaint> chiefComplaints = new List<ChiefComplaint>();
                chiefComplaints.Add(new ChiefComplaint());
                prescription.ChiefComplaints = chiefComplaints;

                List<MedicineHistory> medicineHistory = new List<MedicineHistory>();
                medicineHistory.Add(new MedicineHistory());
                prescription.MedicineHistory = medicineHistory;
            }

            return prescription;
        }
        public Prescription GetPrescriptionOnExamination(int prescriptionId)
        {
            var prescription = GetDetails(prescriptionId);
            if (prescription != null && prescription.Id > 0)
            {
                if (prescription.OnExaminations != null && prescription.OnExaminations.Count() > 0)
                {
                    prescription.OnExaminations = prescription.OnExaminations;
                }
                else
                {
                    List<OnExamination> onExaminations = new List<OnExamination>();
                    onExaminations.Add(new OnExamination { PrescriptionId = prescription.Id, Status = OnExaminationStatusEnum.Extraoral.ToString() });
                    onExaminations.Add(new OnExamination { PrescriptionId = prescription.Id, Status = OnExaminationStatusEnum.Intraoral.ToString() });
                    prescription.OnExaminations = onExaminations;
                }
                if (prescription.DiagnosticTest != null && prescription.DiagnosticTest.Count() > 0)
                {
                    foreach (var item in prescription.DiagnosticTest)
                    {
                        var diagonostic = _context.Diagnostics.Where(m => m.TestName == item.DiagnosticName).FirstOrDefault();
                        item.CategoryId = diagonostic != null ? diagonostic.ParentId ?? 0 : 0;
                    }
                    prescription.DiagnosticTest = prescription.DiagnosticTest;
                }
                else
                {
                    List<DiagnosticTest> diagnosticTest = new List<DiagnosticTest>();
                    diagnosticTest.Add(new DiagnosticTest { PrescriptionId = prescription.Id });
                    prescription.DiagnosticTest = diagnosticTest;
                }
                if (prescription.DifferentialDiagnosis != null && prescription.DifferentialDiagnosis.Count() > 0)
                {
                    prescription.DifferentialDiagnosis = prescription.DifferentialDiagnosis;
                }
                else
                {
                    List<DifferentialDiagnosis> differentialDiagnosis = new List<DifferentialDiagnosis>();
                    differentialDiagnosis.Add(new DifferentialDiagnosis { PrescriptionId = prescription.Id });
                    prescription.DifferentialDiagnosis = differentialDiagnosis;
                }
                if (prescription.TreatmentPlans != null && prescription.TreatmentPlans.Count() > 0)
                {
                    prescription.TreatmentPlans = prescription.TreatmentPlans;
                }
                else
                {
                    List<TreatmentPlan> treatmentPlans = new List<TreatmentPlan>();
                    treatmentPlans.Add(new TreatmentPlan { PrescriptionId = prescription.Id });
                    prescription.TreatmentPlans = treatmentPlans;
                }
            }
            else
            {
                prescription = new Prescription();
                List<OnExamination> onExaminations = new List<OnExamination>();
                onExaminations.Add(new OnExamination { Status = OnExaminationStatusEnum.Extraoral.ToString() });
                onExaminations.Add(new OnExamination { Status = OnExaminationStatusEnum.Intraoral.ToString() });
                prescription.OnExaminations = onExaminations;

                List<DiagnosticTest> diagnosticTest = new List<DiagnosticTest>();
                diagnosticTest.Add(new DiagnosticTest());
                prescription.DiagnosticTest = diagnosticTest;

                List<DifferentialDiagnosis> differentialDiagnosis = new List<DifferentialDiagnosis>();
                differentialDiagnosis.Add(new DifferentialDiagnosis());
                prescription.DifferentialDiagnosis = differentialDiagnosis;

                List<TreatmentPlan> treatmentPlans = new List<TreatmentPlan>();
                treatmentPlans.Add(new TreatmentPlan());
                prescription.TreatmentPlans = treatmentPlans;
            }

            return prescription;
        }

        public Prescription GetPrescriptionDone(int prescriptionId)
        {
            Prescription prescription = GetDetails(prescriptionId);
            if (prescription != null && prescription.Id > 0)
            {
                if (prescription.TreatmentDones != null && prescription.TreatmentDones.Count() > 0)
                {
                    prescription.TreatmentDones = prescription.TreatmentDones;
                }
                else
                {
                    List<TreatmentDone> treatmentDones = new List<TreatmentDone>();
                    treatmentDones.Add(new TreatmentDone { PrescriptionId = prescription.Id });
                    prescription.TreatmentDones = treatmentDones;
                }
                if (prescription.PrescribedMedicines != null && prescription.PrescribedMedicines.Count() > 0)
                {
                    foreach (var item in prescription.PrescribedMedicines)
                    {
                        var medicine = _context.Medicines.Where(m => m.Name == item.MedicineName).FirstOrDefault();
                        if (medicine != null)
                        {
                            item.GenericId = medicine.GenericId ?? 0;
                            item.GenericName = medicine.Generic != null ? medicine.Generic.GenericName : "";
                        }
                    }
                    prescription.PrescribedMedicines = prescription.PrescribedMedicines;
                }
                else
                {
                    List<PrescribedMedicine> prescribedMedicines = new List<PrescribedMedicine>();
                    prescribedMedicines.Add(new PrescribedMedicine { PrescriptionId = prescription.Id });
                    prescription.PrescribedMedicines = prescribedMedicines;
                }
                if (prescription.PrescribedAdvices != null && prescription.PrescribedAdvices.Count() > 0)
                {

                    prescription.PrescribedAdvices = prescription.PrescribedAdvices;
                }
                else
                {
                    List<PrescribedAdvice> prescribedAdvices = new List<PrescribedAdvice>();
                    prescribedAdvices.Add(new PrescribedAdvice { PrescriptionId = prescription.Id });
                    prescription.PrescribedAdvices = prescribedAdvices;
                }
                //var payment = await _paymentService.GetAllByPatientIdAsync(prescription.PatientId);
            }
            else
            {
                prescription = new Prescription();
                List<TreatmentDone> treatmentDones = new List<TreatmentDone>();
                treatmentDones.Add(new TreatmentDone());
                prescription.TreatmentDones = treatmentDones;

                List<PrescribedMedicine> prescribedMedicines = new List<PrescribedMedicine>();
                prescribedMedicines.Add(new PrescribedMedicine());
                prescription.PrescribedMedicines = prescribedMedicines;

                List<PrescribedAdvice> prescribedAdvices = new List<PrescribedAdvice>();
                prescribedAdvices.Add(new PrescribedAdvice());
                prescription.PrescribedAdvices = prescribedAdvices;
            }
            return prescription;
        }

        public IEnumerable<Prescription> GetAllDonePrescriptionByPatientId(int patientId)
        {
            var res = _context.Prescriptions.Where(p => p.PatientId == patientId && p.Status == "C").ToList();
            return res;
        }

        public int GetPrescriptionDoneCountByPatientId(int patientId)
        {
            var res = _context.Prescriptions.Where(p => p.PatientId == patientId && p.Status == "C").Count();
            return res;
        }

        public PagedList<Prescription> GetPrescriptionByPatientId(int patientId, int doctorId, int page, int pageSize)
        {
            var result = _context.Prescriptions.Where(p => p.PatientId == patientId && p.DoctorId == doctorId).OrderByDescending(a => a.Id).ToList();
            return (PagedList<Prescription>)result.ToPagedList(page, pageSize);
        }
        public Prescription GetLastPrescriptions(int patientIntId)
        {
            var lastPrescription = _context.Prescriptions.OrderByDescending(a => a.Id).FirstOrDefault(a => a.PatientId == patientIntId);
            return lastPrescription;
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