using Core.Entities;
using System.Linq.Expressions;

namespace Core.Intefaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int id); //identificador
    Task<IEnumerable<T>> GetAllAsync(); // obtiene todos los recursos 
    IEnumerable<T> Find(Expression<Func<T, bool>> expression); // regresa un conjunto de registros dependiendo de la expresion 

    void Add(T entity); // Agrega un elemento al contexto
    void AddRange(IEnumerable<T> entities); //Agrega una lista de entidades al contexto
    void Remove(T entity); // elimina un elemento al contexto
    void RemoveRange(IEnumerable<T> entities); //elimina una lista de entidades al contexto
    void Update(T entity); //Actualiza el contexto

}
