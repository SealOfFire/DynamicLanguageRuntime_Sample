using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitySample.Entity
{
    /// <summary>
    /// 产品
    /// </summary>
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public Guid ProducerId { get; set; }

        /// <summary>
        /// 制造商
        /// </summary>
        [ForeignKey("ProducerId")]
        public virtual Producer Producer { get; set; }

        public Product() { }

        public Product(string name) { this.Name = name; }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("-------------------------------------------------");
            sb.AppendLine(string.Format("id:{0}", this.ProducerId));
            sb.AppendLine(string.Format("name:{0}", this.Name));
            sb.AppendLine("-------------------------------------------------");
            return sb.ToString();
        }
    }
}
