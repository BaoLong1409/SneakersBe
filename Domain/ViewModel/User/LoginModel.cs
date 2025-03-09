using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.User
{
    public class LoginModel
    {
        [Required]
        public required string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
