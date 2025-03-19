namespace Domain.ViewModel.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string AvatarUrl { get; set; }
        public required string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public required string LanguageCode { get; set; }
        public required string Theme { get; set; }
    }
}
