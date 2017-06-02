using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        //Associa DB
        private ApplicationDbContext _context;
        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing) { _context.Dispose(); }

        //Ações
        public ViewResult Index()
        {
            //var movies = GetMovies();
            var movies = _context.Movies
                                 .Include(m => m.Genre)
                                 .ToList();

            //PERMISSÃO APENAS PARA QUEM POSSUI ATRIBUTO CanManageMovies ativo
            if (User.IsInRole(RoleName.CanManageMovies))
            {
                return View("IndexAutorizado", movies);
            }
            return View(movies);    
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Details(int id)
        {
            //var customer = GetCustomers().SingleOrDefault(c => c.Id == id);
            //Include = associa (FK) a outra tabela necessaria para identificar campo 
            var movies = _context.Movies
                                 .Include(c => c.Genre)
                                 .SingleOrDefault(c => c.Id == id);
            if (movies == null)
                return HttpNotFound();

            return View(movies);
        }

        //MovieForm - Exibe Formulario
        [Authorize(Roles = RoleName.CanManageMovies)] //Restringe Acesso apenas a Usuarios Autorizados
        public ActionResult MovieForm()
        {
            var genre = _context.Genres.ToList();
            //MovieFormViewModel 
            //DbSet<Genre> Genres
            //Agrega&Encapsula 2 tabelas para utilizar em uma mesma View
            var viewModel = new MovieFormViewModel
            {
                Genres = genre
            };
            return View(viewModel);
        }

        //Submissão Formulário OU Atualiza Informações do Customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(Movie movie)
        {
            //se novo -> insere no banco, senão -> atualiza movie selecionado.
            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movie); //Memoria Principal
            }
            else
            {
                var movieInDb = _context.Movies.Single(c => c.Id == movie.Id);
                //TryUpdateModel(customerInDb);  CUIDADO
                //Mapper.Map(customer, customerInDb);  <<--- OLHAR ESSE MODO
                //movieInDb.DateAdded = DateTime.Now;
                movieInDb.Name = movie.Name;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.NumberInStock = movie.NumberInStock;
                movieInDb.GenreId = movie.GenreId;
            }

            try
            {
                _context.SaveChanges(); //Memoria Secundaria (persiste os dados)
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("Index", "Movies");
        }

        //Editar Movie by Id
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            //Agrega&Encapsula 2 tabelas para utilizar em uma mesma View
            var viewModel = new MovieFormViewModel
            {
                Movie = movie,
                Genres = _context.Genres.ToList()
            };

            return View("MovieForm", viewModel);
        }





        /* Passado
        private IEnumerable<Movie> GetMovies()
        {
        return new List<Movie>
                {
                    new Movie { Id = 1, Name = "Shrek" },
                    new Movie { Id = 2, Name = "Wall-e" }
                };
        }


        [Route("movies/released/{year}/{month}")]
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month);
        }


        //===========================================#####
        GET: Movies/Random  --  ActionResult/ViewResult/PartialViewResult/ContentResult/RedirectResult/RedirectToRouteResult/JsonReult/HttpNotFoundResult
        //public ActionResult Random()
        //{
            //filmes
            var movie = new List<Movie>
            {
                new Movie { Name = "O dia depois de amanhã" },
                new Movie { Name = "O sexto sentido" }

            };
            //clientes
            var customers = new List<Customer>
            {
                new Customer {Name = "Fulano 12"},
                new Customer {Name = "Fulano 23"}
            };
            //intercala filmes&clientes
            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };
            

            //return View(viewModel);
            //return Content("olá");
            //return HttpNotFound();
            //return RedirectToAction("Index", "Home", new {page = 1, sortBy = "name"});
            
        }

        // GET: movies/edit/1
        public ActionResult Edit(int id)
        {
            return Content("id = " + id);
        }

        /*movies
        public ActionResult Index(int? pageIndex, string sortBy)
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;
            if (String.IsNullOrWhiteSpace(sortBy))
                sortBy = "Name";

            return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));

        }
        */
    }
}