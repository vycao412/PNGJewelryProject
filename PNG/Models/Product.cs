using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PNG.Models
{
    public class Product
    {
        public string ProductID { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public float Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [Display(Name = "Category")]
        public string CategoryID { get; set; }
        [Display(Name = "Status")]
        public int StatusID { get; set; }

        public Product()
        {
        }

        public Product(string productID, string productName, int quantity, float price)
        {
            ProductID = productID;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }

        public Product(string productID, string productName, int quantity, float price, string description, string image, string categoryID, int statusId)
        {
            ProductID = productID;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
            Description = description;
            Image = image;
            CategoryID = categoryID;
            StatusID = statusId;
        }
    }
}