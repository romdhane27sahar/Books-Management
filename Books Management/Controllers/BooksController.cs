
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Books_Management.Models;

namespace Books_Management.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookManagementDbContext _context;

        public BooksController(BookManagementDbContext context)
        {
            _context = context;
        }
        /******************************* recherche  et filtrage******************/
        
       
        public IActionResult searchByTitle(string title)
        {

            var books = _context.Books.ToList();
            if (title == null) return View(books);
            var res = from m in books
                      where m.Title.ToUpper().Contains(title.ToUpper())
                      select m;
            //methode 2
            var res2 = _context.Books.Where(m => m.Title.ToUpper().Contains(title.ToUpper()));


            return View(res.ToList());

        }
        /********************************/

        // searchByGenre
        public IActionResult searchByGenre(string genre)
        {

            var books = _context.Books.ToList();
            if (genre == null) return View(books);
            var res = from m in books
                      where m.Genre.ToUpper().Contains(genre.ToUpper())
                      select m;
             

            return View(res.ToList());

        }
        /*******************************/
        //search selon un critere choisi 

        public IActionResult searchByCritere(string value, string critere)
        {


            var books = _context.Books.ToList();
            if (value == null) return View(books);//retourner view contenant tous les movies 
            if (critere == "Title")
            {
                var res = from m in books
                          where m.Title.ToUpper().Contains(value.ToUpper())
                          select m;
                return View(res.ToList());
            }

            var res2 = from m in books
                       where m.Genre.ToUpper().Contains(value.ToUpper())
                       select m;

            return View(res2.ToList());

        }
        /************************************/
        //filtrer par 2

        public IActionResult filtrerPar2()

        {
            var books = _context.Books.ToList();
            //selectionner genre=> parcourir lise des books et pour chaque book afficher son genre
           
            //envoyer la liste au view
            ViewBag.GL = books.Select(m => m.Genre).ToList(); //viewbag est un objet .gl est property de viewbag. on l'a crée pour en mettre  la liste des genres et l'envoyer à la vue

            return View(books);
        }

        [HttpPost] 
        public IActionResult filtrerPar2 (string genre, string title)
        {
            var books = _context.Books.AsQueryable(); 
            ViewBag.GL = books.Select(m => m.Genre).Distinct().ToList();
            if (genre != "All")
            {
                if (!String.IsNullOrEmpty(title))
                {

                    books = books.Where(m => m.Title.ToUpper().Contains(title.ToUpper()));

                }
                books = books.Where(m => m.Genre == genre);

            }

            if (!String.IsNullOrEmpty(title))
            {
                books = books.Where(m => m.Title == title || m.Title.ToUpper().Contains(title.ToUpper()));

            }
            return View("filtrerPar2", books.ToList());
        }

        /*****************Afficher toutes les methodes de filtrage dans 1 seule view *************/
        public IActionResult SearchBooks()
        {
            return View();
        }







        // GET: Books
        public async Task<IActionResult> Index()
        {
            var bookManagementDbContext = _context.Books.Include(b => b.Author);
            return View(await bookManagementDbContext.ToListAsync());
        }




        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.IdB == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "IdA", "FullName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdB,Title,Description,Genre,AuthorId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "IdA", "FullName", book.AuthorId);
            return View(book);
        }




        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "IdA", "FullName", book.AuthorId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdB,Title,Description,Genre,AuthorId")] Book book)
        {
            if (id != book.IdB)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.IdB))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "IdA", "FullName", book.AuthorId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.IdB == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookManagementDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.IdB == id)).GetValueOrDefault();
        }
    }
}
