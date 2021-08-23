using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new(){
            new ItemDto(Guid.NewGuid(),"Shoko","Shoko Of Tnuva",5,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(),"Coca Cola","Best drink on the world",7,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(),"Chips","Fresh french fries",20,DateTimeOffset.UtcNow)
        };

        // GET /items
        [HttpGet]
        public IEnumerable<ItemDto> GetItems(){
            return items;
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public ItemDto GetItemById(Guid id){
            return items.SingleOrDefault(item => item.Id == id);
        }

        // POST /items
        [HttpPost]
        public ActionResult<ItemDto> AddItem(CreateItemDto ItemToCreate){
            var newItem =new ItemDto(Guid.NewGuid(),ItemToCreate.Name,ItemToCreate.Description,ItemToCreate.Price,DateTimeOffset.UtcNow);
            items.Add(newItem);
            return CreatedAtAction(nameof(GetItemById),new {id = newItem.Id},newItem);
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateItem(Guid id,UpdateItemDto ItemToUpdate){
            var index = items.FindIndex(item => item.Id == id);

            items[index] = items[index] with {
                Name = ItemToUpdate.Name,
                Description = ItemToUpdate.Description,
                Price = ItemToUpdate.Price,
            };
            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteItem(Guid id){
            var index = items.FindIndex(item => item.Id == id);
            items.RemoveAt(index);
            return NoContent();
        }
    }
}