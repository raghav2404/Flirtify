using API.Entities;
using System;
using System.Collections.Generic;

namespace API.Interfaces;


public interface ITokenService
{
    string CreateToken(AppUser user);
}
