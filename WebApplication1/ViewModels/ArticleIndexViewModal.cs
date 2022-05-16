using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using WebApplication1.Models;
using WebApplication1.Serivces;

namespace WebApplication1.ViewModels
{
    public class ArticleIndexViewModal
    {
        [DisplayName("搜尋")]
        public string Search { get; set; }
        public List<Article> DataList { get; set; }
        public ForPaging Paging { get; set; }
        public string Account { get; set; }
    }
}