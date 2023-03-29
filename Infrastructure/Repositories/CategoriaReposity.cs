

using Core.Entities;
using Core.Intefaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class CategoriaReposity :  GenericRepository<Categoria>, ICategoriaReposity
{
    public CategoriaReposity(TiendaContext context) : base(context)
    {

    }
}
