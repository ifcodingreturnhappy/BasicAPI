using DataLayer.Models;
using System;

namespace API.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user, out DateTime? expiryDate);
    }
}