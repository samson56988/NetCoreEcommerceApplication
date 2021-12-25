using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {

        private ApplicationDbContext _db;

        protected readonly IConfiguration _config;

        public HomeController(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
            ConnectionString = _config.GetConnectionString("DefaultConnection");
            ProviderName = "System.Data.SqlClient";
        }

        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }

        List<Product> _Products = new List<Product>();
        public IActionResult Index()
        {
            _Products = new List<Product>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("GetProduct", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Product stu = new Product();
                    stu.Id = Convert.ToInt32(rdr["Id"].ToString());
                    stu.Name = rdr["Name"].ToString();
                    stu.Price = Convert.ToDecimal(rdr["Price"].ToString());
                    stu.ProductColor = rdr["ProductColor"].ToString();
                    stu.isAvailable = Convert.ToBoolean(rdr["isAvailable"].ToString());
                    stu.ProductType = rdr["Product"].ToString();
                    stu.Image = rdr["Image"].ToString();
                    _Products.Add(stu);
                }
                rdr.Close();
            }

            return View(_Products);
        }


        public ActionResult Details(int? id)
        {

            if(id==null)
            {
                return NotFound();
            }
            Product product = new Product();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("GetProductById", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@ProductId", id);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    product.Id = Convert.ToInt32(rdr["Id"].ToString());
                    product.Name = rdr["Name"].ToString();
                    product.Price = Convert.ToDecimal(rdr["Price"].ToString());
                    product.ProductColor = rdr["ProductColor"].ToString();
                    product.isAvailable = Convert.ToBoolean(rdr["isAvailable"].ToString());
                    product.ProductType = rdr["Product"].ToString();
                    product.SpecialType = rdr["SpecialTag"].ToString();
                    product.Image = rdr["Image"].ToString();
                }
                rdr.Close();
            }

            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }

   
    }
}
