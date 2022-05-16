using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Serivces;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class ItemController : Controller
    {
        // GET: Item
        private readonly CartService cartService = new CartService();
        private readonly ItemService itemService = new ItemService();
        public ActionResult Index(int Page = 1)
        {
            ItemViewModel Data = new ItemViewModel();
            Data.Paging = new ForPaging(Page);
            Data.IdList = itemService.GetIdList(Data.Paging);
            foreach (var Id in Data.IdList)
            {
                ItemDetailViewModel newBlock = new ItemDetailViewModel();
                newBlock.Data = itemService.GetDataById(Id);
                string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
                newBlock.InCart = cartService.CheckInCart(Cart, Id);
                Data.ItemBlock.Add(newBlock);
            }
            return View(Data);

        }
        #region 商品頁面
        public ActionResult Item(int Id)
        {
            ItemDetailViewModel ViewData = new ItemDetailViewModel();
            ViewData.Data = itemService.GetDataById(Id);
            string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
            ViewData.InCart = cartService.CheckInCart(Cart, Id);
            return View(ViewData);
        }
        #endregion
        #region 商品列表區塊
        public ActionResult ItemBlock(int Id)
        {
            ItemDetailViewModel ViewData = new ItemDetailViewModel();
            ViewData.Data = itemService.GetDataById(Id);
            string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
            ViewData.InCart = cartService.CheckInCart(Cart, Id);
            return PartialView(ViewData);
        }
        #endregion
        #region 新增商品
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(ItemCreateViewModel Data)
        {
            if (Data.ItemImage != null)
            {
                string filename = Path.GetFileName(Data.ItemImage.FileName);
                string Url = Path.Combine(Server.MapPath("~/Upload/"), filename);
                Data.ItemImage.SaveAs(Url);
                Data.NewData.Image = filename;
                itemService.Insert(Data.NewData);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("ItemImage", "請選擇上傳檔案");
                return View(Data);
            }
        }
        #endregion
        #region 刪除商品
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int Id)
        {
            Item Data = itemService.GetDataById(Id);
            string path = Path.Combine(Server.MapPath("~/Upload/"), Data.Image);
            FileInfo file = new FileInfo(path);
            file.Delete();
            itemService.Delete(Id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}