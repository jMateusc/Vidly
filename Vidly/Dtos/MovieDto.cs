using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        //DataAnnotation <label>

        public byte NumberInStock { get; set; }

        //DataAnnotation <label>

        public DateTime DateAdded { get; set; }

        //DataAnnotation <label>

        public DateTime ReleaseDate { get; set; }

        //DataAnnotation <label>

        [Required]
        public byte GenreId { get; set; }
    }
}