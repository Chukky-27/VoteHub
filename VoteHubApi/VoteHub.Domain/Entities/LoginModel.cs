using System.ComponentModel.DataAnnotations;

namespace VoteHub.Domain.Entities
{
    public record LoginModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; init; } = string.Empty;
    }
}
