using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class CostCenterService : ICostCenterService, IDisposable
    {
        private readonly IUnitOfWork<DataContext> _unitOfWork;

        public CostCenterService(IUnitOfWork<DataContext> unitOfWork)
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
        //private void UpdateToDB(IEnumerable<CostCenter> costCenters)
        //{
        //    try
        //    {
        //        _context.CostCenters.UpdateRange(costCenters);
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

        //private void AddtoDB(CostCenter costCenter)
        //{
        //    try
        //    {
        //        _context.CostCenters.Add(costCenter);
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

        //private void CommittoDB(IEnumerable<CostCenter> costCenters)
        //{
        //    try
        //    {
        //        _context.CostCenters.AddRange(costCenters);
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

        public IEnumerable<CostCenter> GetAll()
        {
            return _unitOfWork.GetRepository<CostCenter>().GetAll().OrderBy(cc => cc.CostCenterID);
        }
    }
}