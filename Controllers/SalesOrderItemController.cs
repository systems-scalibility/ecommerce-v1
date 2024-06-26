using System.Linq.Expressions;
using ecommerce_v1.Models;
using ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce_v1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesOrderItemController(SalesOrderItemService salesOrderItemService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var salesOrderItems = await salesOrderItemService.GetAll();
        return Ok(new { salesOrderItems });
    }

    [HttpGet("product/{codeNumber}")]
    public async Task<IActionResult> GetAllByCodeNumber(string codeNumber)
    {
        var salesOrderItems = await salesOrderItemService.GetAllByCodeNumber(codeNumber);
        return Ok(new { salesOrderItems });
    }

    [HttpGet("range-dates")]
    public async Task<IActionResult> GetAllRangeDates(DateTime startDate, DateTime endDate)
    {
        var salesOrderItems = await salesOrderItemService.GetAllRangeDates(startDate, endDate);
        return Ok(new { salesOrderItems });
    }

    [HttpGet("quantity/{quantity:int}")]
    public async Task<IActionResult> GetAllByQuantity(int quantity, string? condition = "=")
    {
        Expression<Func<SalesOrderItem, bool>> expression = condition switch
        {
            "=" => x => x.Quantity == quantity,
            ">" => x => x.Quantity > quantity,
            "<" => x => x.Quantity < quantity,
            _ => throw new InvalidOperationException("Invalid condition")
        };
        var salesOrderItems = await salesOrderItemService.GetAllByQuantity(quantity, expression);
        var orderItems = salesOrderItems.ToList();
        return Ok(new { count = orderItems.Count, salesOrderItems = orderItems });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var salesOrderItem = await salesOrderItemService.GetById(id);
        if (salesOrderItem == null) return NotFound();
        return Ok(salesOrderItem);
    }

    [HttpPost]
    public async Task<IActionResult> Add(SalesOrderItem salesOrderItem)
    {
        var salesOrderItemCreated = await salesOrderItemService.Add(salesOrderItem);
        return CreatedAtAction(nameof(GetById), new { id = salesOrderItemCreated.SalesOrderItemId },
            salesOrderItemCreated);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, SalesOrderItem salesOrderItem)
    {
        if (id != salesOrderItem.SalesOrderItemId) return BadRequest();
        var salesOrderItemUpdated = await salesOrderItemService.Update(salesOrderItem);
        return Ok(salesOrderItemUpdated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await salesOrderItemService.Delete(id);
        return NoContent();
    }
}