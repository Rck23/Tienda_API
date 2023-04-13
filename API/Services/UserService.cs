using API.Dtos;
using API.Helpers;
using Core.Entities;
using Core.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
namespace API.Services;

public class UserService : IUserService
{
    private readonly JWT _jwt;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt,
        IPasswordHasher<Usuario> passwordHasher)
    {
        _jwt = jwt.Value;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        // Crear usuario
        var usuario = new Usuario
        {
            Nombres = registerDto.Nombres,
            ApellidoMaterno = registerDto.ApellidoMaterno,
            ApellidoPaterno = registerDto.ApellidoPaterno,
            Email = registerDto.Email,
            Username = registerDto.Username

            // EJEMPLO DE NUEVO REGISTRO
            //{ 
            //    "nombres":"Ulises",
	           // "apellidoPaterno":"Martínez",
	           // "apellidoMaterno":"Yo",
	           // "email":"miCorreo@gmail.com",
	           // "username":"ulises23",
	           // "password":"123456"
            //}
         };

        // ENCRIPTAMOS EL PASSWORD
        usuario.Password = _passwordHasher.HashPassword(usuario, registerDto.Password);

        var usuarioExiste = _unitOfWork.Usuarios
                                    .Find(u => u.Username.ToLower() == registerDto.Username.ToLower())
                                    .FirstOrDefault();

        // VALIDACIONES
        if (usuarioExiste == null)
        {
            var rolPredeterminado = _unitOfWork.Roles
                                    .Find(u => u.Nombre == Autorizacion.rol_predeterminado.ToString())
                                    .First();
            try
            {
                usuario.Roles.Add(rolPredeterminado);
                _unitOfWork.Usuarios.Add(usuario);
                await _unitOfWork.SaveAsync();

                return $"El usuario  {registerDto.Username} ha sido registrado exitosamente";
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return $"Error: {message}";
            }
        }
        else
        {
            return $"El usuario con {registerDto.Username} ya se encuentra registrado.";
        }
    }

}

