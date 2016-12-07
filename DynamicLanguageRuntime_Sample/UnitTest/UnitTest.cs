using EntitySample;
using EntitySample.Entity;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            // 在应用程序启动时自动升级数据库结构
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbSampleContext, Configuration>());
        }

        [TestCleanup()]
        public void TestCleanup() { }


        [TestMethod]
        public void TestMethod1()
        {
            using (DbSampleContext context = new DbSampleContext())
            {
                Producer producer = context.Producers.Where(p => p.Name.Contains("intel")).FirstOrDefault();
                if (producer == null)
                    producer = new Producer();

                ScriptRuntime runtime = Python.CreateRuntime();
                ScriptScope scope = runtime.CreateScope();

                scope.SetVariable("context", context);
                scope.SetVariable("producer", producer);
                ScriptEngine engine = runtime.GetEngine("python");
                // 创建代码
                string expression = string.Format("product=producer.FindProductByName(\"i7\")");
                ScriptSource source = engine.CreateScriptSourceFromString(expression);

                // 编译
                CompiledCode compiledCode = source.Compile();

                dynamic result1 = compiledCode.Execute(scope);
                dynamic result2;
                scope.TryGetVariable("product", out result2);

                if (result2 is Product)
                {
                    Console.WriteLine(result2 as Product);
                }
                else
                {
                    Console.WriteLine("error");
                }
            }

        }
    }
}
