using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PNG.Models
{
    public class Category
    {
        public string CategoryID { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        [Display(Name = "Status")]
        public int StatusID { get; set; }

        public Category()
        {

        }

        public Category(string name)
        {
            CategoryName = name;
        }

        public Category(string id, string name, int statusID)
        {
            CategoryID = id;
            CategoryName = name;
            StatusID = statusID;
        }
    }
}