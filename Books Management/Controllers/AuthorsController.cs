using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Books_Management.Models;

namespace Books_Management.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly BookManagementDbContext _context;

        public AuthorsController(BookManagementDbContext context)
        {
            _context = context;
        }

        /******************************* recherche  et filtrage******************/


        public IActionResult searchByFullName(string fullname)
        {

            var authors = _context.Authors.ToList();
            if (fullname == null) return View(authors);
            var res = from m in authors
                      where m.FullName.ToUpper().Contains(fullname.ToUpper())
                      select m;
            //methode 2
            var res2 = _context.Authors.Where(m => m.FullName.ToUpper().Contains(fullname.ToUpper()));


            return View(res.ToList());

        }
        /********************************/

        // searchByNatinality
        public IActionResult searchByNationality(string nationality)
        {

            var authors = _context.Authors.ToList();
            if (nationality == null) return View(authors);
            var res = from m in authors
                      where m.Nationality.ToUpper().Contains(nationality.ToUpper())
                      select m;
            //methode 2
            var res2 = _context.Authors.Where(m => m.Nationality.ToUpper().Contains(nationality.ToUpper()));


            return View(res.ToList());

        }
        /*******************************/
        //search selon un critere choisi 

        public IActionResult searchByCritere(string value, string critere)
        {


            var authors = _context.Authors.ToList();
            if (value == null) return View(authors);
            if (critere == "FullName")
            {
                var res = from m in authors
                          where m.FullName.ToUpper().Contains(value.ToUpper())
                          select m;
                return View(res.ToList());
            }

            var res2 = from m in authors
                       where m.Nationality.ToUpper().Contains(value.ToUpper())
                       select m;

            return View(res2.ToList());

        }


        /************************************/
        //filtrer par 2

        public IActionResult filtrerPar2()

        {
            var authors = _context.Authors.ToList();
            //selectionner genre=> parcourir lise des books et pour chaque book afficher son genre

            //envoyer la liste au view
            ViewBag.GL = authors.Select(m => m.Nationality).ToList(); //viewbag est un objet .gl est property de viewbag. on l'a crée pour en mettre  la liste des genres et l'envoyer à la vue

            return View(authors);
        }

        [HttpPost]
        public IActionResult filtrerPar2(string nationality, string firstName, string lastName)
        {
            var authors = _context.Authors.AsQueryable();
            ViewBag.GL = authors.Select(m => m.Nationality).Distinct().ToList();
            if (nationality != "All")
            {
                if (!String.IsNullOrEmpty(firstName))
                {

                    authors = authors.Where(m => m.FirstName.ToUpper().Contains(firstName.ToUpper()));

                }
                authors = authors.Where(m => m.Nationality == nationality);

                if (!String.IsNullOrEmpty(lastName))
                {

                    authors = authors.Where(m => m.LastName == lastName || m.LastName.ToUpper().Contains(lastName.ToUpper()));

                }
                authors = authors.Where(m => m.Nationality == nationality);

            }

            if (!String.IsNullOrEmpty(firstName))
            {
                authors = authors.Where(m => m.FirstName.ToUpper().Contains(firstName.ToUpper()));
            }

			if (!String.IsNullOrEmpty(lastName))
			{
				authors = authors.Where(m => m.LastName.ToUpper().Contains(lastName.ToUpper()));
			}
			return View("filtrerPar2", authors.ToList());
        }

        /*****************Afficher toutes les methodes de filtrage dans 1 seule view *************/
        public IActionResult SearchAuthors()
        {
            return View();
        }



        // GET: Authors
        public async Task<IActionResult> Index()
        {
              return _context.Authors != null ? 
                          View(await _context.Authors.ToListAsync()) :
                          Problem("Entity set 'BookManagementDbContext.Authors'  is null.");
        }


        /*public IActionResult MyBooks(int id)
        {
            return View(_context.Books.Where(m => m.AuthorId == id).ToList());


        }*/

        //jointure en utilisant les proprietés de navigation (j'ai utilisé 2 facons et les 2 fonctionnelles)

                //jointure facon 1 (appellée au niveau de la vue de index.cshtml de authors)
                public IActionResult MyBooks(int id)
                {
                    var author = _context.Authors.Find(id);//chercher l'auteur dont l'id correspond à celui fourni en paramètres  

                    //Si l'auteur n'est pas trouvé la méthode renvoie une réponse HTTP 404 (Not Found).
                    if (author == null)
                    {
                        return NotFound();
                    }

                    //si auteur trouvé, on récupère son nom et prenom(fullName )
                       // ViewBag.AuthorName = $"{author.FirstName}{author.LastName}'s Books";
                        ViewBag.AuthorName = $"{author.FullName}'s Books";

                    //récupérer tous les livres dans la base de données qui ont l'identifiant d'auteur correspondant à celui fourni en paramètre. 
                    var books = _context.Books
                        .Where(m => m.AuthorId == id)
                        .ToList();

                    return View(books);
                }

                //jointure facon 2
                public async Task<IActionResult> MyBooks2()
                {
                    var bookManagementDbContext = _context.Books.Include(m => m.Author);//Include pour associer les données de l'auteur associé à chaque livre à ce livre parcouru
                    return View(await bookManagementDbContext.ToListAsync());
                }









        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.IdA == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdA,FirstName,LastName,Nationality,Email")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdA,FirstName,LastName,Nationality,Email")] Author author)
        {
            if (id != author.IdA)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.IdA))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.IdA == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'BookManagementDbContext.Authors'  is null.");
            }
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
          return (_context.Authors?.Any(e => e.IdA == id)).GetValueOrDefault();
        }
    }
}
