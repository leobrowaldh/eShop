

namespace eShop.Data.Shared.Interfaces;

//To use for all entities, so that Id is allways accessible, even when using generics.
public interface IEntity
{
    public int Id { get; set; }
}
