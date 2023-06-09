using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Interface;

namespace WebApi.Entities
{
    // Use by Reports module only
    public class UserSavedToggleColumn : IEntity
    {
        [Key]
        public int UserSavedToggleColumnID { get; set; }
        public int UserID { get; set; }
        [StringLength(255)]
        public string WebPage { get; set; }
        [StringLength(255)]
        public string SelectedColumn { get; set; }
    }
}