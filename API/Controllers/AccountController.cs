using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using API.Data;
using API.Entities;
using System.Linq;
using System;
using DataContext = API.Data.DataContext;
using API.DTOs;
using RegisterDTO = API.DTOs.RegisterDTO;
using LoginDTO = API.DTOs.LoginDTO;
using API.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] // api/account/register
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
    {
       if(await UserExists(registerDto.UserName))
        {
            return BadRequest("Username is taken");
        }

        using var hmac = new System.Security.Cryptography.HMACSHA512(); // hashing algorithm - hmac sha-512
        var user  = new AppUser
        {
            UserName = registerDto.UserName.ToLower(),
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();


        return Ok(new UserDTO
        {
            UserName = user.UserName,
            Token = tokenService.CreateToken(user)
        });
    }
    [HttpPost("login")] // api/account/login
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
        if(user == null) return Unauthorized("Invalid username");

        using var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginDto.Password));

        for(int i=0;i<computedHash.Length;i++)
        {
            if(computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid password");
            }
        }
        return Ok(new UserDTO
        {
            UserName = user.UserName,
            Token = tokenService.CreateToken(user)
        });
    }

    private async Task<bool> UserExists(string userName)
    {
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
    }
   
}