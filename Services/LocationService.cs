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
    public class LocationService : ILocationService, IDisposable
    {
        private readonly IUnitOfWork<DataContext> _unitOfWork;

        public LocationService(IUnitOfWork<DataContext> unitOfWork)
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
        //private void UpdateToDB(IEnumerable<Location> locations)
        //{
        //    try
        //    {
        //        _context.Locations.UpdateRange(locations);
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


        //private void AddtoDB(Location location)
        //{
        //    try
        //    {
        //        _context.Locations.Add(location);
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

        //private void CommittoDB(IEnumerable<Location> locations)
        //{
        //    try
        //    {
        //        _context.Locations.AddRange(locations);
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

        public IEnumerable<Location> GetAll()
        {
            return _unitOfWork.GetRepository<Location>().GetAll().OrderBy(c => c.LocationID);
        }
    }
}