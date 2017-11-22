using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class ReviewController : Controller
    {
        private readonly MvcMovieContext _context;

        public ReviewController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Review
        public async Task<IActionResult> Index()
        {
            var mvcMovieContext = _context.Review.Include(r => r.Movie);
            return View(await mvcMovieContext.ToListAsync());
        }

        // GET: Review/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Movie)
                .SingleOrDefaultAsync(m => m.ReviewID == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }
        [Authorize] 
        // GET: Review/Create
        public IActionResult Create(int? id)
        {
            var movies = from m in _context.Movie
                        select m;

            foreach (var item in movies)
            {
                if (item.ID == id)
                {
                    ViewData["movieTitle"] = item.Title;
                    ViewData["movieID"] = id;
                }
            }
            return View();

        }
        [Authorize]
        // POST: Review/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewID,Detail,Name,MovieID")] Review review, int? id)
        {
            if (ModelState.IsValid)
            {
                review.MovieID = (int) id;

                ViewData["movieID"] = review.MovieID;
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Movies",  new{ id = review.MovieID } );
            }
            //ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "Title", review.MovieID);
            return View(review);
        }
        [Authorize]

        // GET: Review/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review.SingleOrDefaultAsync(m => m.ReviewID == id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["movieID"] = review.MovieID;


            var movies = from m in _context.Movie
                         select m;

            foreach (var item in movies)
            {
                if (item.ID == review.MovieID)
                {
                    Console.WriteLine("EDIT ITEM ID: " + item.ID);
                    Console.WriteLine("EDIT MOVIE ID: " + review.MovieID);
                    Console.WriteLine("EDIT TITLE: " + item.Title);
                    ViewData["moviewTitle"] = item.Title;
                }
            }

            return View(review);
        }

        [Authorize]

        // POST: Review/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewID,Detail,Name,MovieID")] Review review)
        {
            if (id != review.ReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Movies", new { id = review.MovieID });
            }

            return View(review);
        }
        [Authorize]

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .SingleOrDefaultAsync(m => m.ReviewID == id);

            if (review == null)
            {
                return NotFound();
            }
            else
            {
                var movies = from m in _context.Movie
                             select m;

                foreach (var item in movies)
                {
                    if (item.ID == review.MovieID)
                    {
                        ViewData["movieTitle"] = item.Title;
                        ViewData["movieID"] = item.ID;
                    }
                }
            }

            return View(review);
        }
        [Authorize]

        // POST: Review/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Review.SingleOrDefaultAsync(m => m.ReviewID == id);
            _context.Review.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Movies", new { id = review.MovieID });
        }

        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.ReviewID == id);
        }
    }
}
