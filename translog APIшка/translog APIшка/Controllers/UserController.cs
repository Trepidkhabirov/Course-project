 using Microsoft.AspNetCore.Mvc;
 using Microsoft.EntityFrameworkCore;
 using translog_APIшка.Model;
 
 namespace translog_APIшка.Controllers;
 
 [ApiController]
 [Route("api/[controller]")]
 public class UserController : ControllerBase
 {
     [HttpGet("login")]
     public IActionResult Login(string login, string password)
     {
         var db = new TransLogCourseContext();
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
                     userId = user.UserId,
                     login = user.Username,
                     password = user.Password,
                     roleId = user.RoleId,
                    fullname = user.FullName,
                    numberhone =  user.Numberphone,
                    isActive = user.IsActive
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
         var db = new TransLogCourseContext();
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
             FullName = model.fullname,
             Numberphone = model.numberphone,
             IsActive =  model.isActive
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
                 fullname = newUser.FullName,
                 numberphone = newUser.Numberphone,
                 isActive = newUser.IsActive
             }
         );
     }
     [HttpGet("GetUser")]
     public IActionResult GetUser()
     {
         var db = new TransLogCourseContext();
         if (!ModelState.IsValid)
             return BadRequest(ModelState);
         var users = db.Users.ToList();
         if (users.Count() == 0)
         {
             return BadRequest(new { message = "пользователей нету!" });
         }
         else
         {
                 return Ok(users);
         }
         return Unauthorized(new { message = "Ошибка" });
     }
 
     [HttpDelete("DeleteUser")]
     public IActionResult DeleteUser(string login)
     {
         var db = new TransLogCourseContext();
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
     public string fullname { get; set; }
     public string numberphone { get; set; }
     public int isActive  { get; set; }
 }
 
