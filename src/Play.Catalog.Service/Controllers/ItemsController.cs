using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Play.Catalog.Service.Dtos;
using System.Linq;


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
    [HttpGet]
    public IEnumerable<ItemDto> Get(){
        return items;
    }

    //GET /items/12345
    //GET /items/{id}
    [HttpGet("{id}")]
    public ItemDto GetById(Guid id){
        var item = items.Where(item => item.Id == id).SingleOrDefault();
        return item;
    }

    [HttpPost]
    public ActionResult<ItemDto> Post(CreateItemDto createItemDto){
        var item = new ItemDto(Guid.NewGuid(),createItemDto.Name,createItemDto.Description,createItemDto.Price,DateTimeOffset.UtcNow);
        items.Add(item);

        return CreatedAtAction(nameof(GetById), new {id = item.Id}, item);
    }
}
}