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


    public async Task<IEnumerable<Producto>> GetProductosMasCaros(int cantidad) =>
    
        await _context.Productos
                  .OrderByDescending(p => p.Precio).Take(cantidad).ToListAsync();

    public override async Task<Producto> GetByIdAsync(int id)
    {
        return await _context.Productos
            .Include(p => p.Marca)
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // SOBREESCRIBIENDO EL METODO DE GENERIC REPOSITORY
    public override async Task<IEnumerable<Producto>> GetAllAsync()
    {
        return await _context.Productos
            .Include(u => u.Marca)
            .Include(u => u.Categoria)
            .ToListAsync();
    }
 

}
