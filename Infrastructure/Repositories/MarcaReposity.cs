
using Core.Intefaces;
using Core.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class MarcaReposity : GenericRepository<Marca>, IMarcaReposity
{
    public MarcaReposity(TiendaContext context): base(context)
    {
        
    }
}
