using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartAPI.Models
{
    public class CartModel
    {
        public long ID { get; set; }

        public string Name { get; set; }
        
        public List<CartItemModel> ItemList { get; set; }
    
        public double TotalPrice
        {
            get
            {
                return ItemList.Sum(i => i.SubTotalPrice);
            }
        }

        public CartModel()
        {

        }        
    }
}