using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EntitySample.Entity
{
    /// <summary>
    /// 生产商
    /// </summary>
    public class Producer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProducerId { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public virtual ICollection<Product> Products { get; set; }

        public Producer() { }

        public Producer(string name) { this.Name = name; }

        /// <summary>
        /// 通过ID获取商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Product FindProductById(string productId)
        {
            Guid id = Guid.Empty;
            Guid.TryParse(productId, out id);
            if (this.Products != null)
                return this.Products.Where(products => products.ProducerId == id).FirstOrDefault();
            return null;
        }

        /// <summary>
        /// 通过ID获取商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Product FindProductByName(string name)
        {
            return this.Products.Where(products => products.Name.Contains(name)).FirstOrDefault();
        }
    }
}
