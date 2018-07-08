using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModels.Shop;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop
        public ActionResult Categories()
        {
            // declare list of models
            List<CategoryVM> categoriesVMList;
            // init the list
            using (Db db = new Db())
            {
                // return view with the list
                categoriesVMList = db.Categories
                    .ToArray()
                    .OrderBy(x => x.Sorting)
                    .Select(x => new CategoryVM(x))
                    .ToList();
            }


            return View(categoriesVMList);
        }

        // Post: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            // delcare id
            string id;

            using (Db db = new Db())
            {

            
                //check cata name is unique
                if (db.Categories.Any(x => x.Name == catName))
                    return "titletaken";

                // init dto and add to dto
                CategoryDTO dto = new CategoryDTO
                {
                    Name = catName,
                    Slug = catName.Replace(" ", "-").ToLower(),
                    Sorting = 100
                };

                // save dto
                db.Categories.Add(dto);
                db.SaveChanges();

                // get the id
                id = dto.Id.ToString();

            }
            // return the id
            return id;
        }
    }
}