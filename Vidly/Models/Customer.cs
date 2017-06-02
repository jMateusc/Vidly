using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }

        //DataAnnotation 
        [Required(ErrorMessage = "O campo Nome deve ser obrigatoriamente preenchido.")]
        [StringLength(255)]
        public string Name { get; set; }

        public bool IsSubscribedToNewsletter { get; set; }

        //DataAnnotation <label> e  Validação Customizada [Min18YearsIfMember]    
        [Display(Name = "Date of Birthdate")]
        [Min18YearsIfMember]
        public DateTime? Birthdate { get; set; }

        //DataAnnotation <label>
        //[Required(ErrorMessage = "É obrigatório selecionar um item da lista.")]
        [Display(Name = "Membership Type")]
        public byte MembershipTypeId { get; set; }

        /*Associação tabela MembershipType->Customer para 
         *resgatar conteúdo e relacionar FK */
        public MembershipType MembershipType { get; set; }
    }
}