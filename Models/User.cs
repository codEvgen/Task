using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test_task.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("UserGroup")]
        public int UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
        [ForeignKey("UserState")]
        public int UserStateId { get; set; }
        public UserState UserState { get; set; }
    }
}