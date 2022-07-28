using System;

namespace Play.Catalog.Service.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        decimal Price { get; set; }
        DateTimeOffset CreatedDate { get; set; }
    }
}

