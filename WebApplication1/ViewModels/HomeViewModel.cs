using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Serivces;

namespace WebApplication1.ViewModels
{
    public class HomeViewModel
    {
        [DisplayName("搜尋")]
        public string Search { get; set; }
        public List<Members> DataList { get; set; }
        public ForPaging Paging { get; set; }
    }
}