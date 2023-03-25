

using Core.Entities;
using Grpc.Core;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductosController : BaseApiController
{
    private readonly TiendaContext _context;
    public ProductosController(TiendaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> Get()
    {
        var productos = await _context.Productos.ToListAsync();

        return Ok(productos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        return Ok(producto);
    }
}
