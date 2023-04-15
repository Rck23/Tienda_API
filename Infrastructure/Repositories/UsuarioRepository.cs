
using Core.Entities;
using Core.Intefaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;


public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(TiendaContext context) : base(context)
    {
    }

    public async Task<Usuario> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Usuarios
              .Include(u => u.Roles) // INCLUYE LOS ROLES
              .Include(u => u.RefreshTokens) // INCLUYE EL TOKEN REFRESH
              .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken)); // BUSCA SI EXISTE EL TOKEN
    }

    public async Task<Usuario> GetByUsernameAsync(string username)
    {
        return await _context.Usuarios
            .Include(u => u.Roles) // INCLUYE LOS ROLES
            .Include(u => u.RefreshTokens) // INCLUYE EL TOKEN REFRESH
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower()); 
    }
}
