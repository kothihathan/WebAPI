using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Interface;

namespace WebApi.Entities
{
    public class MailAttachment : IEntity
    {
        [Key]
        public Guid AttachmentID { get; set; }
        public Guid MailID { get; set; }
        [Column(TypeName = "text")]
        public string SavedPath { get; set; }
        [Column(TypeName = "text")]
        public string Filename { get; set; }
    }
}