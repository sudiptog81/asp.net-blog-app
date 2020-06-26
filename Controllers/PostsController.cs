using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogMvc.Data;
using BlogMvc.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlogMvc.Controllers
{
  [Authorize]
  public class PostsController : Controller
  {
    private readonly ApplicationDbContext _db;

    public PostsController(ApplicationDbContext db)
    {
      _db = db;
    }

    // GET: Posts
    public async Task<IActionResult> Index()
    {
      return View(await _db.Post.ToListAsync());
    }

    // GET: Posts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var post = await _db.Post
        .FirstOrDefaultAsync(m => m.Id == id);
      if (post == null)
      {
        return NotFound();
      }

      return View(post);
    }

    // GET: Posts/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Posts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
      [Bind("Id,Title,Content,CreatedAt")] Post post)
    {
      if (ModelState.IsValid)
      {
        post.CreatedAt = DateTime.Now;
        _db.Add(post);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      return View(post);
    }

    // GET: Posts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var post = await _db.Post.FindAsync(id);
      if (post == null)
      {
        return NotFound();
      }

      return View(post);
    }

    // POST: Posts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
      [Bind("Id,Title,Content,CreatedAt")] Post post)
    {
      if (id != post.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          var postFromDb = await _db.Post.FindAsync(id);
          if (postFromDb == null)
          {
            return NotFound();
          }

          postFromDb.Title = post.Title;
          postFromDb.Content = post.Content;

          _db.Update(postFromDb);
          await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!PostExists(post.Id))
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

      return View(post);
    }

    // GET: Posts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var post = await _db.Post.FindAsync(id);
      if (post == null)
      {
        return NotFound();
      }

      return View(post);
    }

    // POST: Posts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var post = await _db.Post.FindAsync(id);
      _db.Post.Remove(post);
      await _db.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool PostExists(int id)
    {
      return _db.Post.Any(e => e.Id == id);
    }
  }
}