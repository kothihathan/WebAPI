using System.ComponentModel.DataAnnotations;
using WebApi.Interface;

namespace WebApi.Entities
{
    public class ModulePage : IEntity
    {
        [Key]
        public int ModulePageID { get; set; }
        [StringLength(255)]
        public string ModulePageCodeName { get; set; }
        [StringLength(255)]
        public string ModulePageTitle { get; set; }
        [StringLength(255)]
        public string ModulePageTranslate { get; set; }
        [StringLength(255)]
        public string PageFile { get; set; }
        [StringLength(255)]
        public string PageAddress { get; set; }
        public int PageOrder { get; set; }
        [StringLength(255)]
        public string Icon { get; set; }
        public int ModuleID { get; set; }
        public bool IsHide { get; set; }
    }
}