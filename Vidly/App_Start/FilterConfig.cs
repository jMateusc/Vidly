using System.Web;
using System.Web.Mvc;

namespace Vidly
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //Atributo adicionado para limitar acesso global a seções/conteúdos da página.
            filters.Add(new AuthorizeAttribute());
        }
    }
}
