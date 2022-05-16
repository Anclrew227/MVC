using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Serivces;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        private readonly CartService cartService = new CartService();
        [Authorize]
        public ActionResult Index()
        {
            CartBuyViewModel Data = new CartBuyViewModel();
            string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
            Data.DataList = cartService.GetItemFromCart(Cart);
            return View(Data);
        }
        #region 放入購物車中
        [Authorize]
        public ActionResult Put(int Id, string toPage)
        {
            string Cart;
            if (HttpContext.Session["Cart"] != null)
            {
                Cart = HttpContext.Session["Cart"].ToString();
            }
            else
            {
                Cart = DateTime.Now.ToString() + User.Identity.Name;
                HttpContext.Session["Cart"] = Cart;
            }
            cartService.SaveCart(User.Identity.Name, Cart, Id);
            if (toPage == "Item")
            {
                return RedirectToAction("Item", "Item", new { Id = Id });
            }
            else if (toPage == "ItemBlock")
            {
                return RedirectToAction("ItemBlock", "Item", new { Id = Id });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region 從購物車中取出
        [Authorize]
        public ActionResult Pop(int Id, string toPage)
        {
            string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
            cartService.SaveCartRemove(Cart, Id);
            if (toPage == "Item")
            {
                return RedirectToAction("Item", "Item", new { Id = Id });
            }
            else if (toPage == "ItemBlock")
            {
                return RedirectToAction("ItemBlock", "Item", new { Id = Id });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        #endregion
    }
}