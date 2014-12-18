using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UniversityMnagementSystemMVC.Models;


namespace UniversityMnagementSystemMVC.Controllers
{
    public class CourseController : Controller
    {
        private UniversityMvcDBEntities db = new UniversityMvcDBEntities();

        public ActionResult CourseInformationView()
        {
            CourseInformatonViewModel aCourseInformatonViewModel = new CourseInformatonViewModel();
            aCourseInformatonViewModel.Departments = db.Departments.ToList();
            aCourseInformatonViewModel.Teachers = db.Teachers.ToList();
            aCourseInformatonViewModel.CourseAssignToTeachers = db.CourseAssignToTeachers.ToList();
            aCourseInformatonViewModel.Courses = db.Courses.ToList();
            return View(aCourseInformatonViewModel);
        }

        [HttpPost]
        public PartialViewResult _CourseInformationViewPartial(int id)
        {
            CourseInformatonViewModel aCourseInformatonViewModel = new CourseInformatonViewModel();
            aCourseInformatonViewModel.Departments = db.Departments.ToList();
            aCourseInformatonViewModel.CourseAssignToTeachers = db.CourseAssignToTeachers.Where(x=>x.DeptId==id).ToList();

            foreach (var teacher in db.Teachers.ToList())
            {
                foreach (var source in aCourseInformatonViewModel.CourseAssignToTeachers)
                {
                    if (source.TeacherId==teacher.TeacherId)
                    {
                        
                        var tech = db.Teachers.Where(x => x.TeacherId == source.TeacherId).ToList();
                        aCourseInformatonViewModel.Teachers=tech;
                    }
                }
            }
            foreach (var course in db.Courses.ToList())
            {
                foreach (var source in aCourseInformatonViewModel.CourseAssignToTeachers)
                {
                    if (source.CourseId==course.CourseId)
                    {
                        var cou = db.Courses.Where(x => x.CourseId == source.CourseId).ToList();
                        aCourseInformatonViewModel.Courses=cou;
                    }
                }
            }
            return PartialView(aCourseInformatonViewModel);
        }


        // GET: Course
        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.Department);
            return View(courses.ToList());
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code");
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseId,Name,Code,Description,DeptId,Semester,Credit")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception exception)
                {

                    return View("ErrorPage");
                }
                return RedirectToAction("Index");
            }

            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code", course.DeptId);
            return View(course);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code", course.DeptId);
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseId,Name,Code,Description,DeptId,Semester,Credit")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception exception)
                {

                    return View("ErrorPage");
                }
                return RedirectToAction("Index");
            }
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code", course.DeptId);
            return View(course);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            try
            {
                db.SaveChanges();
            }
            catch (Exception exception)
            {

                return View("ErrorPage");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult CodeExists(string code)
        {
            var user = db.Courses.Where(x => x.Code == code).FirstOrDefault();
            if (user != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NameExists(string name)
        {
            var user = db.Courses.Where(x => x.Name == name).FirstOrDefault();
            if (user != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
