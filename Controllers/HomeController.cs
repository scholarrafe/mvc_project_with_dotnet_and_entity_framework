using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test_project;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        app_databaseDbContext _context = new app_databaseDbContext();
        public ActionResult Index()
        {
            var listofData = _context.Students.ToList();
            return View(listofData);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var classList = _context.Classes.ToList();
            var classSelectList = new SelectList(classList, "Id", "Id");
            ViewData["ClassId"] = classSelectList;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Student model)
        {
            model.Id = Guid.NewGuid();
            _context.Students.Add(model);
            _context.SaveChanges();
            ViewBag.Message = "Data inserted successfully";
            return RedirectToAction("Index");
               

            // Repopulate the ClassId dropdown list in case of validation failure
            var classes = _context.Classes.ToList();
            ViewBag.ClassId = new SelectList(classes, "Id", "Id", model.ClassId);

            return View(model);
        }





        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var data = _context.Students.Where(x => x.Id == id).FirstOrDefault();
            var classList = _context.Classes.ToList();
            var classSelectList = new SelectList(classList, "Id", "Id");
            ViewData["ClassId"] = classSelectList;
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(Student model)
        {
            try
            {
                var data = _context.Students.Find(model.Id);
                if (data != null)
                {
                    data.Name = model.Name;
                    data.Gender = model.Gender;
                    data.DOB = model.DOB;
                    data.ClassId = model.ClassId;
                    data.CreatedDate = model.CreatedDate;
                    data.ModificationDate = model.ModificationDate;

                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while updating the student.";
                return View(model);
            }
        }


        public ActionResult Details(Guid id)
        {
            var data = _context.Students.Where(x => x.Id == id).FirstOrDefault();
            return View(data);
        }

        public ActionResult Delete(Guid id)
        {
            try
            {
                var data = _context.Students.Find(id);
                if (data != null)
                {
                    _context.Students.Remove(data);
                    _context.SaveChanges();
                    ViewBag.Message = "Data Removed";
                }
                else
                {
                    ViewBag.Message = "Student not found";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while deleting the student.";
            }

            return RedirectToAction("Index");
        }

    }
}