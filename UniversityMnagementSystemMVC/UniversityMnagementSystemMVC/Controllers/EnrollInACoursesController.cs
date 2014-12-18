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
    public class EnrollInACoursesController : Controller
    {
        private UniversityMvcDBEntities db = new UniversityMvcDBEntities();


        public PartialViewResult StudentView()
        {
            var student = db.Students.First();
            return PartialView("_StudentViewPartial");
        }

        [HttpPost]
        public PartialViewResult StudentView(int id)
        {
            var student = db.Students.SingleOrDefault(x => x.StudentId == id);
            return PartialView("_StudentViewResultPartial",student);
        }

        // GET: EnrollInACourses
        public ActionResult Index()
        {
            var enrollInACourses = db.EnrollInACourses.Include(e => e.Course).Include(e => e.Student);
            return View(enrollInACourses.ToList());
        }

        // GET: EnrollInACourses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnrollInACourse enrollInACourse = db.EnrollInACourses.Find(id);
            if (enrollInACourse == null)
            {
                return HttpNotFound();
            }
            return View(enrollInACourse);
        }

        // GET: EnrollInACourses/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo");
            return View();
        }

        // POST: EnrollInACourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollInACourseId,Date,CourseId,StudentId")] EnrollInACourse enrollInACourse)
        {
            if (ModelState.IsValid)
            {
                db.EnrollInACourses.Add(enrollInACourse);
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

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", enrollInACourse.CourseId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Name", enrollInACourse.StudentId);
            return View(enrollInACourse);
        }

        // GET: EnrollInACourses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnrollInACourse enrollInACourse = db.EnrollInACourses.Find(id);
            if (enrollInACourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", enrollInACourse.CourseId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Name", enrollInACourse.StudentId);
            return View(enrollInACourse);
        }

        // POST: EnrollInACourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollInACourseId,Date,CourseId,StudentId")] EnrollInACourse enrollInACourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollInACourse).State = EntityState.Modified;
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
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", enrollInACourse.CourseId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Name", enrollInACourse.StudentId);
            return View(enrollInACourse);
        }

        // GET: EnrollInACourses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnrollInACourse enrollInACourse = db.EnrollInACourses.Find(id);
            if (enrollInACourse == null)
            {
                return HttpNotFound();
            }
            return View(enrollInACourse);
        }

        // POST: EnrollInACourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EnrollInACourse enrollInACourse = db.EnrollInACourses.Find(id);
            db.EnrollInACourses.Remove(enrollInACourse);
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
    }
}
