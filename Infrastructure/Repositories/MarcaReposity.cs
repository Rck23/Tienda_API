
using Core.Entities;
using Core.Intefaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class MarcaReposity : GenericRepository<Marca>, IMarcaReposity
{
    public MarcaReposity(TiendaContext context) : base(context)
    {

    }
}
