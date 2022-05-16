using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Serivces;

namespace WebApplication1.ViewModels
{
    public class ItemViewModel
    {
        public List<int> IdList { get; set; }
        public List<ItemDetailViewModel> ItemBlock { get; set; } = new List<ItemDetailViewModel>();
        public ForPaging Paging { get; set; }
    }
}