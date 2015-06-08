using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpers;
using PagedList;
using Rent_All_Certificate.Attributes;
using Rent_All_Certificate.Models;

namespace Rent_All_Certificate.Controllers
{
    [LoginValidRole(ValidRoleId = new[] {Roles.TechnicalAdministrator})]
    public class EmployeesController : Controller
    {
        private RentAllUserEntities db = new RentAllUserEntities();

        // GET: Employees
        public ActionResult Index(int? page)
        {
            var employee = db.Employee.Include(e => e.Role);
            return View(employee.ToList().ToPagedList(page ?? 1, Pagenumber.MaxResults));
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Role1");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeNewModel employeeNewModel)
        {
            employeeNewModel.EmployeeModel.PasswordHash = "temp";
            if (ModelState.IsValid)
            {
                employeeNewModel.EmployeeModel.PasswordSalt = HashHelper.CreateSalt();
                var passwordHash = HashHelper.Hash(employeeNewModel.Password.ToCharArray(),
                    employeeNewModel.EmployeeModel.PasswordSalt);
                employeeNewModel.EmployeeModel.PasswordHash = passwordHash;
                db.Employee.Add(employeeNewModel.EmployeeModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Role1", employeeNewModel.EmployeeModel.RoleID);
            return View(employeeNewModel);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employeeEditModel = new EmployeeEditModel
            {
                EmployeeModel = db.Employee.Find(id)
            };
            if (employeeEditModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Role1", employeeEditModel.EmployeeModel.RoleID);
            return View(employeeEditModel);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeEditModel employeeEditModel)
        {
            employeeEditModel.EmployeeModel.PasswordHash = "temp";

            if (ModelState.IsValid)
            {
                if (employeeEditModel.EditPassword != null)
                {
                    db.Entry(employeeEditModel.EmployeeModel).State = EntityState.Modified;
                    employeeEditModel.EmployeeModel.PasswordSalt = HashHelper.CreateSalt();
                    employeeEditModel.EmployeeModel.PasswordHash =
                        HashHelper.Hash(employeeEditModel.EditPassword.ToCharArray(),
                            employeeEditModel.EmployeeModel.PasswordSalt);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    var currentEmployee =
                        db.Employee.FirstOrDefault(e => e.EmployeeID == employeeEditModel.EmployeeModel.EmployeeID);
                    currentEmployee.Email = employeeEditModel.EmployeeModel.Email;
                    currentEmployee.Firstname = employeeEditModel.EmployeeModel.Firstname;
                    currentEmployee.Lastname = employeeEditModel.EmployeeModel.Lastname;
                    currentEmployee.RoleID = employeeEditModel.EmployeeModel.RoleID;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Role1", employeeEditModel.EmployeeModel.RoleID);
            return View(employeeEditModel);
        }

        // GET: Employees/Deactivate/1
 
        public ActionResult Deactivate(int id)
        {
            var employee = db.Employee.Find(id);
            var fired = new Fired
            {
                EmployeeID = employee.EmployeeID
            };
            db.Fired.Add(fired);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Employees/Reactivate/1
        public ActionResult Reactivate(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var fired = db.Fired.Find(id);
            if (fired == null)
            {
                return HttpNotFound();
            }

            db.Fired.Remove(fired);
            db.SaveChanges();
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
    }
}