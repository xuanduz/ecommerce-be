using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanQA.Models.Class
{
    public class CartModel
    {
        public Nullable<DateTime> car_date { get; set; }
        public string U_name { get; set; }
        
        public List<CartDetailModel> CartDetail { get; set; }
    }
}