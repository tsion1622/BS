using BM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BM.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public TeacherController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        private List<Teacher> GetTeachers()
        {
            List<Teacher> Teachers = new List<Teacher>();
            Teachers.Add(new Teacher

            {
                Id = 1,
                FirstName = "lema",
                MiddelName = "megersa",
                Age = 34
            });
            Teachers.Add(new Teacher
            {
                Id = 2,
                FirstName = "lomi",
                MiddelName = "tadesse",
                Age = 44
            });
            return Teachers;

        }
        public IActionResult Index()
        {
           
           
          List<Teacher> Teachers = GetTeachers();
            ViewBag.TotalTeachers = Teachers.Count;

            return View( Teachers.ToList());
        }

        public IActionResult Details(int? id)
        {
            Teacher teacher = GetTeachers().FirstOrDefault(x=>x.Id==id);
            return View(teacher);
        }


      
    }
}
