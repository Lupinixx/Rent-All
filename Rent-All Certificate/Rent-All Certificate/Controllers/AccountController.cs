using System;
using System.Linq;
using System.Web.Mvc;
using Helpers;
using Rent_All_Certificate.Attributes;
using Rent_All_Certificate.Models;

namespace Rent_All_Certificate.Controllers
{
    [LoginInvalid]
    public class AccountController : Controller
    {
        private RentAllEntities db = new RentAllEntities();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Employee model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var employee = db.Employee.SingleOrDefault(emp => emp.Email == model.Email);
                if (employee != null)
                {
                    var passwordHash = new HashHelper().Hash(model.PasswordHash.ToCharArray(), employee.PasswordSalt);

                    if (passwordHash.Equals(employee.PasswordHash))
                    {
                        Session["EmployeeID"] = employee.EmployeeID;
                        Session["RoleID"] = employee.RoleID;
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                ModelState.AddModelError("", "Your email address and/or password are incorrect.");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "User was not found.");
            }

            return View(model);
        }
    }
}