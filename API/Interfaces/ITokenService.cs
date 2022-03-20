using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        /* string CreateToken(AppUser user); (L207)  Promena u metodi string --> Task*/
       Task<string> CreateToken(AppUser user);
    }
}