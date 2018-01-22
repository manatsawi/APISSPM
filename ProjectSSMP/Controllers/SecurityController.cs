using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectSSMP.Models;

namespace ProjectSSMP.Controllers
{
    public class SecurityController : Controller
    {
        private readonly sspmContext _context;


        public SecurityController(sspmContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        //public async Task<IActionResult> Login(LgoinInputModel inputModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    if (!validateuser(inputModel.Username, inputModel.Password))
        //    {
        //        ModelState.AddModelError("","");
        //        return View();
        //    }
        //    //List<Claim> claims = new List<Claim>
        //    //{

        //    //}

        //}

        [HttpPost]
        public JsonResult Authen(string user, string pass)
        {
            string membercode = string.Empty;
            if (!validateuser(user, pass))
            {
                return Json(new { success = false, msg = "ไม่สามารถดึงข้อมูลได้ กรุณาทำรายการใหม่" });

            }
            return Json(new { success = true,msg = "user : " + user + " pass : " + pass });
            

        }

        private bool validateuser(string user , string pass)
        {
            var userid = (from u in _context.UserSspm where u.Username.Equals(user) select u).FirstOrDefault();
            if (userid == null)
                return false;
            if(userid.Password != pass)
            {
                return false;
            }
            return true;
        }

        


    }
}