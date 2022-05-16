using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class ItemDetailViewModel
    {
        public Item Data { get; set; }
        public bool InCart { get; set; }
    }
}