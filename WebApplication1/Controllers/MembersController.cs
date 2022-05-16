using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Security;
using WebApplication1.Serivces;
using WebApplication1.ViewModels;
using System.Web.Configuration;
using System.IO;

namespace WebApplication1.Controllers
{
    public class MembersController : Controller
    {
        // GET: Members
        private readonly MembersDBService membersDBService = new MembersDBService();
        private readonly MailServicecs mailServicecs = new MailServicecs();
        private readonly CartService cartService = new CartService();
        public ActionResult Index()
        {
            return View();
        }
        #region 註冊
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterMembersViewModel RegisterMember)
        {
            if (ModelState.IsValid)
            {
                if (RegisterMember.MemberImage != null)
                {
                    if (membersDBService.CheckImage(RegisterMember.MemberImage.ContentType))
                    {
                        string filename = Path.GetFileName(RegisterMember.MemberImage.FileName);
                        string url = Path.Combine(Server.MapPath("~/Upload/"), filename);
                        RegisterMember.MemberImage.SaveAs(url);
                        RegisterMember.member.Image = filename;
                        RegisterMember.member.Password = RegisterMember.Password;
                        string AuthCode = mailServicecs.GetAuthCode();
                        RegisterMember.member.AuthCode = AuthCode;
                        membersDBService.Register(RegisterMember.member);
                        string TempBody = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/RegisterMail.html"));
                        UriBuilder ValidateUrl = new UriBuilder(Request.Url)
                        {
                            Path = Url.Action("EmailValidate", "Members", new { Account = RegisterMember.member.Account, AuthCode = AuthCode })
                        };
                        string MailBody = mailServicecs.ChangeEmail(TempBody, RegisterMember.member.Name, ValidateUrl.ToString().Replace("%3F", "?"));
                        mailServicecs.SendEmail(MailBody, RegisterMember.member.Email);
                        TempData["RegisterResult"] = "註冊成功";
                        return RedirectToAction("RegisterResult");
                    }
                    else
                    {
                        ModelState.AddModelError("MemberImage", "這不是圖片格式");
                    }
                }
                else
                {
                    ModelState.AddModelError("MemberImage", "請選擇圖片");
                }
            }
            RegisterMember.Password = null;
            RegisterMember.PasswordCheck = null;
            return View(RegisterMember);
        }
        #endregion
        #region 註冊結果
        public ActionResult RegisterResult()
        {
            return View();
        }
        #endregion
        #region 驗證結果
        public ActionResult EmailValidate(string Account,string AuthCode)
        {
            ViewData["EmailValidtae"] = membersDBService.EmailValidate(Account, AuthCode);
            return View();
        }
        #endregion
        #region 帳號驗證
        public JsonResult AccountCheck(RegisterMembersViewModel RegisterMember)
        {
            return Json(membersDBService.AccountCheck(RegisterMember.member.Account), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 登入
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginMemberViewModel LoginMember)
        {
            string LoginStr = membersDBService.Login(LoginMember.Account, LoginMember.Password);
            if (string.IsNullOrWhiteSpace(LoginStr))
            {
                HttpContext.Session.Clear();
                string Cart = cartService.GetCartSave(LoginMember.Account);
                if (Cart != null)
                {
                    HttpContext.Session["Cart"] = Cart;
                }
                string RoleData = membersDBService.GetRole(LoginMember.Account);
                JwtService jwtService = new JwtService();
                string Token = jwtService.CreateToken(LoginMember.Account, RoleData);
                string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Value = Server.UrlEncode(Token);
                cookie.Expires = DateTime.Now.AddMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["ExpireMinutes"]));
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("123", LoginStr);
                return View();
            }
        }
        #endregion
        #region 登出
        [Authorize]
        public ActionResult Logout()
        {
            string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values.Clear();
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Set(cookie);
            return RedirectToAction("Login");
        }
        #endregion
        #region 修改密碼
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordViewModel ChangeMember)
        {
            string ChangeResult = membersDBService.ChangePassword(User.Identity.Name, ChangeMember.Password, ChangeMember.newPassword);
            if (string.IsNullOrWhiteSpace(ChangeResult))
            {
                TempData["ChangeResult"] = "密碼修改成功";
                return RedirectToAction("Logout");
            }
            else
            {
                ViewData["ChangeResult"] = ChangeResult;
                return View();
            }
        }
        #endregion
    }
}