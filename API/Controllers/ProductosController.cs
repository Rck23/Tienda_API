using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]

//[ApiVersion("1.0")]
//[ApiVersion("1.1")]
[Authorize(Roles = "Administrador")]
public class ProductosController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductosController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        //EL CONTENEDOR DE REPOSITORIOS ES LA UNIDAD DE TRABAJO
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]

    public async Task<ActionResult<Pager<ProductoListDto>>> Get([FromQuery] Params productParams)
    {
        var resultado = await _unitOfWork.Productos
                                    .GetAllAsync(productParams.PageIndex, productParams.PageSize,
                                    productParams.Search);

        var listaProductosDto = _mapper.Map<List<ProductoListDto>>(resultado.registros);

        Response.Headers.Add("X-InlineCount", resultado.totalRegistros.ToString());

        return new Pager<ProductoListDto>(listaProductosDto, resultado.totalRegistros,
            productParams.PageIndex, productParams.PageSize, productParams.Search);

    }


    [HttpGet]
    //[MapToApiVersion("1.1")] // ESTO INDICA LA VERSION DEL METODO
    public async Task<ActionResult<IEnumerable<ProductoDto>>> Get11()
    {
        var productos = await _unitOfWork.Productos.GetAllAsync();


        return _mapper.Map<List<ProductoDto>>(productos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDto>> Get(int id)
    {
        var producto = await _unitOfWork.Productos.GetByIdAsync(id);

        if (producto == null)
        {
            return NotFound();
        }
        return _mapper.Map<ProductoDto>(producto);
    }

    // POST: api/Productos
    [HttpPost]
    public async Task<ActionResult<Producto>> Post(ProductoAddUpdateDto productoDto)
    {
        // MAPEAR EL PRODUCTO
        var producto = _mapper.Map<Producto>(productoDto);

        _unitOfWork.Productos.Add(producto);
        await _unitOfWork.SaveAsync();

        if (producto == null)
        {
            return BadRequest();
        }

        productoDto.Id = producto.Id;
        return CreatedAtAction(nameof(Post), new { id = productoDto.Id }, productoDto);
    }

    //PUT: api/Productos/id
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductoAddUpdateDto>> Put(int id, [FromBody] ProductoAddUpdateDto productoDto)
    {
        if (productoDto == null)
        {
            return NotFound();
        }

        var producto = _mapper.Map<Producto>(productoDto);

        _unitOfWork.Productos.Update(producto);
        await _unitOfWork.SaveAsync();

        return productoDto;
    }

    //DELETE: api/productos/id 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _unitOfWork.Productos.GetByIdAsync(id);

        if (producto == null)
        {
            return NotFound();
        }

        _unitOfWork.Productos.Remove(producto);
        await _unitOfWork.SaveAsync();

        return NoContent();
    }
}

