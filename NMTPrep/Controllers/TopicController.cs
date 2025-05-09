using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NMT.Models;

namespace NMT.Controllers
{
    public class TopicController : Controller
    {
        private readonly DbNMTContext _context;

        public TopicController(DbNMTContext context)
        {
            _context = context;
        }

        // GET: Topics
        public async Task<IActionResult> Index()
        {
            return View(await _context.Topics.ToListAsync());
        }

        // GET: Topics/Details/5
        public IActionResult Details(int id)
        {
            var topic = _context.Topics
                .Include(t => t.Section)
                .FirstOrDefault(t => t.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // GET: Topics/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Sections = new SelectList(_context.Sections.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Topic topic)
        {
            if (ModelState.IsValid)
            {
                _context.Topics.Add(topic);
                _context.SaveChanges();
                return RedirectToAction("Index", "Section");
            }

            ViewBag.Sections = new SelectList(_context.Sections.ToList(), "Id", "Name");
            return View(topic);
        }

        // GET: Topic/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null) return NotFound();

            ViewBag.Sections = new SelectList(_context.Sections, "Id", "Name", topic.SectionId);
            return View(topic);
        }

        // POST: Topic/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Topic model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Sections = new SelectList(_context.Sections, "Id", "Name", model.SectionId);
                return View(model);
            }

            var t = await _context.Topics.FindAsync(model.Id);
            if (t == null) return NotFound();

            t.Name = model.Name;
            t.SectionId = model.SectionId;
            t.TheoryHtml = model.TheoryHtml;

            _context.Update(t);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Section");
        }


        // GET: Topics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic != null)
            {
                _context.Topics.Remove(topic);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopicExists(int id)
        {
            return _context.Topics.Any(e => e.Id == id);
        }
    }
}
