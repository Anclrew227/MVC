using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;
using WebApplication1.Serivces;

namespace WebApplication1.ViewModels
{
    public class AlbumViewModel
    {
        [DisplayName("上傳圖片")]
        [FileExtensions(ErrorMessage = "上傳的不是圖片")]
        public HttpPostedFileBase upload { get; set; }
        public List<Album> FileList { get; set; }
        public ForPaging Paging { get; set; }
        public Album File { get; set; }
    }
}