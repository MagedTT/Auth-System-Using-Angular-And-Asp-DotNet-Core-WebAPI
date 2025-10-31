using AuthAPI.Data;
using AuthAPI.DTOs;
using AuthAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UsersController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Authenticate([FromBody] LoginUserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            // var errors = ModelState.Values.SelectMany(x => x.Errors)
            //     .Select(x => x.ErrorMessage).ToList();
            return BadRequest(ModelState);
        }
        if (userDto is null)
            return BadRequest();

        var userFromDB = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userDto.UserName && x.Password == userDto.Password);

        if (userFromDB is null)
            return NotFound(new { Message = "User Not Found" });

        return Ok(new { Message = "Successed" });
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterNewUserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (userDto is null)
            return BadRequest();

        var user = _mapper.Map<User>(userDto);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Successed" });
    }
}