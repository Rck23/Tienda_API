
namespace Core.Intefaces;

public interface IUnitOfWork
{
    IProductoReposity Productos { get; }
    IMarcaReposity Marcas { get; }
    ICategoriaReposity Categorias { get; }
    IRolRepository Roles { get; }
    IUsuarioRepository Usuarios { get; }
    Task<int> SaveAsync();
}
