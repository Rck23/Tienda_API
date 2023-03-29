
using Core.Intefaces;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork;

public class UnitOfWork: IUnitOfWork, IDisposable
{
    private readonly TiendaContext _context;
    private  IProductoReposity _productos;
    private  IMarcaReposity _marcas;
    private  ICategoriaReposity _categorias;

    public UnitOfWork(TiendaContext context)
    {
       _context = context;
    }

    //CARGA DE LOS REPOSITORIOS DE FORMA RETARDADA (Solo cuando el usuario acceda aparecera el repositorio)
    public ICategoriaReposity Categorias
    {
        get
        {
            if (_categorias == null){
                _categorias = new CategoriaReposity(_context);
            }
            return _categorias;
        }
    }

    public IMarcaReposity Marcas
    {
        get
        {
            if (_marcas == null)
            {
                _marcas = new MarcaReposity(_context);
            }
            return _marcas;
        }
    }

    public IProductoReposity Productos
    {
        get
        {
            if (_productos == null)
            {
                _productos = new ProductoRepository(_context);
            }
            return _productos;
        }
    }

    // GUARDA LOS CAMBIOS EN EL CONTEXTO
    public int Save()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
