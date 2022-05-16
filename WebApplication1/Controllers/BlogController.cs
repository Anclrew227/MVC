using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Serivces;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class BlogController : Controller
    {
        public readonly ArticleDBService articleService = new ArticleDBService();
        public readonly MessageDBService messageService = new MessageDBService();
        private readonly MembersDBService memberService = new MembersDBService();
        // GET: Blog
        #region 部落格首頁
        public ActionResult Index(string Account)
        {
            BlogViewModal Data = new BlogViewModal();
            Data.Member = memberService.GetDatabyAccount(Account);
            return View(Data);
        }
        public ActionResult ShowPopular(string Account)
        {
            ArticleIndexViewModal Data = new ArticleIndexViewModal();
            Data.DataList = articleService.ShowPopular(Account);
            return PartialView(Data);
        }
        public ActionResult List(string Account, string Search, int Page = 1)
        {
            ArticleIndexViewModal Data = new ArticleIndexViewModal();
            Data.Account = Account;
            Data.Search = Search;
            Data.Paging = new ForPaging(Page);
            Data.DataList = articleService.GetDataList(Data.Account, Data.Search, Data.Paging);
            return PartialView(Data);
        }
        #endregion
        #region 文章
        public ActionResult Article(int A_Id)
        {
            ArticleViewModal Data = new ArticleViewModal();
            Data.article = articleService.GetArticleById(A_Id);
            articleService.AddWatch(A_Id);
            ForPaging Paging = new ForPaging(0);
            Data.DataList = messageService.GetDataList(A_Id, Paging);
            return View(Data);
        }
        [Authorize]
        public ActionResult CreateArticle()
        {
            return PartialView();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateArticle([Bind(Include = "Title,Content")] Article Data)
        {
            Data.Account = User.Identity.Name;
            articleService.InsertArticle(Data);
            return RedirectToAction("Index", new { Account = User.Identity.Name });
        }
        [Authorize]
        public ActionResult UpdateArticle(int A_Id)
        {
            Article Data = articleService.GetArticleById(A_Id);
            return PartialView(Data);
        }
        [Authorize]
        [HttpPost]
        public ActionResult UpdateArticle(int A_Id, [Bind(Include = "Content")] Article Data)
        {
            if (articleService.CheckUpdate(A_Id))
            {
                Data.A_Id = A_Id;
                articleService.UpdateArticle(Data);
            }
            return RedirectToAction("Article", new { A_Id = A_Id });
        }
        [Authorize]
        public ActionResult DeleteArticle(int A_Id)
        {
            articleService.DeleteArticle(A_Id);
            return RedirectToAction("Index", new { Account = User.Identity.Name });
        }
        #endregion
        #region 留言區
        public ActionResult Message(int A_Id)
        {
            ViewData["A_Id"] = A_Id;
            return PartialView();
        }
        public ActionResult MessageList(int A_Id, int Page = 1)
        {
            MessageViewModal Data = new MessageViewModal();
            Data.A_Id = A_Id;
            Data.Paging = new ForPaging(Page);
            Data.DataList = messageService.GetDataList(Data.A_Id, Data.Paging);
            return PartialView(Data);
        }
        [Authorize]
        public ActionResult CreateMessage(int A_Id)
        {
            ViewData["A_Id"] = A_Id;
            return PartialView();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateMessage(int A_Id, [Bind(Include = "Content")] Message Data)
        {
            Data.A_Id = A_Id;
            Data.Account = User.Identity.Name;
            messageService.InsertMessage(Data);
            return RedirectToAction("MessageList", new { A_Id = A_Id });
        }
        [Authorize]
        public ActionResult UpdateMessage(int A_Id, int M_Id, string Content)
        {
            Message Data = new Message();
            Data.A_Id = A_Id;
            Data.M_Id = M_Id;
            Data.Content = Content;
            messageService.UpdateMessage(Data);
            return RedirectToAction("Article", new { A_Id = A_Id });
        }
        [Authorize]
        public ActionResult DeleteMessage(int A_Id, int M_Id)
        {
            messageService.DeleteMessage(A_Id, M_Id);
            return RedirectToAction("Article", new { A_Id = A_Id });
        }
        #endregion
    }
}