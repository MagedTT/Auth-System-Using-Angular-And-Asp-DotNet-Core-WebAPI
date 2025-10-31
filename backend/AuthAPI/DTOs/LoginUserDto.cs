using System.ComponentModel.DataAnnotations;

namespace AuthAPI.DTOs;

public class LoginUserDto
{
    [Required(ErrorMessage = "Username is Required.")]
    public string UserName { get; set; } = "";
    [Required(ErrorMessage = "Password is Required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";
}