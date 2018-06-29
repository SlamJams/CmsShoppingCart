using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Models.Data
{
    [Table("tblCategories")]
    public class CategoryDTO
    {
        [Key]
        public int Id { get; set; }

        public String Name { get; set; }
        public String Slug { get; set; }
        public int Sorting { get; set; }
    }
}