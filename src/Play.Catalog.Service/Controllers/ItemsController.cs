using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Play.Catalog.Service.Dtos;
using System.Linq;
using Play.Catalog.Service.Repositories; 
using System.Threading.Tasks;
using Play.Catalog.Service.Entities;


namespace Play.Catalog.Service.Controllers{

    //https://localhost:5001/items  example 
    [ApiController]
    [Route("items")]
public class ItemsController :ControllerBase{

    private static readonly List<ItemDto> items= new(){
    new ItemDto(Guid.NewGuid(),"Potion","Restore a small amount of HP",5,DateTimeOffset.UtcNow),
    new ItemDto(Guid.NewGuid(),"Antidote","Cures Poison",7,DateTimeOffset.UtcNow),
    new ItemDto(Guid.NewGuid(),"Bronze sword","Deals a small amount of damage",5,DateTimeOffset.UtcNow) 
};

    private readonly ItemsRepository itemsRepository=new();

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetAsync(){

        var item = (await itemsRepository.GetAllAsync())
                            .Select(item=> item.AsDto());

        return item;
    }

    //GET /items/12345
    //GET /items/{id}
    [HttpGet("{id}")]
    public  async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id){
        var item =await itemsRepository.GetAsync(id); 
        if (item==null){
            return NotFound();
        }
        return item.AsDto(); 
    }

     //POST /items
    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto){
        var item = new Item{
            Name =createItemDto.Name,
            Description =createItemDto.Description,
            Price =createItemDto.Price,
            CreatedDate  =DateTimeOffset.UtcNow
        };
 
        await itemsRepository.CreateAsync(item);

        return CreatedAtAction(nameof(GetByIdAsync), new {id = item.Id}, item);
    }

     //PUT /items/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(Guid  id, UpdateItemDto updateItemDto)
    {
        var existingItem= await itemsRepository.GetAsync(id);

        if (existingItem==null){
            return NotFound();
        }

            existingItem.Name=updateItemDto.Name;
            existingItem.Description=updateItemDto.Description;
            existingItem.Price=updateItemDto.Price;

        await itemsRepository.UpdateAsync(existingItem);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid  id){
        var item= await itemsRepository.GetAsync(id);

        if (item==null){
            return NotFound();
        }

        await itemsRepository.RemoveAsync(id);
        
        return NoContent();

    }
}
}