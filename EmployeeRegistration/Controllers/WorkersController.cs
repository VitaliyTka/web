using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeRegistration;

namespace EmployeeRegistration.Controllers
{
    public class WorkersController : Controller
    {
        private readonly DBWorkersContext _context;

        public WorkersController(DBWorkersContext context)
        {
            _context = context;
        }

        // GET: Workers
        public async Task<IActionResult> Index()
        {
            var dBWorkersContext = _context.Workers.Include(w => w.PicturePath);
            return View(await dBWorkersContext.ToListAsync());
        }

        // GET: Workers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workers = await _context.Workers
                .Include(w => w.PicturePath)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workers == null)
            {
                return NotFound();
            }

            return View(workers);
        }

        // GET: Workers/Create
        public IActionResult Create()
        {
            ViewData["PicturePathId"] = new SelectList(_context.Picture, "Id", "PicturePath");
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,FirstName,LastName,Patronymic,Email,Salary,PicturePathId")] Workers workers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PicturePathId"] = new SelectList(_context.Picture, "Id", "Id", workers.PicturePathId);
            return View(workers);
        }

        // GET: Workers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workers = await _context.Workers.FindAsync(id);
            if (workers == null)
            {
                return NotFound();
            }
            ViewData["PicturePathId"] = new SelectList(_context.Picture, "Id", "Id", workers.PicturePathId);
            return View(workers);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,FirstName,LastName,Patronymic,Email,Salary,PicturePathId")] Workers workers)
        {
            if (id != workers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkersExists(workers.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PicturePathId"] = new SelectList(_context.Picture, "Id", "Id", workers.PicturePathId);
            return View(workers);
        }

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workers = await _context.Workers
                .Include(w => w.PicturePath)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workers == null)
            {
                return NotFound();
            }

            return View(workers);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workers = await _context.Workers.FindAsync(id);
            _context.Workers.Remove(workers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkersExists(int id)
        {
            return _context.Workers.Any(e => e.Id == id);
        }
    }
}
