using Microsoft.AspNetCore.Mvc;
using BM.Models;

namespace BM.Controllers
{
    public class EmployeesController : Controller
    {
        public List<Employees> GetEmployee() {

            List<Employees> listofemployees = new List<Employees>();
            listofemployees.Add(new()
            {
                Id = 1,
                FirstName = "Abebe",
                MiddelName = "Takele",
                Email = "abe@gmail.com",
                UserName = "Test",
            });

            listofemployees.Add(new()
            {
                Id = 1,
                FirstName = "Sisay",
                MiddelName = "Molla",
                Email = "sis@gmail.com",
                UserName = "Test",
            });

            listofemployees.Add(new()
            {
                Id = 1,
                FirstName = "Haymanot",
                MiddelName = "Beba",
                Email = "haymi@gmail.com",
                UserName = "Test",
            });
            return listofemployees;
        }
        public IActionResult Index()
        {
            List<Employees> listofemployees = GetEmployee();
            return View(listofemployees.ToList());
      
        
        }
   public IActionResult Details(int id)
        {
            Employees employee = GetEmployee().FirstOrDefault(x=>x.Id==id);
            return View(employee);
        }
    
    
    
    }



}
