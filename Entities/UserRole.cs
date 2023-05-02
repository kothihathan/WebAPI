using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Interface;

namespace WebApi.Entities
{
    public class UserRole : IEntity
    {
        [Key]
        public int UserRoleID { get; set; }
        [StringLength(255)]
        public string UserRoleDescription { get; set; }
    }
  
}