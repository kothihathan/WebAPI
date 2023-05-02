using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Modules;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class ModulePageService : IModulePageService, IDisposable
    {
        private readonly IUnitOfWork<DataContext> _unitOfWork;
        public ModulePageService(IUnitOfWork<DataContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        #region Moved implementation of Dispose pattern to UnitOfWork Class for improvement code reusability

        //// Public implementation of Dispose pattern callable by consumers.
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        //    GC.SuppressFinalize(this);
        //}

        //// Protected implementation of Dispose pattern.
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposed)
        //        return;

        //    if (disposing)
        //    {
        //        handle.Dispose();
        //        // Free any other managed objects here.
        //        //
        //        _context.Dispose();
        //    }

        //    disposed = true;
        //}
        #endregion

        #region Not Use for UnitOfWork Repository Pattern
        //private void UpdateToDB(IEnumerable<ModulePage> modulePages)
        //{
        //    try
        //    {
        //        _context.ModulePages.UpdateRange(modulePages);
        //        _context.SaveChanges();
        //        return;
        //    }
        //    //XL add to catch Database update Exception
        //    catch (DbUpdateException ex)
        //    {

        //        throw new AppException(ex.InnerException.Message);
        //    }
        //    catch (AppException ex)
        //    {
        //        // return error message if there was an exception
        //        throw new AppException(ex.Message);
        //    }
        //}


        //private void AddtoDB(ModulePage modulePage)
        //{
        //    try
        //    {
        //        _context.ModulePages.Add(modulePage);
        //        return;
        //    }
        //    //XL add to catch Database update Exception
        //    catch (DbUpdateException ex)
        //    {

        //        throw new AppException(ex.InnerException.Message);
        //    }
        //    catch (AppException ex)
        //    {
        //        // return error message if there was an exception
        //        throw new AppException(ex.Message);
        //    }
        //}

        //private void CommittoDB(Object obj)
        //{
        //    try
        //    {
        //        _context.Entry(obj).State = EntityState.Added;
        //        _context.SaveChanges();
        //        return;
        //    }
        //    //XL add to catch Database update Exception
        //    catch (DbUpdateException ex)
        //    {

        //        throw new AppException(ex.InnerException.Message);
        //    }
        //    catch (AppException ex)
        //    {
        //        // return error message if there was an exception
        //        throw new AppException(ex.Message);
        //    }
        //}

        //private void CommittoDB(IEnumerable<ModulePage> modulePages)
        //{
        //    try
        //    {
        //        _context.ModulePages.AddRange(modulePages);
        //        _context.SaveChanges();
        //        return;
        //    }
        //    //XL add to catch Database update Exception
        //    catch (DbUpdateException ex)
        //    {

        //        throw new AppException(ex.InnerException.Message);
        //    }
        //    catch (AppException ex)
        //    {
        //        // return error message if there was an exception
        //        throw new AppException(ex.Message);
        //    }
        //}
        #endregion

        public IEnumerable<ModulePage> GetAll()
        {
            //return _context.Set<ModulePage>().Where(y => y.IsHide == false).AsNoTracking().OrderBy(m => m.ModulePageID);
            return _unitOfWork.GetRepository<ModulePage>().FindByCondition(y => y.IsHide == false).AsNoTracking().OrderBy(m => m.ModulePageID);
        }

        public FuseNavigationModel GetModulePagesByUserId(int userID)
        {
            // Get the list of modulePages associated to the user in a list
            List<int> modulePageAccess = (from a in _unitOfWork.GetRepository<User>().AsQueryable()
                                        join b in _unitOfWork.GetRepository<UserJoinUserRole>().AsQueryable() on a.UserID equals b.UserID
                                        join c in _unitOfWork.GetRepository<UserRoleJoinUserRight>().AsQueryable() on b.UserRoleID equals c.UserRoleID                                        
                                        join d in _unitOfWork.GetRepository<ModulePageJoinUserRight>().AsQueryable() on c.UserRightID equals d.UserRightID
                                        join e in _unitOfWork.GetRepository<ModulePage>().AsQueryable() on d.ModulePageID equals e.ModulePageID
                                        where a.UserID == userID && a.IsLock == false && b.IsLock == false && e.IsHide == false
                                        select e.ModulePageID).Distinct().ToList();
                                        
            FuseNavigationModel result = new FuseNavigationModel
            {
                id = "Applications",
                title = "Applications",
                type = "group",
                icon = "apps",
                children = (from a in _unitOfWork.GetRepository<User>().AsQueryable()
                            join b in _unitOfWork.GetRepository<UserJoinUserRole>().AsQueryable() on a.UserID equals b.UserID
                            join c in _unitOfWork.GetRepository<UserRoleJoinUserRight>().AsQueryable() on b.UserRoleID equals c.UserRoleID
                            join d in _unitOfWork.GetRepository<ModulePageJoinUserRight>().AsQueryable() on c.UserRightID equals d.UserRightID
                            join e in _unitOfWork.GetRepository<ModulePage>().AsQueryable() on d.ModulePageID equals e.ModulePageID
                            join f in _unitOfWork.GetRepository<Module>().AsQueryable() on e.ModuleID equals f.ModuleID
                            where a.UserID == userID && a.IsLock == false && b.IsLock == false && e.IsHide == false && f.IsHide == false
                            orderby f.ModuleOrder ascending
                            select new FuseNavigationDetailModel
                            {
                                id = f.ModuleCodeName,
                                title = f.ModuleTitle,
                                translate = f.ModuleTranslate,
                                type = "group",
                                icon = f.Icon,
                                children = (from j in _unitOfWork.GetRepository<ModulePage>().AsQueryable()
                                            where j.ModuleID == f.ModuleID && j.IsHide == false && modulePageAccess.Contains(j.ModulePageID)
                                            // && f.IsHide == false && a.IsLock == false
                                            select new FuseNavigationDetail2Model
                                            {
                                                id = j.ModulePageCodeName,
                                                title = j.ModulePageTitle,
                                                translate = j.ModulePageTranslate,
                                                type = "item",
                                                icon = j.Icon,
                                                url = j.PageAddress,
                                            }).Distinct().ToList()
                            }).DistinctBy(x => x.id).ToList()
            };
            return result;
        }
    }

}