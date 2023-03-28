
using Core.Entities;

namespace Core.Intefaces;

public interface IProductoReposity : IGenericRepository<Producto>
{
    Task<IEnumerable<Producto>> GetProductosMasCaros(int cantidad);
}
