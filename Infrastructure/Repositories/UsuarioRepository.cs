
using Core.Entities;
using Core.Intefaces;
using Infrastructure.Data;
namespace Infrastructure.Repositories;


public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(TiendaContext context) : base(context)
    {
    }
}
