using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private static List<Product> Items => new List<Product>
        {
            new() { Id = 1, CategoryId = 1, Name = "Pencil", Price = 12.25m },
            new() { Id = 2, CategoryId = 2, Name = "Book", Price = 19.50m },
            new() { Id = 3, CategoryId = 1, Name = "Eraser", Price = 5.0m },
        };

        [Authorize(Policy = "Read")]
        public IActionResult Get() => Ok(Items);

        [HttpPost]
        [Authorize(Policy = "Upsert")]
        public IActionResult Post(Product product)
        {
            Items.Add(product);

            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "Upsert")]
        public IActionResult Put(int id, Product product)
        {
            var item = Items.FirstOrDefault(f => f.Id == id);
            if (item != null)
            {
                item = product;
            }

            return Ok();
        }

        [HttpDelete]
        [Authorize(Policy = "Delete")]
        public IActionResult Delete(int id)
        {
            var item = Items.FirstOrDefault(f => f.Id == id);
            if (item != null)
            {
                Items.Remove(item);
            }

            return Ok();
        }
    }
}