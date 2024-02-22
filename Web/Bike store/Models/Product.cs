using NoNameBikes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoNameBikes.Models
{
    public partial class Product
    {
        public Product()
        {
            SalesOrderDetail = new HashSet<SalesOrderDetail>();
        }

        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Please enter a valid color")]
        public string Color { get; set; }
        public decimal StandardCost { get; set; }
        
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Please enter a valid price.")]
        public decimal ListPrice { get; set; }

        public string Size { get; set; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Please enter a valid weight.")]
        public decimal? Weight { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? ProductModelId { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Please enter a valid date.")]
        public DateTime SellStartDate { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Please enter a valid date.")]
        public DateTime? SellEndDate { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Please enter a valid date.")]
        public DateTime? DiscontinuedDate { get; set; }

        public byte[] ThumbNailPhoto { get; set; }
        public string ThumbnailPhotoFileName { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ProductModel ProductModel { get; set; }
        public virtual ICollection<SalesOrderDetail> SalesOrderDetail { get; set; }

        public virtual decimal? SalePrice { get; set; }
        public virtual DateTime? SaleStartDate { get; set; }
        public virtual DateTime? SaleEndDate { get; set; }
    }
}
