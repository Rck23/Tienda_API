
namespace Core.Intefaces;

public interface IUnitOfWork
{
    IProductoReposity Productos { get; }
    IMarcaReposity Marcas { get; }
    ICategoriaReposity Categorias { get; }
    Task<int> SaveAsync();
}
