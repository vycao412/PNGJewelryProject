using PNG.Daos;
using PNG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PNG.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<Category> list = CategoryDAO.Instance.GetAllForAdmin();
            return View(list);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Category c)
        {
            if(CategoryDAO.Instance.AddNewCategory(c)){
                TempData["ADD_CATEGORY"] = "Add new category successfully!";
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            Category c = CategoryDAO.Instance.GetOneCategory(id);
            var dictionary = new Dictionary<int, string>
                {
                    { 3, "Available" },
                    { 4, "Unavailable" }
                };

            Session["CATEGORY"] = new SelectList(dictionary, "Key", "Value");
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Category c)
        {
            if (ModelState.IsValid)
            {
                if (CategoryDAO.Instance.Update(c))
                {
                    TempData["UPDATE_CATEGORY"] = "Update successfully!";
                }
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (CategoryDAO.Instance.Delete(id))
            {
                TempData["DELETE_CATEGORY"] = "Delete successfully!";
            }
            return RedirectToAction("Index");
        }

    }
}