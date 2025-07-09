using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using API.Data;
using API.Entities;
using System.Linq;
using System;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using AllowAnonymousAttribute = Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute;

namespace API.Controllers;

[Authorize]
public class UserController : BaseApiController
{
    
    private readonly DataContext _context;

    public UserController(DataContext context)
    {
        _context = context;
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
       var users = await _context.Users.ToListAsync();
       return Ok(users);
    }
    [AllowAnonymous]
    [HttpGet("{id:int}")] // api/user/{id}
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }


}
