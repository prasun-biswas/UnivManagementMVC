using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityMnagementSystemMVC.Models
{
    public class CourseInformatonViewModel
    {
        public List<Department> Departments { get; set; }
        public List<Course> Courses { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<CourseAssignToTeacher> CourseAssignToTeachers { get; set; }
    }
    
}