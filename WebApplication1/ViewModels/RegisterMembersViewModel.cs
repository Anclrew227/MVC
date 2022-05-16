using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class RegisterMembersViewModel
    {
        [DisplayName("大頭照：")]
        public HttpPostedFileBase MemberImage { get; set; }
        public Members member { get; set; }
        [DisplayName("密碼：")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
        [DisplayName("確認密碼：")]
        [Required(ErrorMessage = "請輸入密碼")]
        [Compare("Password",ErrorMessage = "密碼兩次輸入不一致")]
        public string PasswordCheck { get; set; }
    }
}