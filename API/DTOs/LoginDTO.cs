using System;
using System.ComponentModel.DataAnnotations;
namespace API.DTOs;

public class LoginDTO
{

    public required string UserName { get; set; }

    public required string Password { get; set; }
}
