using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BLL
{
    public record RegisterDTo(string UserName,
    string Email,
    string Password
    , string? Role);

    public record LoginDto(string UserName, string Password);

    public record TokenDto(string Token, DateTime Expiry);
}
