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
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Mail;
using CsvHelper;
using System.Text;
using System.Globalization;
using System.Net.Mime;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Drawing.Printing;
using System.Net.NetworkInformation;
using WebApi.Interfaces;
using WebApi.Models.Messaging;
using System.ServiceModel.Channels;

namespace WebApi.Services
{
    public enum MailLabels : int
    {
        ASSET_LOAN = 0,
        ASSET_VERIFICATION = 1,
        ASSET_SERVICING = 2,
        ASSET_LOST_DAMAGED = 3,
        ASSET_DONATED = 4,
        ASSET_TRANSFER = 5,
        OTHERS = 6
    };

    public class MailService : IMailService, IDisposable
    {        
        private readonly AppSettings _appSettings;
        private ILogger _log;
        private IUnitOfWork<DataContext> _unitOfWork;
        public MailService(IUnitOfWork<DataContext> unitOfWork, IOptions<AppSettings> appSettings, ILogger<MailService> log)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
            _log = log;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        #region Moved implementation of Dispose pattern to UnitOfWork Class for improvement code reusability

        //// Flag: Has Dispose already been called?
        //bool disposed = false;
        //// Instantiate a SafeHandle instance.
        //SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
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

        public Mail CreateResendMail(Mail originMail, List<MailAttachment> originMailAttachments)
        {
            Mail newMail = new Mail();
            List<MailAttachment> newMailAttachments = new List<MailAttachment>();

            newMail._appSettings = originMail._appSettings;
            newMail._log = originMail._log;

            newMail.OriginMailID = originMail.Id;
            newMail.ReceivingUser = originMail.ReceivingUser;
            newMail.SendingUser = originMail.SendingUser;

            newMail.Label = originMail.Label;
            newMail.SentTime = DateTime.Now;
            newMail.SendingUserID = originMail.SendingUserID;
            newMail.ReceivingUserID = originMail.ReceivingUserID;
            newMail.Subject = originMail.Subject;
            newMail.Message = originMail.Message;
            newMail.HasAttachments = originMail.HasAttachments;


            // using var transaction = _context.Database.BeginTransaction();
            // try
            // { 
            //AddtoDB(newMail);
            //CommittoDB(newMail);
            _unitOfWork.GetRepository<Mail>().Add(newMail);
            

            if (newMail.HasAttachments)
            {
                foreach (var a in originMailAttachments)
                {
                    string filePath = a.SavedPath;
                    string fileName = a.Filename;
                    Attachment attachment = new Attachment(filePath);
                    attachment.Name = fileName;
                    newMail.attachments.Add(attachment);

                    MailAttachment mailAttachment = new MailAttachment();
                    mailAttachment.MailID = newMail.Id;
                    mailAttachment.Filename = fileName;
                    mailAttachment.SavedPath = filePath;
                    newMailAttachments.Add(mailAttachment);
                }
            }

            //CommittoDB(newMailAttachments);
            _unitOfWork.GetRepository<MailAttachment>().AddRange(newMailAttachments);
            _unitOfWork.Commit();
            // transaction.Commit();
            // }
            // catch (Exception ex)
            // {
            //     transaction.Rollback();
            // }

            return newMail;
        }

