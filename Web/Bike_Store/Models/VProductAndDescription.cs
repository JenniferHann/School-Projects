using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NoNameBikes.Models
{
    public partial class VProductAndDescription
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ProductModel { get; set; }
        public int ProductModelId { get; set; }
        public DateTime? SellEndDate { get; set; }
        public int ProductCategoryId { get; set; }
        public string Culture { get; set; }
        public string Description { get; set; }
    }
}
