using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domin.Models.Orders
{
    public class Order:BaseClass<int>
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        //Order properties
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
   
        public OrderState OrderState { get; set; }
        public PaymentType PaymentType { get; set; }
        public List<OrderItem> OrderItems { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string OrderNote { get; set; }



        public string PaymentId { get; set; }
        public string PaymentToken { get; set; }
        public string ConversationId { get; set; }

    }
}
