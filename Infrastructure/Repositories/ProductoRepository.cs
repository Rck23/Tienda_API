using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Core.Intefaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ProductoRepository : GenericRepository<Producto>, IProductoReposity
{
    public ProductoRepository(TiendaContext context): base(context)
    {

    }


    public async Task<IEnumerable<Producto>> GetProductoMasCaros(int cantidad) =>
    
        await _context.Productos
                  .OrderByDescending(p => p.Precio).Take(cantidad).ToListAsync();
    
}
