using EntitySample;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace DLRSample
{
    public class ScriptSample
    {
        /// <summary>
        /// 脚本运行时
        /// </summary>
        public ScriptRuntime pythonScriptRuntime;

        /// <summary>
        /// 脚本命名空间，可以多个
        /// 设置或获取脚本中的数据
        /// </summary>
        public ScriptScope scope;

        /// <summary>
        /// 执行脚本的引擎
        /// </summary>
        public ScriptEngine pythonScriptEngine;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ScriptSample()
        {
            this.Initialize();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            // 初始化脚本运行时
            this.pythonScriptRuntime = Python.CreateRuntime();

            // 初始化脚本引擎 方法1
            this.pythonScriptEngine = Python.GetEngine(this.pythonScriptRuntime);
            // 初始化脚本引擎 方法2
            this.pythonScriptEngine = this.pythonScriptRuntime.GetEngine("python");

            // 初始化scope
            this.scope = this.pythonScriptRuntime.CreateScope();
        }

        public void SetDbContextVariable(object dbSampleContext)
        {
            this.scope.SetVariable("DbContext", dbSampleContext);
        }

        //public void InitializeScriptScope()
        //{
        //    // this.scope = this.pythonScriptRuntime.ExecuteFile(@"D:\03_MyProgram\GitHub\test\DynamicLanguageRuntime_Sample\PythonCode\PythonCode.py");

        //    // this.scope.SetVariable("DbSampleContext", dbSampleContext);
        //    // this.scope.SetVariable("p2", 20);
        //    //Add add;
        //    //scope.TryGetVariable<Add>("Add", out add);
        //    //int sum = add(10, 20);
        //}

        public int Add1(int p0, int p1)
        {

            return 0;
        }
    }
}
