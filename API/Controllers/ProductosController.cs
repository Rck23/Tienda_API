

using Core.Entities;
using Core.Intefaces;
using Grpc.Core;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductosController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductosController(IUnitOfWork unitOfWork)
    {
        //EL CONTENEDOR DE REPOSITORIOS ES LA UNIDAD DE TRABAJO
       _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> Get()
    {
        var productos = await _unitOfWork.Productos.GetAllAsync();

        return Ok(productos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var producto = await _unitOfWork.Productos.GetByIdAsync(id);

        return Ok(producto);
    }

    // POST: api/Productos
    [HttpPost]
    public async Task<ActionResult<Producto>> Post(Producto producto)
    {
        _unitOfWork.Productos.Add(producto);
        _unitOfWork.Save();

        if(producto == null)
        {
           return BadRequest();
        }

        return CreatedAtAction(nameof(Post), new {id=producto.Id}, producto);
    }

    //PUT: api/Productos/id
    [HttpPut("{id}")]
    public async Task<ActionResult<Producto>> Put(int id, [FromBody]Producto producto)
    {
        if (producto == null)
        {
            return NotFound();
        }

        _unitOfWork.Productos.Update(producto);
        _unitOfWork.Save();

        return producto;
    }

    //DELETE: api/productos/id 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete (int id)
    {
        var producto = await _unitOfWork.Productos.GetByIdAsync (id);

        if (producto == null)
        {
            return NotFound();
        }

        _unitOfWork.Productos.Remove(producto);
        _unitOfWork.Save();

        return NoContent();
    }
}
