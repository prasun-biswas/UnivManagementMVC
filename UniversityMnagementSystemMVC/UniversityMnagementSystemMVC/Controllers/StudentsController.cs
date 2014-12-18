using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using UniversityMnagementSystemMVC.Models;

namespace UniversityMnagementSystemMVC.Controllers
{
    public class StudentsController : Controller
    {
        private UniversityMvcDBEntities db = new UniversityMvcDBEntities();

        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Department);
            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,Name,Email,ContactNo,Date,Address,DeptId,RegNo")] Student student)
        {
            string StID = db.Students.Count().ToString();
            string enrollmentDate = student.Date.Year.ToString().TrimStart();

            List<Student> aList=new List<Student>();
            foreach (Student allStudent in db.Students)
            {
                aList.Add(allStudent);
            }

            Student lastStudentinList = aList.Last();
            string stdPkey = lastStudentinList.StudentId.ToString();
            
            if (ModelState.IsValid)
            {
                
                string newRegNo = string.Format(enrollmentDate + "-" + student.DeptId + "-" + stdPkey);


                student.RegNo = newRegNo;   
                student.Department = db.Departments.Where(x => x.DeptId == student.DeptId).SingleOrDefault();
                db.Students.Add(student);
                try
                {
                    db.SaveChanges();
                    var stu = db.Students.SingleOrDefault(x => x.RegNo == student.RegNo);
                    return View("RegistrationNumberView",stu);
                }
                catch (Exception exception)
                {

                    return View("ErrorPage");
                }
                return RedirectToAction("Index");
            }

            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code", student.DeptId);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code", student.DeptId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,Name,Email,ContactNo,Date,Address,DeptId,RegNo")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
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
            
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "Code", student.DeptId);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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

        public JsonResult EmailExists(string email)
        {
            var user = db.Students.Where(x => x.Email == email).FirstOrDefault();
            if (user != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ContactExists(string contact)
        {
            var user = db.Students.Where(x => x.ContactNo == contact).FirstOrDefault();
            if (user != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }


    }
}
