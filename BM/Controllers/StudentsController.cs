using Microsoft.AspNetCore.Mvc;
using BM.Models;

namespace BM.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Index()
        {
     List<Students> listofstudent= new List<Students>();
            listofstudent.Add(
                new()
                {
               Id = 1, 
               Name = "tsi",
               FatherName = "Lidu"
                });

            listofstudent.Add(
               new()
               {
                   Id = 2,
                   Name = "kal",
                   FatherName = "nnn"
               });
            listofstudent.Add(
               new()
               {
                   Id = 3,
                   Name = "yutre",
                   FatherName = "bxjqs"
               });

           
            //student.Id = 1;
            //student.Name = "Tsion";
            //student.FatherName = "tamirat";
            //ViewBag.sss = "Welcome";
            //};
            //Students student = new Students();
            //Students student2 = new();
            //{ 
            //student.Id = 2;
            //student.Name = "kal";
            //student.FatherName = "lidu";
            //};
            //listOfStudent
            return View(listofstudent);
           
           
        }
        //private IActionResult GetStudent() {

        //    Students students1 = new Students();
        //    students1.Id = 1;
        //    students1.Name = "ddf";
        //    students1.FatherName = "efff";
        //    ViewBag.stud = students1;
        //    return View(students1);
        //}
    }
}
