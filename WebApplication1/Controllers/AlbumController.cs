using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Serivces;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class AlbumController : Controller
    {
        private readonly AlbumDBService albumService = new AlbumDBService();
        // GET: Album
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult List(int Page = 1)
        {
            AlbumViewModel Data = new AlbumViewModel();
            Data.Paging = new ForPaging(Page);
            Data.FileList = albumService.GetDataList(Data.Paging);
            return PartialView(Data);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return PartialView();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create([Bind(Include = "upload")]AlbumViewModel Data)
        {
            if (Data.upload != null)
            {
                string Url = Path.Combine(Server.MapPath("~/Upload/"), Data.upload.FileName);
                Data.upload.SaveAs(Url);
                albumService.UploadFile(Data.upload.FileName, Url, Data.upload.ContentLength, Data.upload.ContentType, User.Identity.Name);
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Show(int Alb_Id)
        {
            AlbumViewModel Data = new AlbumViewModel();
            Data.File = albumService.GetAlbumById(Alb_Id);
            if (Data.File != null)
            {
                UrlHelper urlHelper = new UrlHelper(Request.RequestContext);
                return Content(urlHelper.Content("~/Upload/" + Data.File.FileName));
            }
            else
            {
                return null;
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DownloadFile(int Alb_Id)
        {
            AlbumViewModel Data = new AlbumViewModel();
            Data.File = albumService.GetAlbumById(Alb_Id);
            if (Data.File != null)
            {
                Stream iStream = new FileStream(Data.File.Url, FileMode.Open, FileAccess.Read);
                return File(iStream, Data.File.Type, Data.File.FileName);
            }
            else
            {
                return JavaScript("alert('無此檔案')");
            }
        }
        [Authorize]
        public ActionResult DeleteFile(int Alb_Id)
        {
            albumService.Delete(Alb_Id);
            return RedirectToAction("Index");
        }
        public ActionResult Carousel()
        {
            AlbumViewModel Data = new AlbumViewModel();
            Data.Paging = new ForPaging(1);
            Data.FileList = albumService.GetDataList(Data.Paging);
            return View(Data);
        }
    }
}