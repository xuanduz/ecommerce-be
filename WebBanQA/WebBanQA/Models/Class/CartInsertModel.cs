using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanQA.Models.Class
{
    public class CartInsertModel
    {
        public string car_uid { get; set; }
        public string car_select { get; set; }
        public string car_status { get; set; }
        public Nullable<DateTime> car_date { get; set; }

    }
}