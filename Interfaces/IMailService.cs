using System.Collections.Generic;
using WebApi.Entities;

namespace WebApi.Interfaces
{
    public interface IMailService
    {
        Mail CreateResendMail(Mail originMail, List<MailAttachment> originMailAttachments);
        dynamic GetFolder(int paramFolderId, int userId);
        dynamic GetLabel(int paramLabelId, int userId);
        dynamic ResendMail(string paramMailId, int userId);
        dynamic GetSentFolder(int paramFolderId, int pageNumber, int rowsOfPage);        
    }
}
