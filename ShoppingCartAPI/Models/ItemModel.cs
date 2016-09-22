namespace ShoppingCartAPI.Models
{
    public class ItemModel
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long Quantity { get; set; }

        public double Price { get; set; }

        public ItemModel()
        {

        }        
    }
}