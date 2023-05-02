using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Interface;

namespace WebApi.Entities
{
    public class BusinessArea : IEntity
    {
        [Key]
        public int BusinessAreaID { get; set; }
        public int CompanyID { get; set; }
        [StringLength(4)]
        public string BusinessAreaCode { get; set; }
        [StringLength(255)]
        public string BusinessAreaDescription { get; set; }
    }
}