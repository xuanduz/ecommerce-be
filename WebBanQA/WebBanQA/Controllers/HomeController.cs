using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net;
using WebBanQA.Models;
using WebBanQA.Models.Class;
using WebBanQA.Models.Cart;
using User = WebBanQA.Models.User;

namespace WebBanQA.Controllers
{
    // khi them moi controller thi phai go cai dong nay
    //[EnableCors("*", "*", "*")]
    //[EnableCorsAttribute("*", "*", "*")]

    public class HomeController : ApiController
    {
        //https://localhost:44314/api/home
        [HttpGet]
        [Route("api/home/topSaleProduct/{number}")]
        public IEnumerable<Models.Product> GetTopSaleProduct(int number)
        {
            DBProductDataContext db = new DBProductDataContext();
            return db.Products.OrderByDescending(x => x.P_discount).Take(number).ToList();
        }

        [HttpGet]
        [Route("api/login/{username}/{password}")]
        public bool Login(string username, string password)
        {
            DBUserDataContext db = new DBUserDataContext();
            var res = db.Users.Where(x => x.U_name == username && x.U_pass == password).FirstOrDefault();
            if (res == null)
            {
                return false;
            }
            return true;
        }
        
        [HttpPost]
        [Route("api/register")]
        public string Register(string fname, string lname, string uname, string password, string email, string phonenumber, string address)
        {
            try
            {
                DBUserDataContext db = new DBUserDataContext();
                User user = new User();

                var checkUserName = db.Users.Where(x => x.U_name == uname).SingleOrDefault();
                var checkEmail = db.Users.Where(x => x.U_email == email).SingleOrDefault();
                var checkPhoneNumber = db.Users.Where(x => x.U_contact == phonenumber).SingleOrDefault();

                if(checkUserName != null)
                {
                    return "Username exist";
                }
                else if(checkEmail != null)
                    {
                        return "Email exist";
                    }
                else if(checkPhoneNumber != null)
                {
                    return "Phone number exist";
                }
                    else
                    {
                        // insert new user
                        user.U_id = "";
                        user.U_Fname = fname;
                        user.U_Lname = lname;
                        user.U_name = uname;
                        user.U_email = email;
                        user.U_status = "true";
                        user.U_add = address;
                        user.U_contact = phonenumber;
                        user.U_created = DateTime.Now;
                        user.U_pass = password;
                        db.Users.InsertOnSubmit(user);
                        db.SubmitChanges();

                        return "Success";
                    }
            }
            catch
            {
                return "Fail";
            }
        }

        [HttpGet]
        [Route("api/checkAccount")]
        public string checkAccountExist(string username)
        {
            DBUserDataContext db = new DBUserDataContext();
            var checkUserName = db.Users.Where(x => x.U_name == username).SingleOrDefault();
            var checkEmail = db.Users.Where(x => x.U_email == username).SingleOrDefault();
            if (checkUserName != null)
            {
                return checkUserName.U_name.ToString();
            }else if (checkEmail != null)
            {
                return checkEmail.U_name.ToString();
            }
            else
            {
                return "NOT EXIST";
            }
        }
    }
}
