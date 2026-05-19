  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using translog_APIшка.Model;
  
  namespace translog_APIшка.Controllers;
  
  
  [ApiController]
  [Route("api/[controller]")]
  public class OrderController : ControllerBase
  {
      [HttpGet("GetOrder")]
      public IActionResult GetOrder()
      {
          if (!ModelState.IsValid)
          {
              return BadRequest(ModelState);
          }
          var db = new TransLogCourseContext();
          var orders = db.Orders
              .Include(o => o.Vehicle)
              .ThenInclude(v => v.Drivers)
              .ThenInclude(d => d.User)
              .Include(o => o.User)
              .ToList();
          if (orders.Count() == 0)
          {
              return Unauthorized("Заказов нету");
          }
          else
          {
              return Ok(orders);
          }
      }
  
      [HttpPost("CreateOrder")]
      public IActionResult CreateOrder([FromBody] OrderModel model)
      {
          if (!ModelState.IsValid)
          {
              return BadRequest(ModelState);
          }
          var db = new TransLogCourseContext();
          var newOrder = new Order
          {
              OrderId = model.OrderId,
              ReceivedAt = model.ReceivedAt,
              Status = model.Status,
              Weight = model.Weight,
              VolumeM3 = model.VolumeM3,
              DeparturePoint = model.DeparturePoint,
              ArrivalPoint = model.ArrivalPoint,
              Description = model.Description,
              UserId = model.UserId,
              VehicleId = model.VehicleId,
              DepartureTime = model.DepartureTime,
              ArrivalTime = model.ArrivalTime,
          };
          db.Orders.Add(newOrder);
          db.SaveChanges();
          return Ok(new
          {
              message = "Заказ добавлен!",
              newOrder
          });
      }
  
      [HttpPut("UpdateOrder")]
      public IActionResult UpdateStatus(int OrderId, [FromBody] OrderModel model)
      {
          if (!ModelState.IsValid)
          {
              return BadRequest(ModelState);
          }
          var db = new TransLogCourseContext();
          var order = db.Orders.FromSqlRaw($"select * from orders where order_id = '{OrderId}'").ToList();
          if (order.Count == 0)
          {
              return Unauthorized("Такого заказа нету!");
          }
          else
          {
              foreach (var o in order)
              {
                  o.Status = model.Status;
                  if (model.DepartureTime != null) o.DepartureTime = model.DepartureTime;
                  if (model.ArrivalTime != null) o.ArrivalTime = model.ArrivalTime;
                  if (model.VehicleId != null) o.VehicleId = model.VehicleId;
                  if (model.Distance_km != 0) o.DistanceKm = model.Distance_km;
              }
              db.SaveChanges();
              return Ok(new { message = "Заказ обновлен!", order});
          }
      }
  
      [HttpGet("GetHistory")]
      public IActionResult GetHistory(int Userid)
      {
          if (!ModelState.IsValid)
          {
              return BadRequest(ModelState);
          }
          var db = new TransLogCourseContext();
          var order = db.Orders.FromSqlRaw($"select * from orders where user_id = '{Userid}'").ToList();
          if (order.Count == 0)
          {
              return Unauthorized("Истории нет");
          }
          else
          {
              return Ok(order);
          }
      }
  }
  
  public class OrderModel
  {
      public int OrderId { get; set; }
  
      public DateTime? ReceivedAt { get; set; }
  
      public int UserId { get; set; }
  
      public int? VehicleId { get; set; }
  
      public string Status { get; set; } = null!;
  
      public decimal? Weight { get; set; }
  
      public decimal? VolumeM3 { get; set; }
  
      public string? DeparturePoint { get; set; }
  
      public string? ArrivalPoint { get; set; }
  
      public DateOnly? DepartureTime { get; set; }
  
      public DateOnly? ArrivalTime { get; set; }
  
      public string? Description { get; set; }
      public int Distance_km { get; set; }
  }