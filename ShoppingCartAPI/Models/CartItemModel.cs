using System.ComponentModel.DataAnnotations;

namespace ShoppingCartAPI.Models
{
    public class CartItemModel
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "CartId is required")]
        public long CartId { get; set; }

        [Required(ErrorMessage = "ItemId is required")]
        public long ItemId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 10, ErrorMessage = "Quantity cannot be less than 1 or more than 10")]
        public long Quantity { get; set; }

        public double Price { get; set; }

        public double SubTotalPrice
        {
            get
            {
                return Quantity * Price;
            }
        }

        //used for POST
        public string Token { get; set; }

        public CartItemModel()
        {

        }        
    }
}