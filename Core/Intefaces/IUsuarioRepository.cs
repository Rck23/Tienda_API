using Core.Entities;
namespace Core.Intefaces;

public interface IUsuarioRepository : IGenericRepository<Usuario> { 
    
    Task<Usuario> GetByUsernameAsync(string username);

    Task<Usuario> GetByRefreshTokenAsync(string refreshToken);
}

