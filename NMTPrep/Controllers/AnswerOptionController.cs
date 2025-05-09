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
    public class AnswerOptionController : Controller
    {
        private readonly DbNMTContext _context;

        public AnswerOptionController(DbNMTContext context)
        {
            _context = context;
        }

        // GET: AnswerOptions
        public async Task<IActionResult> Index()
        {
            var dbNMTContext = _context.AnswerOptions.Include(a => a.Question);
            return View(await dbNMTContext.ToListAsync());
        }

        // GET: AnswerOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerOption = await _context.AnswerOptions
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answerOption == null)
            {
                return NotFound();
            }

            return View(answerOption);
        }

        // GET: AnswerOptions/Create
        public IActionResult Create()
        {
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Hint");
            return View();
        }

        // POST: AnswerOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestionId,OptionText,IsCorrect")] AnswerOption answerOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(answerOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Hint", answerOption.QuestionId);
            return View(answerOption);
        }

        // GET: AnswerOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerOption = await _context.AnswerOptions.FindAsync(id);
            if (answerOption == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Hint", answerOption.QuestionId);
            return View(answerOption);
        }

        // POST: AnswerOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuestionId,OptionText,IsCorrect")] AnswerOption answerOption)
        {
            if (id != answerOption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answerOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerOptionExists(answerOption.Id))
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
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Hint", answerOption.QuestionId);
            return View(answerOption);
        }

        // GET: AnswerOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerOption = await _context.AnswerOptions
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answerOption == null)
            {
                return NotFound();
            }

            return View(answerOption);
        }

        // POST: AnswerOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answerOption = await _context.AnswerOptions.FindAsync(id);
            if (answerOption != null)
            {
                _context.AnswerOptions.Remove(answerOption);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswerOptionExists(int id)
        {
            return _context.AnswerOptions.Any(e => e.Id == id);
        }
    }
}
