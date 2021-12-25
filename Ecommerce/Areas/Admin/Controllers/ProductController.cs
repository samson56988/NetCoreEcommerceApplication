using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IWebHostEnvironment _he;

        protected readonly IConfiguration _config;
        
        public ProductController(ApplicationDbContext db, IConfiguration config, IWebHostEnvironment he)
        {
            _db = db;
            _config = config;
            ConnectionString = _config.GetConnectionString("DefaultConnection");
            ProviderName = "System.Data.SqlClient";
            _he = he;
        }

        List<Product> _Products = new List<Product>();

        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }

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
                    _Products.Add(stu);
                }
                rdr.Close();
            }

            return View(_Products);
        }

        [HttpPost]
        public IActionResult Index(decimal? LowAmount, decimal? LargeAmount)
        {
            _Products = new List<Product>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                if (LowAmount == null || LargeAmount == null)
                {
                    SqlCommand cmd2 = new SqlCommand("GetProduct", con);
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader rdr2 = cmd2.ExecuteReader();
                    while (rdr2.Read())
                    {
                        Product stu = new Product();
                        stu.Id = Convert.ToInt32(rdr2["Id"].ToString());
                        stu.Name = rdr2["Name"].ToString();
                        stu.Price = Convert.ToDecimal(rdr2["Price"].ToString());
                        stu.ProductColor = rdr2["ProductColor"].ToString();
                        stu.isAvailable = Convert.ToBoolean(rdr2["isAvailable"].ToString());
                        stu.ProductType = rdr2["Product"].ToString();
                        _Products.Add(stu);
                    }
                    rdr2.Close();
                }
                else

                {
                    SqlCommand cmd = new SqlCommand("searchproduct", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    con.Open();
                    cmd.Parameters.AddWithValue("@Lowamount", LowAmount);
                    cmd.Parameters.AddWithValue("@Highamount", LargeAmount);
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
                        _Products.Add(stu);
                    }
                    rdr.Close();

                }
            }

            return View(_Products);
        }


        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_db.ProductTypes.ToList(), "ProductId", "Product");
            ViewData["TagID"] = new SelectList(_db.SpecialTags.ToList(), "SpecialTagId", "SpecialTag");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product,IFormFile image)
        {
           

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Image", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Image/" + image.FileName;
                }
                if(image == null)
                {
                    product.Image = "Images/noimage.PNG";
                }
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd2 = new SqlCommand("AddProduct", con);
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@Name", product.Name);
                    cmd2.Parameters.AddWithValue("@Price", product.Price);
                    cmd2.Parameters.AddWithValue("@Image", product.Image);
                    cmd2.Parameters.AddWithValue("@Productcolor", product.ProductColor);
                    cmd2.Parameters.AddWithValue("@isAvailable", product.isAvailable);
                    cmd2.Parameters.AddWithValue("@ProductTypeId", product.ProductTypeId);
                    cmd2.Parameters.AddWithValue("@SpecialTypeId", product.SpecialTypeId);
                    cmd2.ExecuteNonQuery();
                    con.Close();
                    TempData["save"] = "Product has been saved successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
           
            return View(product);
        }

        public IActionResult Edit(int? Id)
        {
            ViewData["ProductTypeId"] = new SelectList(_db.ProductTypes.ToList(), "ProductId", "Product");
            ViewData["TagID"] = new SelectList(_db.SpecialTags.ToList(), "SpecialTagId", "SpecialTag");
            if (Id == null)
                return NotFound();

            Product product = new Product();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("GetProductById", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@ProductId", Id);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    product.Id = Convert.ToInt32(rdr["Id"].ToString());
                    product.Name = rdr["Name"].ToString();
                    product.Price = Convert.ToDecimal(rdr["Price"].ToString());
                    product.ProductColor = rdr["ProductColor"].ToString();
                    product.isAvailable = Convert.ToBoolean(rdr["isAvailable"].ToString());
                    product.ProductType = rdr["Product"].ToString();
                    product.Image = rdr["Image"].ToString();
                }
                rdr.Close();
            }

            
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile image)
        {
            product.Image = image.FileName;

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Image", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Image/" + image.FileName;
                }
                if (image == null)
                {
                    product.Image = "Images/noimage.PNG";
                }
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd2 = new SqlCommand("UpdateProduct", con);
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@ProductId", product.Id);
                    cmd2.Parameters.AddWithValue("@Name", product.Name);
                    cmd2.Parameters.AddWithValue("@Price", product.Price);
                    cmd2.Parameters.AddWithValue("@Image", product.Image);
                    cmd2.Parameters.AddWithValue("@Productcolor", product.ProductColor);
                    cmd2.Parameters.AddWithValue("@isAvailable", product.isAvailable);
                    cmd2.Parameters.AddWithValue("@ProductTypeId", product.ProductTypeId);
                    cmd2.Parameters.AddWithValue("@SpeacialTag", product.SpecialTypeId);
                    cmd2.ExecuteNonQuery();
                    con.Close();
                    TempData["edit"] = "Product has been Updated successfully";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(product);
        }

      









    }
}
