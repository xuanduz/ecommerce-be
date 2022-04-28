using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanQA.Models.Class
{
    public class CartDetailModel
    {
        //public string CD_id { get; set; }
        //public string CD_PID { get; set; }
        public string CD_PID { get; set; }
        public string CD_S_name { get; set; }
        public string CD_COLslug { get; set; }
        public Nullable<int> CD_amount { get; set; }
        public string P_name { get; set; }
        public Nullable<double> P_discount { get; set; }
        public Nullable<decimal> P_price { get; set; }
        public string P_image { get; set; }
        public string P_slug { get; set; }

    }
}