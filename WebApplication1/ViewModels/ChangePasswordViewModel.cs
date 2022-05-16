using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DisplayName("舊密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
        [DisplayName("新密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string newPassword { get; set; }
        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [Compare("newPassword",ErrorMessage = "密碼兩次輸入不一致")]
        public string newPasswordCheck { get; set; }
    }
}