using System.ComponentModel.DataAnnotations;

namespace AuthAPI.DTOs;

public class RegisterNewUserDto
{
    [Required(ErrorMessage = "FirstName is Required.")]
    public string FirstName { get; set; } = "";
    [Required(ErrorMessage = "LastName is Required.")]
    public string LastName { get; set; } = "";
    [Required(ErrorMessage = "Username is Required.")]
    public string Username { get; set; } = "";
    [Required(ErrorMessage = "Email is Required.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = "";
    [Required(ErrorMessage = "Password is Required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";


}