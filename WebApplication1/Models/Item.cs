﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Item
    {
        [DisplayName("商品編號")]
        public int Id { get; set; }
        [DisplayName("商品名稱")]
        [Required(ErrorMessage = "請輸入商品名稱")]
        [StringLength(50, ErrorMessage = "商品名稱不可大於50字元")]
        public string Name { get; set; }
        [DisplayName("價格")]
        [Required(ErrorMessage = "請輸入價格")]
        [Range(1, 9999, ErrorMessage = "價格請介於1-9999")]
        public int Price { get; set; }
        [DisplayName("圖片")]
        public string Image { get; set; }
    }
}