using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PNG.Models
{
    public class Cart
    {
        public string CartID { get; set; }
        public Account Account { get; set; }
        public float TotalPrice { get; set; }
        public string PaymenType { get; set; }
        public int StatusID { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Name { get; set; }
        public Dictionary<string, Product> CartDetail { get; set; }


        public Cart()
        {
        }
        public Cart(string cartId, Account account, float totalPrice, string payment, int statusId, DateTime orderDate, string phone, string address, string name, Dictionary<string, Product> cartDetail)
        {
            CartID = cartId;
            Account = account;
            TotalPrice = totalPrice;
            PaymenType = payment;
            StatusID = statusId;
            OrderDate = orderDate;
            Phone = phone;
            Address = address;
            Name = name;
            CartDetail = cartDetail;
        }

        public void Add(Product p)
        {
            if(CartDetail == null)
            {
                CartDetail = new Dictionary<string, Product>();
            }
            int quantity = 1;
            if (CartDetail.ContainsKey(p.ProductID))
            {
                TotalPrice -= CartDetail[p.ProductID].Quantity * CartDetail[p.ProductID].Price;
                quantity += CartDetail[p.ProductID].Quantity;
            }
            TotalPrice += quantity * p.Price;
            p.Quantity = quantity;
            CartDetail[p.ProductID] = p;
        }

        public bool Update(string id, int quantity)
        {
            if (CartDetail.ContainsKey(id))
            {
                TotalPrice -= CartDetail[id].Quantity * CartDetail[id].Price;
                CartDetail[id].Quantity = quantity;
                TotalPrice += CartDetail[id].Quantity * CartDetail[id].Price;
                return true;
            }
            return false;
        }

        public bool Delete(string id)
        {
            if (CartDetail.ContainsKey(id))
            {
                TotalPrice -= CartDetail[id].Quantity * CartDetail[id].Price;
                CartDetail.Remove(id);
                return true;
            }
            return false;
        }
    }
}