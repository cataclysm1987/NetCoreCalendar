using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetCoreCalendar.Data;
using DotNetCoreCalendar.Models;
using DotNetCoreCalendar.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using DotNetCoreCalendar.Controllers.ActionFilters;

namespace DotNetCoreCalendar.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _usermanager;

        public EventController(IDAL dal, UserManager<ApplicationUser> usermanager)
        {
            _dal = dal;
            _usermanager = usermanager;
        }

        // GET: Event
        public IActionResult Index()
        {
            if (TempData["Alert"] != null)
            {
                ViewData["Alert"] = TempData["Alert"];
            }
            return View(_dal.GetMyEvents(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        // GET: Event/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _dal.GetEvent((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Event/Create
        
        public IActionResult Create()
        {
            return View(new EventViewModel(_dal.GetLocations(), User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create(EventViewModel vm, IFormCollection form)
        {
            try
            {
                _dal.CreateEvent(form);
                TempData["Alert"] = "Success! You created a new event for: " + form["Event.Name"];
                return RedirectToAction("Index");
            } catch(Exception ex)
            {
                ViewData["Alert"] = "An error occurred: " + ex.Message;
                return View(vm);
            }
        }

        // GET: Event/Edit/5
        [UserAccessOnly]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _dal.GetEvent((int)id);
            if (@event == null)
            {
                return NotFound();
            }
            var vm = new EventViewModel(@event, _dal.GetLocations(), User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(vm);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(int id, IFormCollection form)
        {
            try
            {
                _dal.UpdateEvent(form);
                TempData["Alert"] = "Success! You modified an event for: " + form["Event.Name"];
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Alert"] = "An error occurred: " + ex.Message;
                var vm = new EventViewModel(_dal.GetEvent(id), _dal.GetLocations(), User.FindFirstValue(ClaimTypes.NameIdentifier));
                return View(vm);
            }
        }

        // GET: Event/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var @event = _dal.GetEvent((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _dal.DeleteEvent(id);
            TempData["Alert"] = "You deleted an event.";
            return RedirectToAction(nameof(Index));
        }
    }
}
