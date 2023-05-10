using System.ComponentModel.DataAnnotations;

namespace Test_task.Models
{
    public class UserState
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        public string Description { get; set; }
    }
}