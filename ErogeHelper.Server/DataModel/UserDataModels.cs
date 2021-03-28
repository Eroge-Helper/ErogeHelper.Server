using System.ComponentModel.DataAnnotations;

namespace ErogeHelper.Server.DataModel
{
    public class UserRegisterParams
    {
        [Required] 
        public string Username { get; set; } = string.Empty;
    
        [Required] 
        public string Password { get; set; } = string.Empty;
    
        [Required] 
        public string Email { get; set; } = string.Empty;
    }

    public class UserLoginParams
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class UserInfoResult
    {
        public int Id { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HomePage { get; set; } = string.Empty;
        public string ExtraInfo { get; set; } = string.Empty;
    }


    public class UserUpdateParams
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string Language { get; set; } = string.Empty;
        public string ExtraInfo { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string HomePage { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }

    public class UserUpdateEmailParams
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;
    }

    public class UserUpdateSecretParams
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;

        [Required] 
        public string Token { get; set; } = string.Empty;
    }
}