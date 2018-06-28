using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // Declare list of PageVM
            List<PagesVM> pagesList;


            using (Db db = new Db())
            {
                // Init the list
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PagesVM(x)).ToList();
            }


            // Return view with list
            return View(pagesList);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // GET: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PagesVM model)
        {
            // Check Model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                // Declare slug
                string slug;

                // init pageDTO
                PageDTO dto = new PageDTO();

                // DTO title
                dto.Title = model.Title;

                // Check for and set slug if needed
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                // Make sure title and slug are unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists.");
                    return View(model);
                }

                // DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;


                // Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            // Set TempData Message
            TempData["SM"] = "You have added a new page.";

            //Redirect
            return RedirectToAction("AddPage");
        }

        // GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //Declare pageVM
            PagesVM model;

            using (Db db = new Db())
            {
                // Get the page
                PageDTO dto = db.Pages.Find(id);

                // Comfirm page exists
                if (dto == null)
                {
                    return Content("The page does not exists.");
                }
                // init pageVM
                model = new PagesVM(dto);
            }

            //return view with model
            return View(model);
        }

        // POST: Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PagesVM model)
        {
            // Check Model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //get page id
                int id = model.Id;

                // declare slug
                string slug = "home";

                // get the page
                PageDTO dto = db.Pages.Find(id);

                // DTO title
                dto.Title = model.Title;

                // Check for and set slug if needed
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }



                // Make sure title and slug are unique
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) || 
                db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists.");
                    return View(model);
                }

                // DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                
                // Save DTO
                db.SaveChanges();
            }

            // Set TempData Message
            TempData["SM"] = "You have edited the page.";

            //Redirect
            return RedirectToAction("EditPage");
        }

        // GET: Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            //declare pageVM
            PagesVM model;

            using (Db db = new Db())
            {


                //get the page
                PageDTO dto = db.Pages.Find(id);

                // confirm page exists
                if ( dto == null)
                {
                    return Content("That page does not exists.");
                }

                // init pageVM
                model = new PagesVM(dto);
            }
            //return view with the model
            return View(model);
        }
    }
}