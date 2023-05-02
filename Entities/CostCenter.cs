using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Interface;

namespace WebApi.Entities
{
    public class CostCenter : IEntity
    {
        [Key]
        public int CostCenterID { get; set; }
        [StringLength(50)]
        public string CostCenterCode { get; set; }
        [StringLength(50)]
        public string CostCenterDescription { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd H:mm:ss}",
        ApplyFormatInEditMode = true)]
        public DateTime? ValidToDate { get; set; }
    }
}