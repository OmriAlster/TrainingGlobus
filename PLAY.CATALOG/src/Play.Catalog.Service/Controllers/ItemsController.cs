using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Entities;
using Play.Common.MongoDB;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private MongoRepository<Item> ItemsRepository;

        public ItemsController(MongoRepository<Item> itemsRepository){
            this.ItemsRepository = itemsRepository;
        }

        // GET /items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync(){
            var items = (await ItemsRepository.GetAllAsync().ConfigureAwait(false)).Select(item => item.AsDto());
            return items;
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemByIdAsync(Guid id){
            var currItem = await ItemsRepository.GetAsync(id);
            if (currItem == null)
                return NotFound();
            return currItem.AsDto();
        }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> AddItemAsync(CreateItemDto ItemToCreate){
            var newItem = new Item{
                Id = Guid.NewGuid(),
                Name = ItemToCreate.Name,
                Description = ItemToCreate.Description,
                Price = ItemToCreate.Price,
                CreatedTime = DateTimeOffset.UtcNow
                };
            await ItemsRepository.CreateAsync(newItem);
            return CreatedAtAction(nameof(GetItemByIdAsync),new {id = newItem.Id},newItem);
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemAsync(Guid id,UpdateItemDto ItemToUpdate){
            var existingItem = await ItemsRepository.GetAsync(id);

            if (existingItem == null)
                return NotFound();

                existingItem.Name = ItemToUpdate.Name;
                existingItem.Description = ItemToUpdate.Description;
                existingItem.Price = ItemToUpdate.Price;

            await ItemsRepository.UpdateAsync(existingItem);
            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemAsync(Guid id){
            // var existingItem = await ItemsRepository.GetByIdAsync(id);
            // if (existingItem == null)
            //     return NotFound();
            await ItemsRepository.RemoveAsync(id);
            return NoContent();
        }
    }
}