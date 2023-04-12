using Core.Entities;
using Core.Intefaces;
using Infrastructure.Data;


namespace Infrastructure.Repositories;

public class RolRepository : GenericRepository<Rol>, IRolRepository
{
    public RolRepository(TiendaContext context) : base(context)
    {
    }
}

