using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebBanQA.Models;
using WebBanQA.Models.Class;
using WebBanQA.Models.MultiTable;
using style = WebBanQA.Models.style;

namespace WebBanQA.Controllers
{
    //[EnableCors("*", "*", "*")]
    //[EnableCorsAttribute("*", "*", "*")]
    public class ProductController : ApiController
    {
        private IEnumerable<ColorModel> temp;

        // GET: Product
        [HttpGet]
        [Route("api/detailProduct/{slug}")]
        public Models.MultiTable.Product GetDetailProductBySlug(string slug)
        {
            DBDetailProductDataContext db = new DBDetailProductDataContext();
            return db.Products.Where(x => x.P_slug == slug).FirstOrDefault();
        }

        [HttpGet]
        [Route("api/randomProduct/{number}")]
        public IEnumerable<Models.Product> GetRandomProduct(int number)
        {
            DBProductDataContext db = new DBProductDataContext();
            Random rand = new Random();
            int toSkip = rand.Next(0, db.Products.Count());
            return db.Products.Skip(toSkip).Take(number).ToList();
        }

        [HttpGet]
        [Route("api/getProductByStyle/{style}")]
        public List<ListProduct> GetListProductByStyle(string style)
        {
            DBDetailProductDataContext db = new DBDetailProductDataContext();
            var res = from pr in db.Products
                      join ca in db.catalogs
                      on pr.P_CAID equals ca.CA_id
                      join st in db.styles
                      on ca.CA_STID equals st.ST_id

                      where st.ST_slug == style
                      select new ListProduct()
                      {
                          ST_slug = st.ST_slug,
                          CA_name = ca.CA_name,
                          P_id = pr.P_id,
                          P_name = pr.P_name,
                          P_discount = pr.P_discount,
                          P_price = pr.P_Price,
                          P_image = pr.P_image,
                          P_note = pr.P_note,
                          P_amount = pr.P_amount,
                          P_content = pr.P_content,
                          P_CAID = pr.P_CAID,
                          P_slug = pr.P_slug,
                          P_color = (List<ColorModel>)GetColorByProdut(pr.P_id),
                          P_size = (List<SizeModel>)GetSizeByProdut(pr.P_id)
                      };
            return res.ToList();
        }

        [HttpGet]
        [Route("api/getStyle")]
        public IEnumerable<style> GetStyle()
        {
            DBStyleDataContext db = new DBStyleDataContext();
            return db.styles.ToList();
        }

        [HttpGet]
        [Route("api/getCatalog/{style}")]
        public IEnumerable<CatalogModel> getCatalog(string style)
        {
            DBDetailProductDataContext db = new DBDetailProductDataContext();
            var res = from ca in db.catalogs
                      join st in db.styles
                      on ca.CA_STID equals st.ST_id
                      where st.ST_slug == style
                      group ca by new { ca.CA_name } into temp
                      select new CatalogModel()
                      {
                          CA_name = temp.Key.CA_name
                      };
            return res.ToList();
        }

        public List<ColorModel> GetColorByProdut(string P_id)
        {
            DBDetailProductDataContext db = new DBDetailProductDataContext();
            var res = from co in db.Colors
                      join pr in db.Products
                      on co.COL_PID equals pr.P_id
                      where pr.P_id == P_id

                      group co by new { co.COL_slug, co.COL_name } into temp
                      select new ColorModel()
                      {
                          COL_slug = temp.Key.COL_slug,
                          COL_name = temp.Key.COL_name
                      };
            return res.ToList();
        }

        public List<SizeModel> GetSizeByProdut(string P_id)
        {
            DBDetailProductDataContext db = new DBDetailProductDataContext();
            var res = from si in db.Sizes
                      join pr in db.Products
                      on si.S_PID equals pr.P_id
                      where pr.P_id == P_id

                      group si by new { si.S_name } into temp
                      select new SizeModel()
                      {
                          S_name = temp.Key.S_name
                      };
            return res.ToList();
        }

        [HttpGet]
        [Route("api/getColor/{style}")]
        public IEnumerable<ColorModel> GetColor(string style)
        {
            DBDetailProductDataContext db = new DBDetailProductDataContext();
            var res = from co in db.Colors
                      join pr in db.Products
                      on co.COL_PID equals pr.P_id
                      join ca in db.catalogs
                      on pr.P_CAID equals ca.CA_id
                      join st in db.styles
                      on ca.CA_STID equals st.ST_id

                      where st.ST_slug == style
                      group co by new { co.COL_slug, co.COL_name } into temp
                      select new ColorModel()
                      {
                          COL_slug = temp.Key.COL_slug,
                          COL_name = temp.Key.COL_name
                      };
            return res.ToList();
        }

        [HttpGet]
        [Route("api/getSize/{style}")]
        public IEnumerable<SizeModel> GetSize(string style)
        {
            DBDetailProductDataContext db = new DBDetailProductDataContext();
            var res = from si in db.Sizes
                      join pr in db.Products
                      on si.S_PID equals pr.P_id
                      join ca in db.catalogs
                      on pr.P_CAID equals ca.CA_id
                      join st in db.styles
                      on ca.CA_STID equals st.ST_id

                      where st.ST_slug == style
                      group si by si.S_name into temp
                      select new SizeModel()
                      {
                          S_name = temp.Key
                      };
            return res.ToList();
        }
    }
}