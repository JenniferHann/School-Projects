using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoNameBikes.Models
{
    public class AccessoriesListModel
    {
        public string ProductModel { get; set; }

        public string Description { get; set; }

        public int ProductModelID { get; set; }

        public decimal Price { get; set; }

        public Byte[] Photo { get; set; }

        public DateTime StartSellDate { get; set; }

        public virtual decimal? SalePrice { get; set; }

        public virtual DateTime? SaleStartDate { get; set; }

        public virtual DateTime? SaleEndDate { get; set; }
    }
}
