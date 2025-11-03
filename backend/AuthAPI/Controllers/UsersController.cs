using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using AuthAPI.Data;
using AuthAPI.DTOs;
using AuthAPI.Helpers;
using AuthAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AuthAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UsersController(
        IMapper mapper,
        AppDbContext context,
        IConfiguration configuration
        )
    {
        _mapper = mapper;
        _context = context;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (userDto is null)
            return BadRequest();

        var userFromDB = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userDto.UserName);

        if (userFromDB is null)
            return NotFound(new { Message = "User Not Found." });

        if (!PasswordHasher.VerifyPassword(userDto.Password, userFromDB.Password))
            return BadRequest(new { Message = "Incorret Password." });

        userFromDB.Token = CreateJwtToken(userFromDB);

        return Ok(new
        {
            Token = userFromDB.Token,
            Message = "Successed"
        });
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterNewUserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (userDto is null)
            return BadRequest();

        if (await UsernameExistsAsync(userDto.Username))
            return BadRequest(new { Message = "Username exists" });

        // check Email
        if (await EmailExistsAsync(userDto.Email))
            return BadRequest(new { Message = "Email exists" });

        // check Password Strength
        var passwordNotValid = CheckPasswordStrength(userDto.Password);

        if (!string.IsNullOrEmpty(passwordNotValid))
            return BadRequest(new { Message = passwordNotValid });

        var user = _mapper.Map<User>(userDto);

        user.Role = "User";
        user.Token = "";

        user.Password = PasswordHasher.HashPassword(userDto.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Successed" });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
        => Ok(await _context.Users.ToListAsync());

    private async Task<bool> UsernameExistsAsync(string userName)
    {
        return await _context.Users.AnyAsync(x => x.UserName == userName);
    }

    private async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email);
    }

    private string CheckPasswordStrength(string password)
    {
        StringBuilder sb = new();

        if (password.Length < 8)
            sb.Append("Password should consist of at least 8 Characters." + Environment.NewLine);

        if (!Regex.IsMatch(password, "[a-z]"))
            sb.Append("Password should contain Lowercase Characters." + Environment.NewLine);
        if (!Regex.IsMatch(password, "[A-Z]"))
            sb.Append("Password Contains Uppercase Characters." + Environment.NewLine);
        if (!Regex.IsMatch(password, "[0-9]"))
            sb.Append("Password should have numbers [0-9]." + Environment.NewLine);

        if (!Regex.IsMatch(password, "[!,@,#,$,%,^,&,*,(,),_,+,=]"))
            sb.Append("Password should have special characters." + Environment.NewLine);

        return sb.ToString();
    }

    private string CreateJwtToken(User user)
    {
        var signingKey = Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]!);
        ClaimsIdentity claimsIdentity = new(new Claim[]
        {
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        });

        SigningCredentials credentials = new(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            SigningCredentials = credentials,
            Expires = DateTime.Now.AddDays(1)
        };

        JwtSecurityTokenHandler tokenHandler = new();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);
        return accessToken;
    }
}