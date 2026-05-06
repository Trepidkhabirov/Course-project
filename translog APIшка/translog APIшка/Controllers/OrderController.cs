using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using translog_APIшка.Models;

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
        var db = new TranslogContext();
        var orders = db.Orders.ToList();
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
    public IActionResult CreateOrder(OrderModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var db = new TranslogContext();
        var newOrder = new Order
        {
            ReceivedAt = model.ReceivedAt,
            Status = model.Status,
            Weight = model.Weight,
            VolumeM3 = model.VolumeM3,
            Phone = model.Phone,
            DeparturePoint = model.DeparturePoint,
            ArrivalPoint = model.ArrivalPoint,
            FullName = model.FullName,
            Description = model.Description,
            UserId = model.UserId
        };
        db.Orders.Add(newOrder);
        db.SaveChanges();
        return Ok(new
        {
            message = "Заказ добавлен!",
            newOrder
        });
    }

    [HttpPut("UpdateStaus")]
    public IActionResult UpdateStatus(int OrderId, OrderModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var db = new TranslogContext();
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
            }
            db.SaveChanges();
            return Ok(new { message = "Статус заказа изменен!", order});
        }
    }

    [HttpGet("GetHistory")]
    public IActionResult GetHistory(int Userid)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var db = new TranslogContext();
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
    public DateTime ReceivedAt { get; set; }

    public string Status { get; set; } = null!;

    public decimal Weight { get; set; }

    public decimal VolumeM3 { get; set; }

    public string Phone { get; set; } = null!;

    public string DeparturePoint { get; set; } = null!;

    public string ArrivalPoint { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Description { get; set; }

    public int? UserId { get; set; }

}