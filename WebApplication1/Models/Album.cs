using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace WebApplication1.Models
{
    public class Album
    {
        public int Alb_Id { get; set; }
        [DisplayName("檔名")]
        public string FileName { get; set; }
        [DisplayName("路徑")]
        public string Url { get; set; }
        [DisplayName("大小")]
        public int Size { get; set; }
        public string Type { get; set; }
        [DisplayName("上傳者")]
        public string Account { get; set; }
        [DisplayName("新增時間")]
        public DateTime CreateTime { get; set; }
        public Members Member { get; set; } = new Members();
    }
}