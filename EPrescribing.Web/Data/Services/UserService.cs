using EPrescribing.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EPrescribing.Web.Data.Services
{
    public interface IUserService : IDisposable
    {
        IEnumerable<LoginUser> GetAll();
        int GetCount();
        LoginUser GetDetails(int Id);
        LoginUser GetUserByMobileNo(string mobileNo);
        bool Add(LoginUser model);
        bool AddUserWithDoctor(LoginUser model);
        bool Update(LoginUser model);
        bool Delete(int Id);
    }
    public class UserService : IUserService
    {
        private readonly AppEntities _context;

        public UserService(AppEntities context)
        {
            _context = context;
        }
        public IEnumerable<LoginUser> GetAll()
        {
            return _context.LoginUsers;
        }
        public int GetCount()
        {
            return _context.LoginUsers.Count();
        }
        public LoginUser GetDetails(int Id)
        {
            return _context.LoginUsers.Find(Id);
        }
        public LoginUser GetUserByMobileNo(string mobileNo)
        {
            return _context.LoginUsers.Where(u => u.MobileNo == mobileNo).FirstOrDefault();
        }

        public bool Add(LoginUser model)
        {
            if (model != null)
            {
                try
                {
                    _context.LoginUsers.Add(model);
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
        public bool AddUserWithDoctor(LoginUser model)
        {
            if (model != null)
            {
                using (var _context = new AppEntities())
                {
                    using (DbContextTransaction transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            _context.LoginUsers.Add(model);
                            _context.SaveChanges();

                            Doctor doctor = new Doctor();
                            
                            doctor.IsActive = false;

                            _context.Doctors.Add(doctor);
                            _context.SaveChanges();

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }


            }
            return false;
        }

        public bool Update(LoginUser model)
        {
            if (model != null)
            {
                using (var _context = new AppEntities())
                {
                    using (DbContextTransaction transaction = _context.Database.BeginTransaction())
                    {

                        try
                        {
                            _context.Entry(model).State = EntityState.Modified;
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
                LoginUser obj = _context.LoginUsers.Find(Id);
                _context.LoginUsers.Remove(obj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
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