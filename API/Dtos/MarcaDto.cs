using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace API.Dtos;
public class MarcaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
}



