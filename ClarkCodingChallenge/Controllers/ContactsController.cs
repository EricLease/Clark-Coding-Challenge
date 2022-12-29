using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClarkCodingChallenge.Models;
using ClarkCodingChallenge.BusinessLogic;

namespace ClarkCodingChallenge.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactsService _service;

        public ContactsController(IContactsService service) => _service = service;

        public IActionResult Index(int id = -1, 
            [FromQuery] bool? saved = null, [FromQuery] int? sort = null, [FromQuery] string filter = null)
        {
            ViewBag.Saved = saved;

            return View(_service.GetContactsViewModel(id, sort ?? 0, filter));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        [HttpPost]
        public IActionResult Save(ContactsViewModel vm)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Saved = _service.SaveContact(vm.Contact);
                
                if (ViewBag.Saved)
                {
                    return RedirectToAction(nameof(Index), 
                        new 
                        { 
                            id = -1, 
                            saved = true, 
                            sort = vm.SortDirection, 
                            filter = vm.Filter 
                        });
                }
            }

            return View(nameof(Index), vm);
        }

        [HttpPost]
        public IActionResult Cancel(ContactsViewModel vm)
            => RedirectToAction(nameof(Index),
                new
                {
                    id = -1,
                    sort = vm.SortDirection,
                    filter = vm.Filter
                });

        [HttpGet]
        public IActionResult List([FromQuery] int? sort = 0, [FromQuery] string filter = null)
            => new JsonResult(_service.GetExistingContacts(sort ?? 0, filter));
    }
}
