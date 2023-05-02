using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class BusinessAreaService : IBusinessAreaService, IDisposable
    {
        private readonly IUnitOfWork<DataContext> _unitOfWork;

        public BusinessAreaService(IUnitOfWork<DataContext> unitOfWork)
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

        #region Not Used for Improvements (UnitOfWork Repository Pattern)
        //private void UpdateToDB(IEnumerable<BusinessArea> businessAreas)
        //{
        //    try
        //    {
        //        _context.BusinessAreas.UpdateRange(businessAreas);
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
        //private void AddtoDB(BusinessArea businessArea)
        //{
        //    try
        //    {
        //        _context.BusinessAreas.Add(businessArea);
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
        //private void CommittoDB(IEnumerable<BusinessArea> businessAreas)
        //{
        //    try
        //    {
        //        _context.BusinessAreas.AddRange(businessAreas);
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

        public IEnumerable<BusinessArea> GetAll()
        {
            return _unitOfWork.GetRepository<BusinessArea>().GetAll().OrderBy(c => c.BusinessAreaID);
        }
    }
}