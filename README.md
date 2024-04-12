# Proyecto de Login en MVC
Este proyecto consiste en un sistema de login desarrollado en ASP.NET Core MVC que cumple con los siguientes requisitos:

Autenticación obligatoria: Los usuarios deben iniciar sesión para acceder a las secciones protegidas de la aplicación. No se puede acceder a estas secciones sin estar autenticado.

Basado en SQL Server: La aplicación utiliza SQL Server como base de datos para almacenar la información de los usuarios y gestionar la autenticación.

## Sección de Diseño de Ingeniería
En esta sección se presenta el diseño de ingeniería del sistema de login desarrollado en ASP.NET Core MVC. Aquí se incluye un diagrama que representa la arquitectura y el flujo de información del sistema, así como una explicación escrita detallada sobre los componentes y la funcionalidad del login.

El diagrama proporciona una visión general de cómo interactúan las diferentes partes del sistema, incluyendo el controlador de cuentas, las vistas, la base de datos y el proceso de autenticación. Además, la explicación escrita describe cada elemento del diagrama y cómo se relacionan entre sí para lograr la funcionalidad deseada.

Esta sección de diseño de ingeniería es fundamental para comprender la estructura y el funcionamiento del sistema de login, lo que facilita su comprensión y mantenimiento por parte de los desarrolladores.

# Diagrama de Arquitectura del Sistema de Login:

![DiagramaLogin drawio](https://github.com/Ji07y/Login/assets/85076732/a711c50d-8fd7-4e0a-8a78-ab470f84fd7f)


* Cliente: Representa el navegador web utilizado por los usuarios para interactuar con el sistema.

* Controlador: Contiene los controladores que manejan las solicitudes entrantes y realizan las acciones correspondientes. En este caso, el AccountController maneja las funciones relacionadas con el login, mientras que el HomeController maneja las funciones relacionadas con las otras páginas del sitio web.

* Vistas: Contiene las vistas HTML que se muestran al usuario. En este caso, la vista Login.cshtml representa la página de inicio de sesión, Index.cshtml representa la página principal y Privacy.cshtml representa la página de privacidad.

* Base de Datos: Representa la base de datos utilizada para almacenar la información de los usuarios. En este caso, hay una tabla llamada Usuarios que almacena la información de los usuarios registrados, como el correo electrónico y la contraseña.

Este diagrama ilustra la estructura básica del sistema de login en ASP.NET Core MVC y cómo interactúan sus diferentes componentes para proporcionar la funcionalidad requerida.

![DiagramaLoginMVC drawio](https://github.com/Ji07y/Login/assets/85076732/3ed56e1d-d53f-46da-9e22-7a8f828a85e9)



## Código del Controlador de Cuenta (AccountController)
El AccountController es responsable de manejar las acciones relacionadas con el login, el logout y la validación de credenciales de usuario. Aquí se encuentra la lógica para autenticar a los usuarios y redirigirlos a las vistas correspondientes.

namespace ProyectCRUD.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login(string? url= null)
        {
            ViewBag.Url = url;
            return View(new UsuarioModel.Login());
        }


        [HttpPost]
        public async Task<IActionResult> Login(UsuarioModel.Login model, string? url = null)
        {
            Validar(model);
            if(!ModelState.IsValid)
            {
                ViewBag.Url = url;
                return View(model);
            }
            if(!model.Correo!.Equals("prueba@mail.com")|| !model.Clave!.Equals("123"))
            {
                
                ModelState.AddModelError("_", "La clave y correo son incorrectos.");
                ViewBag.Url = url;
                return View(model);
            }
            

            Claim[] datos = new Claim[] { new Claim(ClaimTypes.Name, model.Correo!) };
            ClaimsIdentity identidad = new(datos, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties propiedades = new()
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = !model.MantenerSesion? DateTimeOffset.UtcNow.AddMinutes(5):DateTimeOffset.UtcNow.AddDays(1),
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad), propiedades);
            return Redirect(url ?? "/");
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        
        [NonAction]
        private void Validar(UsuarioModel.Login entidad)
        {
            if (string.IsNullOrEmpty(entidad.Correo)) ModelState.AddModelError("Correo", " Es necesario un correo");
            if (string.IsNullOrEmpty(entidad.Clave)) ModelState.AddModelError("Clave", " Es necesario un clave");
           
        }
    }
}
## Código del Controlador de Inicio (HomeController)
El HomeController es responsable de manejar las acciones relacionadas con las vistas principales de la aplicación una vez que el usuario ha iniciado sesión. Este controlador está protegido con el atributo [Authorize], lo que significa que solo los usuarios autenticados pueden acceder a sus acciones.

using Microsoft.AspNetCore.Mvc;
using ProyectCRUD.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ProyectCRUD.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
## Vista de Login (login.cshtml)
La vista de login login.cshtml contiene el formulario donde los usuarios pueden ingresar sus credenciales de correo electrónico y contraseña para iniciar sesión en la aplicación. También incluye opciones para mantener la sesión iniciada y mensajes de validación en caso de que se ingresen datos incorrectos.
 
     

    <label>Correo:</label>
    <input type="email" asp-for="Correo"/>
    <span asp-validation-for="Correo" style="color:red;"></span>
    <br />

    <label>Clave:</label>
    <input type="password" asp-for="Clave" />
    <span asp-validation-for="Clave" style="color:red;"></span>
    <br />

    <label>
        <input type="checkbox" asp-for="MantenerSesion" />
        MantenerSesion
 
    </label>
    <br />
    <div asp-validation-summary="All"></div>
    <button type="submit">Ingresar</button>
    
</form>

Este proyecto garantiza que los usuarios solo pueden acceder a las secciones autenticadas después de iniciar sesión y que sus credenciales se validan correctamente antes de permitir el acceso.
