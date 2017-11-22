using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MvcMovie.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ReviewList
    {
        public SelectList reviewSelects;
        public Movie movie { get; set; }
        public List<Review> reviews { get; set; }

    }
}
