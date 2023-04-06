using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Producto, ProductoDto>()
            .ReverseMap(); // PARA APLICAR VISEVERSAMENTE LOS CAMBIOS

        CreateMap<Marca, MarcaDto>()
            .ReverseMap();

        CreateMap<Categoria, CategoriaDto>()
            .ReverseMap();

        // SE PUEDE REALIZAR ESTE MAPEO GRACIAS A QUE LA CLASE ENTIDAD PRODUCTO
        // TIENE PROPIEDADES DE NAVEGACION HACIA MARCA Y CATEGORIA.
        CreateMap<Producto, ProductoListDto>()
                .ForMember(dest => dest.Marca, origen => origen.MapFrom(origen => origen.Marca.Nombre))
                .ForMember(dest => dest.Categoria, origen => origen.MapFrom(origen => origen.Categoria.Nombre))
                .ReverseMap()
                // ESTO EVITA LA INICIALIZACION DE LAS 2 ENTIDADES para evitar problemas 
                // cuando se guarde un nuevo producto
                .ForMember(origen => origen.Categoria, dest => dest.Ignore())
                .ForMember(origen => origen.Marca, dest => dest.Ignore());

        CreateMap<Producto, ProductoAddUpdateDto>()
                  .ReverseMap()
                  .ForMember(origen => origen.Categoria, dest => dest.Ignore())
                  .ForMember(origen => origen.Marca, dest => dest.Ignore());
    }
}
