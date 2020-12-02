using PNG.Daos;
using PNG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PNG.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.Product = ProductDAO.Instance.GetAllForAdmin();
            ViewBag.Category = CategoryDAO.Instance.GetAllForAdmin();
            return View();
        }

        public ActionResult Detail(string id)
        {
            Product p = null;
            if (Session["USER"] == null)
            {
                p = ProductDAO.Instance.GetOneProduct(id);
            }
            else
            {
                Account user = Session["USER"] as Account;
                if(user.RoleID == 2)
                {
                    p = ProductDAO.Instance.GetOneProduct(id);
                }
                else
                {
                    p = ProductDAO.Instance.GetAllForAdmin().Find(product => product.ProductID == id);
                }
            }
            
            if(p == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }

        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            List<Category> list = CategoryDAO.Instance.GetAll();
            Session["CategoryList"] = ToSelectList(list);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Product p)
        {
            if (ModelState.IsValid)
            {
                if (ProductDAO.Instance.AddNewProduct(p))
                {
                    TempData["ADD_Product"] = "Add new product successfully!";
                }
            }
            
            return View();
        }


        [NonAction]
        public SelectList ToSelectList(List<Category> listCategory)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (Category c in listCategory)
            {
                list.Add(new SelectListItem()
                {
                    Text = c.CategoryName.ToString(),
                    Value = c.CategoryID.ToString()
                });
            }
            return new SelectList(list, "Value", "Text"); ;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Update(string id)
        {
            Product p = ProductDAO.Instance.GetAllForAdmin().Find(product => product.ProductID == id);
            var dictionary = new Dictionary<int, string>
                {
                    { 3, "Available" },
                    { 4, "Unavailable" }
                };
            List<Category> list = CategoryDAO.Instance.GetAll();
            Session["CategoryList"] = ToSelectList(list);
            Session["Status"] = new SelectList(dictionary, "Key", "Value");
            return View(p);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(Product p)
        {
            if (ModelState.IsValid)
            {
                if (ProductDAO.Instance.Update(p))
                {
                    TempData["UPDATE_Product"] = "Update product successfully!";
                }
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (ProductDAO.Instance.Delete(id))
            {
                TempData["DELETE_PRODUCT"] = "Delete product successfully!";
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Search(string search, string categoryId = null)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                if (string.IsNullOrEmpty(search))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    search = search.Trim();
                    List<Product> list = ProductDAO.Instance.SearchForAdmin(search);
                    if (list.Count > 0)
                    {
                        ViewBag.Product = list;
                    }
                }
            }
            else
            {
                ViewBag.Product = ProductDAO.Instance.GetAllForAdmin().FindAll(product => product.CategoryID == categoryId);
            }
            ViewBag.Category = CategoryDAO.Instance.GetAllForAdmin();
            return View();
        }
    }
}