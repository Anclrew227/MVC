using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class LoginMemberViewModel
    {
        [DisplayName("會員帳號")]
        [Required(ErrorMessage = "請輸入帳號")]
        public string Account { get; set; }
        [DisplayName("會員密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
    }
}