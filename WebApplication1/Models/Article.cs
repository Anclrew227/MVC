using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Article
    {
        public int A_Id { get; set; }
        [DisplayName("發表者：")]
        public string Account { get; set; }
        [DisplayName("文章標題：")]
        [Required(ErrorMessage = "請輸入文章標題")]
        [StringLength(50,ErrorMessage = "文章標題最多50字元")]
        public string Title { get; set; }
        [DisplayName("文章內容：")]
        [Required(ErrorMessage = "請輸入文章內容")]
        public string Content { get; set; }
        [DisplayName("新增時間：")]
        public DateTime CreateTime { get; set; }
        [DisplayName("觀看人數：")]
        public int Watch { get; set; }
        public Members Member { get; set; } = new Members();
    }
}