using Autofac.Util;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Win32.SafeHandles;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using WebApi.Helpers;
using WebApi.Interfaces;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ModulePagesController : ControllerBase, IDisposable
    {
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        readonly Disposable _disposable;
        private IModulePageService _modulePageService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private DataContext _context;

        public ModulePagesController(
            IModulePageService modulePageService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _modulePageService = modulePageService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _disposable = new Disposable();
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }

        //[AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            // register the instance so that it is disposed when request ends
            HttpContext.Response.RegisterForDispose(_disposable);
            //var results = _context.ModulePages.FromSqlRaw("SELECT * FROM ModulePages").AsNoTracking().ToList();
            var results = _context.ModulePages.AsNoTracking().ToList();
            return Ok(results);
        }

        //[AllowAnonymous]
        [HttpGet("GetModulePagesByUserId/{userid}")]
        public IActionResult GetModulePagesByUserId(int userid)
        {
            // register the instance so that it is disposed when request ends
            HttpContext.Response.RegisterForDispose(_disposable);
            var result = _modulePageService.GetModulePagesByUserId(userid);

            return Ok(result);
        }
    }
}