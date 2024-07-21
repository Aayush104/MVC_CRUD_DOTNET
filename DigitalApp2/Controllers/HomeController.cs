using DigitalApp2.DataSecurity;
using DigitalApp2.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DigitalApp2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DigitalApp1Context _context;
        private readonly IDataProtector _dataProtector;


        private readonly IWebHostEnvironment _env;
        public HomeController(ILogger<HomeController> logger, DigitalApp1Context context, Security security, IWebHostEnvironment env, IDataProtectionProvider provider)
        {
            _logger = logger;
            _context = context;
            _env = env;
            _dataProtector = provider.CreateProtector(security.datakey);
        }

        public IActionResult  Index()
        {

           

          return View();    


        }




        public IActionResult GetUsers()
        {
            var User = _context.Users.Select(e => new UserEdit
            {
                UserId = e.UserId,
                UserName = e.UserName,
                UserPassword = e.UserPassword,
                UserProfile = e.UserProfile,
                EmailAddress = e.EmailAddress,
                EncId = _dataProtector.Protect(e.UserId.ToString()),

            }).ToList();
            return PartialView("_GetUsers",User);


        }
        //params bata string ma aaerako huxna id excrypt vayera teslai hami int decrypt garxau  ani int ma change garxau
        public IActionResult Details(string id)
        {

            int userid = Convert.ToInt32(_dataProtector.Unprotect(id));
            var User = _context.Users.Where(x => x.UserId == userid).FirstOrDefault();
            return View(User);  

        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }  
        [HttpPost]
        public IActionResult Create(UserEdit edit)
        {
            int maxid;

            if (_context.Users.Any())
            {
                maxid = _context.Users.Max(x => x.UserId) + 1;
            }
            else
            {
                maxid = 1;
            }

            edit.UserId = maxid;

            if (edit.UserFile != null)
            {
                string filename = Guid.NewGuid().ToString()+ Path.GetExtension(edit.UserFile.FileName);
                string filepath = Path.Combine(_env.WebRootPath, "Photos", filename);  
                using (FileStream str = new FileStream(filepath, FileMode.Create))
                {
                    edit.UserFile.CopyTo(str);
                }

                edit.UserProfile = filename;
            }

            User u = new()
            {
                UserId = edit.UserId,
                UserName = edit.UserName,
                EmailAddress = edit.EmailAddress,
                UserPassword = edit.UserPassword,
                UserProfile = edit.UserProfile, 
                UserStatus = true,

            };


            if(u != null)
            {
                _context.Users.Add(u);
                _context.SaveChanges();
                return Content("Success");
            }
            else
            {
                return Json("Failed");
            }


           

        }


        [HttpGet]
        public IActionResult Edit (string id)
        {
            int userid = Convert.ToInt32(_dataProtector.Unprotect(id));

            var user = _context.Users.Where(x => x.UserId == userid).FirstOrDefault();

            UserEdit e = new()
            {
                UserId = user.UserId,
                UserName = user.UserName,   
                EmailAddress = user.EmailAddress,   
                UserPassword = user.UserPassword,   
                UserProfile = user.UserProfile, 
                UserStatus = true,
            };

            ViewData["psw"] = user.UserPassword;
            return View(e);
        }


        [HttpPost]

        public IActionResult Edit(UserEdit e)
        {

            if (e.UserFile != null)
            {

                string filename = "UpdatedImage" + Guid.NewGuid().ToString() + Path.GetExtension(e.UserFile.FileName);
                string filepath = Path.Combine(_env.WebRootPath, "Photos", filename);
                using (FileStream str = new FileStream(filepath, FileMode.Create))
                {
                    e.UserFile.CopyTo(str);
                }

                e.UserProfile = filename;


            }

            User u = new()
            {
                UserId = e.UserId,
                UserName = e.UserName,
                EmailAddress = e.EmailAddress,
                UserPassword = e.UserPassword,
                UserProfile = e.UserProfile,
                UserStatus = true,
            };

            _context.Users.Update(u);
            _context.SaveChanges(); 
            return Content("Success");
        }



        [HttpGet]

        public IActionResult Delete(int id)
        {
            var user = _context.Users.Where(x => x.UserId == id).First();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return Content("Success");

        }

    }


}
