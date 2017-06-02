using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        //DataAnnotation <label>
        [Display(Name = "Number in Stock")]
        public byte NumberInStock { get; set; }

        //DataAnnotation <label>
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }

        //DataAnnotation <label>
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        //DataAnnotation <label>
        [Display(Name = "Genre")]
        [Required]
        public byte GenreId { get; set; }

        /*Associação tabela Genre->Movie para 
         *resgatar conteúdo e relacionar FK */
        public Genre Genre { get; set; }
    }
}