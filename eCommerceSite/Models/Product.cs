using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    /// <summary>
    /// A salable product
    /// </summary>
    public class Product
    {
        [Key] //Primary key marker for database
        public int ProductId { get; set; }

        /// <summary>
        /// Consumer facing name of the product
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Retail value of the product in USD
        /// USD ($)
        /// </summary>
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        /// <summary>
        /// category the product falls under
        /// electronics, furniture, etc...
        /// </summary>
        public string Category { get; set; }


    }
}
