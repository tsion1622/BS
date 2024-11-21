using BM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult SignUpx()
        {
            return View();
        }
        public IActionResult Say( string id)
        {
            ViewBag.Say = $"Hello {id}";
            //return $"Hello {id}";
            var data = "DessaLRHN";
            return View("Say",data);
        }
        private int incre(int id)
        {
            return id *10;
        }
        [HttpPost]
        public IActionResult addResult( int a, int b)
        {
            ViewBag.aditton = add(a, b);
            return View();
        }
        public IActionResult Increment(int id , int a, int b)
        {
           //ViewBag.mul = $"The answer is {incre(id)}";
            ViewBag.add = $"The answer is {add(a, b)}";
            //return $"Hello {id}";
            //int incr =  id*2;
            //var inc = $" incr  {incr}";
            return View("Increment", $" {add(a, b)}");
        }
        private int add(int a, int b) 
            {
            return a + b;
            }
        

        public IActionResult LogInx()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
