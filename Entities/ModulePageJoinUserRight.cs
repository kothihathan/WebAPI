using System.ComponentModel.DataAnnotations;
using WebApi.Interface;

namespace WebApi.Entities
{
    public class ModulePageJoinUserRight : IEntity
    {
        [Key]
        public int ModulePageJoinUserRightID { get; set; }
        public int ModulePageID { get; set; }
        public int UserRightID { get; set; }
    }
}