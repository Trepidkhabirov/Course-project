using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using translog_APIшка.Models;

namespace translog_APIшка.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login(string login, string password)
    {
        var db = new TranslogContext();
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var users = db.Users.FromSqlRaw(
            $"select * from users where username = '{login}' and password = '{password}'").ToList();
        if (users.Count() == 0)
        {
            return Unauthorized(new { message = "Неверный логин или пароль!" });
        }
        else
        {
            foreach (var user in users)
            {

                return Ok(new
                {
                    message = "Вход выполнен",
                    login = user.Username,
                    password = user.Password,
                    roleId = user.RoleId
                });
            }
        }

        return Unauthorized(new { message = "Ошибка авторизации" });
    }
    
    [HttpPost("register")]
    public IActionResult Register(RegisterModel model)
    {
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var db = new TranslogContext();
        var users = db.Users.FromSqlRaw($"select * from users where username = '{model.Username}'").ToList();
        if (users.Count() != 0)
        {
            return BadRequest(new { message = "Такой логин уже есть!" });
        }

        var newUser = new User
        {
            
            Username = model.Username,
            Password =  model.Password,
            RoleId = model.RoleId ?? 0,
            IsActive = 1
        };
        db.Users.Add(newUser);
        db.SaveChanges();
        return Ok(new
            {
                message = "Регистрация прошла успешно!",
                userId = newUser.UserId,
                login = newUser.Username,
                password = newUser.Password,
                roleId = newUser.RoleId,
                Active = newUser.IsActive
            }
        );
    }
    [HttpGet("GetUser")]
    public IActionResult GetUser(string login)
    {
        var db = new TranslogContext();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var users = db.Users.FromSqlRaw($"select * from users where username = '{login}'").ToList();
        if (users.Count() == 0)
        {
            return BadRequest(new { message = "Такого пользователя нету!" });
        }
        else
        {
            foreach (var user in users)
            {
                return Ok(new
                {
                    message = "Пользователь",
                    userId = user.UserId,
                    login = user.Username,
                    roleId = user.RoleId,
                    Active = user.IsActive
                });
            }
        }
        return Unauthorized(new { message = "Ошибка" });
    }

    [HttpDelete("DeleteUser")]
    public IActionResult DeleteUser(string login)
    {
        var db = new TranslogContext();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var users = db.Users.FromSqlRaw($"select * from users where username = '{login}'").ToList();
        if (users.Count() == 0)
        {
            return BadRequest(new { message = "Такого пользователя нету!" });
        }
        else
        {
            foreach (var user in users)
            {
                user.IsActive = 0;
            }
            db.SaveChanges();
            return Ok(new
            {
                message = "Пользователь удален!"
            });
        }
    }
}
public class RegisterModel
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int? RoleId { get; set; }
}

