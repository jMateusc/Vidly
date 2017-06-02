using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        //Associa DB
        private ApplicationDbContext _context;      //db.
        public CustomersController()
        {
            _context = new ApplicationDbContext();  //inicializa conexão db via construtor
        }

        protected override void Dispose(bool disposing){_context.Dispose();}  //famoso clear

        //Ações           

        //[Authorize]  %2F = /
        public ViewResult Index()
        {
            var customers = _context.Customers
                                    .Include(c => c.MembershipType)
                                    .ToList();

            //ALTERAR/CRIAR ATRIBUTO CanManageCustomers para refletir Domain-Model utilizado
            if (User.IsInRole(RoleName.CanManageMovies))
            {
                return View("IndexAutorizado", customers);
            }
           return View(customers);
        }
        
        [Authorize(Roles = RoleName.CanManageMovies)] //ALTERAR <->CanManageCustomers 
        public ActionResult Details(int id)
        {
                //var customer = GetCustomers().SingleOrDefault(c => c.Id == id);
                //Include = associa (FK) a outra tabela necessaria para identificar campo 
                var customer = _context.Customers
                                       .Include(c => c.MembershipType)
                                       .SingleOrDefault(c => c.Id == id);
                if (customer == null)
                    return HttpNotFound();
    
               return View(customer);
        }


        //CustomerForm - Exibe Formulario
        [Authorize(Roles = RoleName.CanManageMovies)] //ALTERAR <->CanManageCustomers
        public ActionResult CustomerForm()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            //Agrega&Encapsula 2 tabelas para utilizar em uma mesma View
            var viewModel = new CustomerFormViewModel
            {
                //Corrije o erro de não gerar uma id=0
                Customer =  new Customer(),
                MembershipTypes = membershipTypes
            };
            return View(viewModel);
        }


        //Submissão Formulário OU Atualiza Informações do Customer  - após submissão retorna para view CustomerForm acima
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(Customer customer)
        {

            //Validando Entradas server-side  <->  ModelState.IsValid
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };
                return View("CustomerForm", viewModel);
            }


            //Se novo -> insere no banco, senão -> atualiza customer selecionado.
            if (customer.Id == 0)
            {
                _context.Customers.Add(customer); //Memoria Principal
            }
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);
                //TryUpdateModel(customerInDb);  CUIDADO
                //Mapper.Map(customer, customerInDb);  <<--- OLHAR ESSE MODO
                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            }
            
            _context.SaveChanges();            //Memoria Secundaria (persiste os dados)
            return RedirectToAction("Index", "Customers");
        }        


        //Editar Customer by Id
        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            
            //Agrega&Encapsula 2 tabelas para utilizar em uma mesma View
            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };

            return View("CustomerForm", viewModel);
        }


        //RELER ERRO NO DELETE
        public ActionResult Delete(int id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            
            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();

            return View("Index");
        }
    }
}