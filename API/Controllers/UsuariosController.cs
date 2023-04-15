using API.Dtos;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


public class UsuariosController : BaseApiController
{
    private readonly IUserService _userService;

    public UsuariosController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(RegisterDto model)
    {
        var result = await _userService.RegisterAsync(model);
        return Ok(result);
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(LoginDto model)
    {
        var result = await _userService.GetTokenAsync(model);

        SetRefreshTokenInCookie(result.RefreshToken); // asigna el token en la cookie llamada "refreshToken"

        return Ok(result);
    }

    [HttpPost("addrole")]
    public async Task<IActionResult> AddRoleAsync(AddRoleDto model)
    {
        var result = await _userService.AddRoleAsync(model);

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"]; //ESTO LEE EL TOKEN QUE SE ESTA EN LA COOKIE

        var response = await _userService.RefreshTokenAsync(refreshToken); // OBTENEMOS NUEVO TOKEN

        if (!string.IsNullOrEmpty(response.RefreshToken))
        {
            SetRefreshTokenInCookie(response.RefreshToken); // SI EL TOKEN NO ES VALIDO SE ENVIA EL MENSAJE
        }

        // ESTABLECEMOS EL REFRESH TOKEN Y ENVIAMOS LA RESPUESTA
        return Ok(response);
    }

    // PONER TOKEN EN COOKIE 
    private void SetRefreshTokenInCookie(string refreshToken)
    {

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(10),
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions); 
    }
}
