using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private static List<Category> Items => new List<Category>
        {
            new() { Id = 1, Name = "Stationary", CreatedOn = DateTime.Now },
            new() { Id = 2, Name = "Books", CreatedOn = DateTime.UtcNow},
        };

        [Authorize(Policy = "Read")]
        public IActionResult Get() => Ok(Items);

        [HttpPost]
        [Authorize(Policy = "Upsert")]
        public IActionResult Post(Category category)
        {
            Items.Add(category);

            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "Upsert")]
        public IActionResult Put(int id, Category category)
        {
            var item = Items.FirstOrDefault(f => f.Id == id);
            if (item != null)
            {
                item = category;
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