using PNG.Daos;
using PNG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PNG.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View("ViewCart");
        }

        [Authorize(Roles = "Member, Guest")]
        public ActionResult ViewCart()
        {
            return View();
        }

        [Authorize(Roles = "Member, Guest")]
        public ActionResult Add(string id)
        {
            Cart cart = null;
            if(Session["CART"] == null)
            {
                cart = new Cart();
            }
            else
            {
                cart = Session["CART"] as Cart;
            }
            Product p = ProductDAO.Instance.GetOneProduct(id);

            cart.Add(p);
            Session["CART"] = cart;
            TempData["AddToCart"] = "Add " + p.ProductName + " successfully!";
            return RedirectToAction("Detail", "Product", new { id = id });
        }

        [HttpPost]
        [Authorize(Roles = "Member, Guest")]
        public ActionResult Update(string id, int quantity)
        {
            Cart cart = Session["CART"] as Cart;
            if(cart.Update(id, quantity))
            {
                Session["CART"] = cart;
                TempData["UPDATE_QUANTITY_CART"] = "Update successfully!";
            }
            return RedirectToAction("ViewCart");
        }

        [Authorize(Roles = "Member, Guest")]
        public ActionResult Delete(string id)
        {
            Cart cart = Session["CART"] as Cart;
            if (cart.Delete(id))
            {
                Session["CART"] = cart;
                TempData["DELETE_QUANTITY_CART"] = "Delete successfully!";
            }
            return RedirectToAction("ViewCart");
        }

        [Authorize(Roles = "Member, Guest")]
        public ActionResult CheckoutInfo()
        {
            if(Session["USER"] == null)
            {
                return RedirectToAction("Login", "Home", null);
            }
            else
            {
                Account user = Session["USER"] as Account;
                if(user.RoleID != 1)
                {
                    if(Session["CART"] == null || ((Cart)Session["CART"]).CartDetail == null || ((Cart)Session["CART"]).CartDetail.Count == 0)
                    {
                        return RedirectToAction("ViewCart");
                    }
                    else
                    {
                        Cart cart = Session["CART"] as Cart;
                        List<string> list = ProductDAO.Instance.CheckQuantity(cart);
                        if(list.Count != 0)
                        {
                            TempData["OUT_OF_STOCK"] = list;
                            return RedirectToAction("ViewCart");
                        }
                        return View();
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public ActionResult Checkout(Cart c)
        {
            if (ModelState.IsValid)
            {
                Account user = Session["USER"] as Account;
                Cart cart = Session["CART"] as Cart;
                cart.Name = c.Name;
                cart.Phone = c.Phone;
                cart.Address = c.Address;
                if (CartDAO.Instance.AddNewCart(cart, user))
                {
                    string id = CartDAO.Instance.GetOrderID();
                    cart.CartID = id;
                    CartDAO.Instance.AddOrderDetail(cart);
                    if (ProductDAO.Instance.UpdateQuanity(cart))
                    {
                        TempData["CHECKOUT_SUCCESS"] = "Checkout successfully!";
                        Session["CART"] = null;
                        return RedirectToAction("Index", "Home", null);
                    }
                    return View("CheckoutInfo");
                }
                
            }
            return View("CheckoutInfo");
        }

        [Authorize(Roles = "Member")]
        public ActionResult History()
        {
            Account user = Session["USER"] as Account;
            List<Cart> listHistory = CartDAO.Instance.GetHistory(user);
            return View(listHistory);
        }

    }
}