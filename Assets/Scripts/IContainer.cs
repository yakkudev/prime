public interface IContainer
{
    public Item item {get; set;}

    public bool PutItem(Item item);

    public Item TakeItem();


    
}