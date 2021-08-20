using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MS.Models.RequestModel
{
    public class PageModel
    {
        [Display(Name ="页码")]
        public int Page { get; set; }


        [Display(Name = "单页数")]
        public int Limit { get; set; }


        [Display(Name = "筛选关键字")]
        public string Key { get; set; }


        [Display(Name = "排序")]
        public string Order { get; set; }


        [Display(Name ="排序字段")]
        public string OrderField { get; set; }

        //public PageModel()
        //{
        //    Page = 1;
        //    Limit = 10;
        //}
    }
}
