using Microsoft.AspNetCore.Mvc;
using MvcCoreLinqToSql.Models;
using MvcCoreLinqToSql.Repositories;

namespace MvcCoreLinqToSql.Controllers
{
    public class EnfermosController : Controller
    {
        RepositoryEnfermos repo;

        public EnfermosController()
        {
            this.repo = new RepositoryEnfermos();
        }

        public IActionResult Index()
        {
            List<Enfermo> enfermos = this.repo.GetEnfermos();
            return View(enfermos);
        }

        public IActionResult Details(string id)
        {
            Enfermo enf = this.repo.FindEnfermo(id);
            return View(enf);
        }

        public IActionResult Delete(string id)
        {
            this.repo.DeleteEnfermo(id);
            return RedirectToAction("Index");
        }

    }
}
