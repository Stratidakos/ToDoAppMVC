﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAppMVC.Data;
using Task = ToDoAppMVC.Models.Task;

namespace ToDoAppMVC.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Task Task { get; set; }
        public TasksController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Upsert(int? id)
        {
            Task = new Task();
            if (id == null)
            {
                //create
                Task.Status = "New";
                Task.Username = User.Identity.Name;
                return View(Task);
            }
            //update
            Task = _db.Tasks.FirstOrDefault(l => l.Id == id);
            if (Task == null)
            {
                return NotFound();
            }

            return View(Task);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Task.Id == 0)
                {
                    //create
                    Task.Status = "New";
                    Task.Username = User.Identity.Name;
                    _db.Tasks.Add(Task);
                }
                else
                {
                    _db.Tasks.Update(Task);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Task);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Json(new { data = await _db.Tasks.Where(l => l.Username == User.Identity.Name).ToListAsync() });
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var taskFromDb = await _db.Tasks.FirstOrDefaultAsync(u => u.Id == id);
            if (taskFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Tasks.Remove(taskFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}