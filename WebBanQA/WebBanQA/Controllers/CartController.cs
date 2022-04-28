using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebBanQA.Models.Cart;
using WebBanQA.Models.Class;

namespace WebBanQA.Controllers
{
    public class CartController : ApiController
    {
        //[EnableCors("*", "*", "*")]
        //[EnableCorsAttribute("*", "*", "*")]
        // GET: Cart
        [HttpGet]
        [Route("api/getCartByUser/{userName}")]
        public IEnumerable<CartModel> GetCartByUser(string userName)
        {
            DBCartDataContext db = new DBCartDataContext();
            var res = from ca in db.Carts
                      join u in db.Users
                      on ca.CAR_UID equals u.U_id

                      where u.U_name == userName
                      select new CartModel()
                      {
                          car_date = ca.car_date,
                          U_name = u.U_name,

                          CartDetail = GetCartDetail(ca.CAR_id, u.U_id),
                      };
            return res.ToList();
        }

        public List<CartDetailModel> GetCartDetail(string car_id, string uid)
        {
            DBCartDataContext db = new DBCartDataContext();
            var res = from cd in db.Cartdetas
                      join ca in db.Carts
                      on cd.CD_CarID equals ca.CAR_id
                      join u in db.Users
                      on ca.CAR_UID equals u.U_id
                      join pr in db.Products
                      on cd.CD_PID equals pr.P_id

                      where cd.CD_CarID == car_id
                      && u.U_id == uid
                      select new CartDetailModel()
                      {
                          CD_PID = cd.CD_PID,
                          CD_S_name = cd.CD_S_name,
                          CD_COLslug = cd.CD_COLslug,
                          CD_amount = cd.CD_amount,
                          P_name = pr.P_name,
                          P_discount = pr.P_discount,
                          P_price = pr.P_Price,
                          P_image = pr.P_image,
                          P_slug = pr.P_slug,
                      };
            return res.ToList();
        }

        [HttpPost]
        [Route("api/cart")]
        public bool InsertItemAddToCart(string CD_PID, string username, string CD_COLslug, string CD_S_name, int CD_amount)
        {
            try
            {
                DBCartDataContext db = new DBCartDataContext();
                Cartdeta cartdeta = new Cartdeta();
                cartdeta.CD_id = "";
                cartdeta.CD_PID = CD_PID;
                cartdeta.CD_CarID = GetCartID(username);
                cartdeta.CD_COLslug = CD_COLslug;
                cartdeta.CD_S_name = CD_S_name;
                cartdeta.CD_amount = CD_amount;
                db.Cartdetas.InsertOnSubmit(cartdeta);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        [Route("api/cart/update/")]
        public bool UpdateItemAddToCart(string CD_PID, string username, string CD_COLslug, string CD_S_name, int amount)
        {
            try
            {
                DBCartDataContext db = new DBCartDataContext();
                string CarID = GetCartID(username);
                Cartdeta cartdeta = (from ca in db.Cartdetas
                                     where ca.CD_PID == CD_PID
                                     && ca.CD_CarID == CarID
                                     && ca.CD_COLslug == CD_COLslug
                                     && ca.CD_S_name == CD_S_name
                                     select ca).SingleOrDefault();
                cartdeta.CD_amount = amount;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        [Route("api/cart/delete/")]
        public bool DeleteItemOnCart(string CD_PID, string username, string CD_COLslug, string CD_S_name)
        {
            try
            {
                DBCartDataContext db = new DBCartDataContext();
                string CarID = GetCartID(username);
                Cartdeta cartdetaForRemove = (from ca in db.Cartdetas
                                     where ca.CD_PID == CD_PID
                                     && ca.CD_CarID == CarID
                                     && ca.CD_COLslug == CD_COLslug
                                     && ca.CD_S_name == CD_S_name
                                     select ca).SingleOrDefault();
                db.Cartdetas.DeleteOnSubmit(cartdetaForRemove);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetCartID(string username)
        {
            DBCartDataContext db = new DBCartDataContext();
            var carId = from u in db.Users
                         join ca in db.Carts
                         on u.U_id equals ca.CAR_UID

                         where u.U_name == username
                         select ca.CAR_id;
            return carId.SingleOrDefault().ToString();
        }
    }
}