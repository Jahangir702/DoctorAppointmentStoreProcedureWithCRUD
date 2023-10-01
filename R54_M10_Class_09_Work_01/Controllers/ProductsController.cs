using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using R54_M10_Class_09_Work_01.Models;
using R54_M10_Class_09_Work_01.ViewModels.Input;
using System.Data;
using X.PagedList;

namespace R54_M10_Class_09_Work_01.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductDbContext db;
        private readonly IWebHostEnvironment env;
        public ProductsController(ProductDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public async Task<IActionResult> Index(int pg=1)
        {
            return View(await db.Products.OrderBy(x=> x.ProductId).Include(x=> x.Sales).ToPagedListAsync(pg, 5));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductInputModel model)
        {
            if(ModelState.IsValid)
            {
               
                
               
                 await  db.Database.ExecuteSqlInterpolatedAsync($"EXEC InsertProduct {model.ProductName}, {model.Price}, {(int)model.Size}, {model.Picture}, {(model.OnSale ? 1 : 0)}");
                 var id = await db.Products.MaxAsync(x=> x.ProductId);
                foreach(var s in model.Sales)
                {
                    await db.Database.ExecuteSqlInterpolatedAsync($"EXEC InsertSale {s.Date}, {s.Quantity}, {id}");
                }
                return Json(new {success=true});
               
            }
            return Json(new { success = true });
        }
        public IActionResult GetSalesForm()
        {
            return PartialView("_SalesForm");
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            string ext = Path.GetExtension(file.FileName);
            string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
            string savePath = Path.Combine(env.WebRootPath, "Pictures", fileName);
            FileStream fs = new FileStream(savePath, FileMode.Create);
            await file.CopyToAsync(fs);
            fs.Close();
            return Json(new { name = fileName });
        }
        public async Task<IActionResult> Edit(int id)
        {
            var data = await db.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (data == null) return NotFound();
            return View(new ProductEditModel
            {
                ProductId = data.ProductId,
                ProductName = data.ProductName,
                Price = data.Price,
                Size = data.Size ?? Size.S,
                OnSale = data.OnSale ?? false

            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditModel model)
        {
            if (ModelState.IsValid)
            {
                var product = await db.Products.FirstOrDefaultAsync(x => x.ProductId == model.ProductId);
                if (product == null) return NotFound();
                product.ProductId = model.ProductId;
                product.ProductName = model.ProductName;
                product.Price = model.Price;
                product.Size = model.Size;
                product.OnSale = model.OnSale;

                if (model.Picture != null)
                {
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(env.WebRootPath, "Pictures", fileName);
                    FileStream fs = new FileStream(savePath, FileMode.Create);
                    await model.Picture.CopyToAsync(fs);
                    product.Picture = fileName;
                    fs.Close();
                }
                db.Database.ExecuteSqlInterpolated($"EXEC UpdateProduct {product.ProductId}, {product.ProductName}, {product.Price}, {(int)product.Size}, {product.Picture}, {(model.OnSale ? 1 : 0)}");
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            db.Database.ExecuteSqlInterpolated($"EXEC DeleteProduct {id}");
            return Json(new { success = true, id });
        }
    }
}
