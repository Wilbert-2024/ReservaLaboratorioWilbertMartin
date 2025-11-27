using ReservaLaboratorioWilbertMartin.Dtos;
using ReservaLaboratorioWilbertMartin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReservaLaboratorioWilbertMartin.Controllers
{
    public class AuthController(IAuthService authService) : Controller
    {
        private readonly IAuthService _authService = authService;

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
      
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            if (!ModelState.IsValid)
                 return BadRequest("Hubo un error al guardar");

            var (success, error) = await _authService.RegisterAsync(dto);

            if (!success)
            {
                //ModelState.AddModelError("", error ?? "Error al registrar.");
                return Conflict(new
                {
                    Message = error
                });
            }
            return Ok(new { success = true });         

        }

        [HttpGet]
        
       public IActionResult Login()
        {
            var token = Request.Cookies["access_token"];
            if (!string.IsNullOrEmpty(token))
            {
                // Aquí podrías validar el token también si deseas
                return RedirectToAction("Index", "Home");
            }
           return View();

        }

        [HttpPost]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { success = false, errorMessage = "Datos invalidos." });

            var (success, result, error) = await _authService.LoginAsync(dto);

            if (!success)
                return Unauthorized(new { success = false, errorMessage = error ?? "Credenciales incorrectas." });

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = result.ExpiresAt

                };

            Response.Cookies.Append("access_token", result.AccesToken, cookieOptions);
            Response.Cookies.Append("refresh_token", result.RefreshToken, cookieOptions);

            return Ok(new
            {
                success = true,
                message = "Login exitoso"

            });


        }

        [Authorize]
         public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return RedirectToAction("Index", "Home");
        }

        // === CONFIRMACION DE CORREO ===

        [HttpGet]

        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return BadRequest("Parametros invalidos.");

            var confirmado = await _authService.ConfirmEmailAsync(email, token);

            if (confirmado)
            {
                TempData["Success"] = " ¡Correo confirmado con exito! Ahora puedes iniciar sesion.";
                return RedirectToAction("Login");
            }

            TempData["Error"] = "El token es invalido o ya expiro.";
            return RedirectToAction("Login");


        }


        // ===== RECUPERAR CONTRASEÑA  =========

        [HttpGet]       
        public IActionResult ForgotPassword() => View(); 

        //Primer paso para hacer la recuperacion por olvidar password
        [HttpPost] 
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto model)
        {

            if (model == null || string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest(new { success = false, errorMessage = "El correo es requerido." });
            }
                        

            var sent = await _authService.SendResetPasswordLinkAsync(model.Email);

            if (sent) {  return Ok(new { success = true }); }

            else  {    return NotFound(new { success = false, errorMessage = "Correo no encontrado." });   }

             

        }

        // ===== RESET CONTRASEÑA
        [HttpGet]
        [AllowAnonymous]

        public IActionResult ResetPassword(string email, string token)
        {
            var model = new ResetPasswordRequestDto { Email = email, Token = token };
            return View(model);

        }

        // SEGUNDO PASO PARA RESETEAR EL PASSWORD

        [HttpPost]
        [AllowAnonymous]


        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var success = await _authService.ResetPasswordAsync(dto);
            if (success)
            {
                return Ok(new { success = true });

            }

            ModelState.AddModelError("", "Token invalido o expirado.");
            return View(dto);

        }


        // === Cambio de contraseña =======
        [Authorize]
        [HttpGet]
       
        public IActionResult ChangePassword() => View();

        [Authorize]
        [HttpPost]
     
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);


            var username = User.Identity?.Name!;
            var success = await _authService.ChangePasswordAsync(username, dto);

            if (success)
            {
                TempData["Success"] = "Contrasena actualizada correctamente.";
                return RedirectToAction("Logout");
            }

            ModelState.AddModelError("", "Contrasena actual incorrecta.");
            return View(dto);

        }


        [HttpPost]
        
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var newTokens = await _authService.RefreshAccessTokenAsync(refreshToken);
            if (newTokens == null)
                return Unauthorized();

            var accessOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = newTokens.ExpiresAt
            };

            var refreshOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)

            };

            Response.Cookies.Append("access_token", newTokens.AccesToken, accessOptions);
            Response.Cookies.Append("refresh_token", newTokens.RefreshToken, refreshOptions);

            return Ok();


        }

        [AllowAnonymous]
   
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

       
    }
}
