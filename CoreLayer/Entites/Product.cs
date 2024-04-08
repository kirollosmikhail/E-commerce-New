using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entites
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandId { get; set; }
        public ProductBrand ProductBrand { get; set; } = new ProductBrand();

        public int ProductTypeId { get; set; }
        public ProductType  ProductType { get; set; } = new ProductType();
    }
}