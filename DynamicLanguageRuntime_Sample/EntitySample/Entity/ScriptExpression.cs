using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitySample.Entity
{
    /// <summary>
    /// 保存脚本
    /// </summary>
    public class ScriptExpression
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ScriptId { get; set; }
        public string Expression { get; set; }

        public ScriptExpression() { }

        public ScriptExpression(string expression) { this.Expression = expression; }
    }
}
