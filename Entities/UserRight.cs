using System.ComponentModel.DataAnnotations;
using WebApi.Interface;

namespace WebApi.Entities
{
    public class UserRight : IEntity
    {
        [Key]
        public int UserRightID { get; set; }
        [StringLength(255)]
        public string UserRightDescription { get; set; }
        [StringLength(255)]
        public string PageModule { get; set; }
    }
}