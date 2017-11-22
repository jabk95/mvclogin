using System;
using System.ComponentModel.DataAnnotations;


namespace MvcMovie.Models
{
    public class Review
    {
        public int ReviewID { get; set; }

        [Display(Name = "Comment")]
        [Required(ErrorMessage = "Detail is required")]
        public string Detail { get; set; }

        [Display(Name = "Reviewed By")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Movie")]

        public int MovieID { get; set; }
        public Movie Movie { get; set; }
    }

}