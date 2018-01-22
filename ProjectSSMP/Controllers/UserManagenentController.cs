using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectSSMP.Models;
using ProjectSSMP.Models.UserManagement;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectSSMP.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly sspmContext _context;

        public UserManagementController(sspmContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(AddUserInputModel inputModel)
        {
            try
            {
                //var loggedInUser = HttpContext.User;
                //var loggedInUserName = loggedInUser.Identity.Name;

                var id = (from u in _context.RunningNumber where u.Type.Equals("UserID") select u).FirstOrDefault();

                int userid;
                if (id.Number == null)
                {
                    userid = 100001;

                }
                else
                {
                    userid = Convert.ToInt32(id.Number);
                    userid = userid + 1;
                }

                UserSspm ord = new UserSspm
                {
                    UserId = userid.ToString(),
                    Username = inputModel.Username,
                    Password = inputModel.Password,
                    Firstname = inputModel.Firstname,
                    Lastname = inputModel.Lastname,
                    JobResponsible = inputModel.JobResponsible,
                    UserCreateBy = "Test",
                    UserCreateDate = DateTime.Now,



                };

                UserAssignGroup ord2 = new UserAssignGroup
                {
                    UserId = userid.ToString(),
                    GroupId = inputModel.GroupID,

                };

                // Add the new object to the Orders collection.
                _context.UserSspm.Add(ord);
                await _context.SaveChangesAsync();
                _context.UserAssignGroup.Add(ord2);
                await _context.SaveChangesAsync();

                var query = from num in _context.RunningNumber
                                           where num.Type.Equals("UserID") 
                                                select num;

                foreach (RunningNumber RunUserID in query)
                {
                    RunUserID.Number = userid.ToString();

                }

                // Submit the changes to the database.
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                }

                return RedirectToAction("Index", "Home");



            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return View();
            }
        }
    }
}
