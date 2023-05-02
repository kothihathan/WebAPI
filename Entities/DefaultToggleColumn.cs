using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Interface;

namespace WebApi.Entities
{
    public class DefaultToggleColumn : IEntity
    {
        [Key]
        public int DefaultToggleColumnID { get; set; }
        [StringLength(255)]
        public string WebPage { get; set; }
        [StringLength(255)]
        public string SelectedColumn { get; set; }
    }
}