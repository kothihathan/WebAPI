using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Interface;

namespace WebApi.Entities
{
    public class Company : IEntity
    {
        [Key]
        public int CompanyID { get; set; }
        [StringLength(4)]
        public string CompanyCode { get; set; }
        [StringLength(255)]
        public string CompanyDescription { get; set; }
    }
}