        public dynamic GetFolder(int paramFolderId, int userId)
        {
            var userIdParam = new SqlParameter("@UserID", userId);
            var folderIdParam = new SqlParameter("FolderID", paramFolderId);
            var results = _unitOfWork.GetRepository<MailModel>().FromSqlRaw(@"
                IF @FolderID = 0
                    select m.Id, su.StaffName as SendingStaffName, su.StaffEmail as SendingStaffEmail, ru.StaffName as ReceivingStaffName,
                    ru.StaffEmail as ReceivingStaffEmail, m.Subject, m.Message, m.SentTime, m.SentSuccessToSMTPServer, m.[Read], m.Starred, m.Important, m.HasAttachments, m.[Label], @FolderID as 'Folder'
                    --, m.Folder
                    from Mail m
                    left join Users su on m.SendingUserID = su.UserID
                    left join Users ru on m.ReceivingUserID = ru.UserID
                    where m.SendingUserID = @UserID
                    order by m.SentTime desc

                ELSE
                    select m.Id,
                    su.StaffName as SendingStaffName, su.StaffEmail as SendingStaffEmail, ru.StaffName as ReceivingStaffName, ru.StaffEmail as ReceivingStaffEmail,	m.Subject, m.Message, m.SentTime, m.SentSuccessToSMTPServer, m.[Read],	m.Starred, m.Important,	m.HasAttachments, m.[Label], @FolderID as 'Folder'
                    --, m.Folder
                    from Mail m
                    left join Users su on su.userID = m.SendingUserID
                    left join Users ru on ru.userID = m.ReceivingUserID
                    where m.ReceivingUserID = @UserID
                    order by m.SentTime desc", new[] { userIdParam, folderIdParam }).ToList();

            return results;
        }

        public dynamic GetLabel(int paramLabelId, int userId)
        {
            var userIdParam = new SqlParameter("@UserID", userId);
            var labelIdParam = new SqlParameter("LabelID", paramLabelId);
            var results = _unitOfWork.GetRepository<MailModel>().FromSqlRaw(@"
                select m.Id, su.StaffName as SendingStaffName, su.StaffEmail as SendingStaffEmail, ru.StaffName as ReceivingStaffName, ru.StaffEmail as ReceivingStaffEmail, m.Subject, m.Message, m.SentTime, m.SentSuccessToSMTPServer, m.[Read], m.Starred, m.Important, m.HasAttachments, m.[Label], m.Folder 
                from Mail m
                left join Users su on su.UserID = m.SendingUserID
                left join Users ru on ru.UserID = m.ReceivingUserID
                where (m.SendingUserID = @UserID OR m.ReceivingUserID = @UserID)
                and m.Label = @LabelID
                order by m.SentTime desc", new[] { userIdParam, labelIdParam }).ToList();

            return results;
        }

        public dynamic ResendMail(string paramMailId, int userId)
        {
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                Guid id = new Guid(paramMailId);
                List<MailAttachment> attachments = new List<MailAttachment>();

                var mailFound = _unitOfWork.GetRepository<Mail>().AsQueryable().Include(g => g.SendingUser).Include(g => g.ReceivingUser).Where(g => g.Id == id && g.SendingUserID == userId).First();
                if (mailFound != null)
                {
                    mailFound._appSettings = _appSettings;
                    mailFound._log = _log;

                    if (mailFound.HasAttachments)
                    {
                        //attachments = _context.MailAttachments.Where(x => x.MailID == mailFound.Id).ToList();
                        attachments = _unitOfWork.GetRepository<MailAttachment>().FindByCondition(x => x.MailID == mailFound.Id).ToList();
                    }

                    Mail mailToSend = CreateResendMail(mailFound, attachments);

                    int mailStatus = mailToSend.send();

                    if (mailStatus == 0 || mailStatus == -1)
                    {
                        transaction.Rollback();
                        return new { success = false, message = "Failed to resend email." };
                    }

                    mailToSend.SentSuccessToSMTPServer = true;

                    _unitOfWork.Commit();
                    transaction.Commit();
                    return  new { success = true, message = "Success." };
                }
                else
                {
                    transaction.Rollback();
                    return new { success = false, message = "You are not authorised to resend the email." };
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                // return BadRequest(new { message = ex.Message });
                return new { success = false, message = ex.Message };
            }
        }

        public dynamic GetSentFolder(int paramFolderId, int pageNumber, int rowsOfPage)
        {
            var results = _unitOfWork.GetRepository<Mail>().GetAll();

            int count = results.Count();
            int TotalPages = (int)Math.Ceiling(count / (double)rowsOfPage);
            var itemslist = results.Skip((pageNumber - 1) * rowsOfPage).Take(rowsOfPage).ToList();

            var paginatedResult = new
            {
                totalCount = count,
                items = itemslist,
                pageSize = rowsOfPage,
                currentPage = pageNumber,
                totalPages = TotalPages
            };

            return paginatedResult;
        }

        #region Not Used for Improvements (UnitOfWork Repository Pattern)

        //private void AddtoDB(Mail newMail, DataContext _context)
        //{
        //    try
        //    {
        //        _context.Mail.Add(newMail);
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

        //private void AddtoDB(Mail newMail)
        //{
        //    try
        //    {
        //        _context.Mail.Add(newMail);
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

        //private void CommittoDB(IEnumerable<MailAttachment> newMailAttachments)
        //{
        //    try
        //    {
        //        _context.MailAttachments.AddRange(newMailAttachments);
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

        //private void AddtoDB(MailAttachment newMailAttachment)
        //{
        //    try
        //    {
        //        _context.MailAttachments.Add(newMailAttachment);
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

        //// Zack DB-Refactor
        //private void DbTracking(object obj, DataContext _context)
        //{
        //    try 
        //    {
        //        _context.Entry(obj).State = EntityState.Added;
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

    }
}