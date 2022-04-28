using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanQA.Models.Class
{
    public class ListProduct
    {
        public string ST_slug { get; set; }
        public string CA_name { get; set; }
        public string P_id { get; set; }
        public string P_name { get; set; }
        public Nullable<double> P_discount { get; set; }
        public Nullable<decimal> P_price { get; set; }
        public string P_image { get; set; }
        public string P_note { get; set; }
        public Nullable<int> P_amount { get; set; }

        public string P_content { get; set; }
        public string P_CAID { get; set; }
        public string P_slug { get; set; }
        public List<ColorModel> P_color { get; set; }
        public List<SizeModel> P_size { get; set; }
    }
}