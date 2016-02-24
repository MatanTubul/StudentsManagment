using System;
using System.Linq;
using System.Web.Mvc;
using StudentsManagment.Models;
using System.Data.Entity;
using System.Net;
using PagedList;

namespace StudentsManagment.Controllers
{
    public class HomeController : Controller
    {
        private studentsDBEntities _db = new studentsDBEntities();
        
        // GET: Home
        public ActionResult Index(int? page)
        {
            return View(_db.students.ToList().ToPagedList(page ?? 1,2));
        }

        // GET: /Home/Create
        public ActionResult Create()
        {         
            return View();
        }

        [HttpPost]
        public ActionResult Create(student studentToCreate)
        {
            Boolean cityValidator = isCityIsValid(studentToCreate.City);

            if (!ModelState.IsValid ||!cityValidator)
            {
                if (!cityValidator)
                    ModelState.AddModelError("City", studentToCreate.City + " " + "is not a valid city name!");
                return View(studentToCreate);
            }          
            _db.students.Add(studentToCreate);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult AutoCompleteCity(string term)
        {

            var result = (from r in _db.Cities
                         
                          where r.CityName.StartsWith(term)
                          select new { r.CityName }).Distinct();
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //GET:/Home/Edit
        public ActionResult Edit(int id)
        {
            var studentToEdit = (from s in _db.students
                                 where s.Id == id
                                 select s).First();
            return View(studentToEdit);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(student studentToEdit)
        {
            Boolean cityValidator = isCityIsValid(studentToEdit.City);

            if (ModelState.IsValid && cityValidator)
            {
                _db.Entry(studentToEdit).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            if(!cityValidator)
                ModelState.AddModelError("City", studentToEdit.City+" "+"is not a valid city name!");
            return View(studentToEdit);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student personalDetail = _db.students.Find(id);
            if (personalDetail == null)
            {
                return HttpNotFound();
            }
            return View(personalDetail);
        }

        // POST: /Home/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            student personalDetail = _db.students.Find(id);
            _db.students.Remove(personalDetail);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*Function that get a city name as an input and check if the input
          is exist in the cities table.                   
        */
        public Boolean isCityIsValid(String city)
        {
            var result = (from r in _db.Cities

                          where r.CityName.Equals(city)
                          select new { r.CityName }).Distinct();
            if(result.Count() < 1)
            {
                return false;
            }
            return true;
        }


    }
}