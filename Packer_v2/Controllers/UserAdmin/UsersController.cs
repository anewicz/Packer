using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Packer_v2.Context;
using Packer_v2.Models;
using Packer_v2.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Packer_v2.Controllers
{
    public class UsersController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: Users
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var usersView = new List<UserView>();

            foreach (var user in users)
            {
                var userView = new UserView
                {
                    Email = user.Email,
                    Name = user.UserName,
                    IdUser = user.Id
                };

                usersView.Add(userView);

            }

            return View(usersView);
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