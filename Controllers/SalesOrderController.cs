using ecommerce_v1.Models;
using ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce_v1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesOrderController(SalesOrderService salesOrderService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var salesOrders = await salesOrderService.GetAll();
        return Ok(new { salesOrderItems = salesOrders });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var salesOrder = await salesOrderService.GetById(id);
        if (salesOrder == null) return NotFound();
        return Ok(salesOrder);
    }

    [HttpPost]
    public async Task<IActionResult> Add(SalesOrder salesOrder)
    {
        var salesOrderItemCreated = await salesOrderService.Add(salesOrder);
        return CreatedAtAction(nameof(GetById), new { id = salesOrderItemCreated.SalesOrderId },
            salesOrderItemCreated);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, SalesOrder salesOrder)
    {
        if (id != salesOrder.SalesOrderId) return BadRequest();
        var salesOrderUpdated = await salesOrderService.Update(salesOrder);
        return Ok(salesOrderUpdated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await salesOrderService.Delete(id);
        return NoContent();
    }
}