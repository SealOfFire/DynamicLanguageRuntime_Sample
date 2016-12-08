using EntitySample;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom;
using System.Reflection;

namespace UnitTest
{
    /// <summary>
    /// 执行脚本测试
    /// </summary>
    [TestClass]
    public class TestScript
    {
        delegate int Add(int p0, int p1);

        /// <summary>
        /// 从脚本文件创建ScriptScope
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            // 创建运行时
            ScriptRuntime runtime = Python.CreateRuntime();
            // 创建 scope（命名空间）
            ScriptScope scope = runtime.ExecuteFile(@"D:\03_MyProgram\GitHub\test\DynamicLanguageRuntime_Sample\PythonCode\PythonCode.py");
            // 从脚本获取运行结果
            Add add;
            scope.TryGetVariable<Add>("add", out add);
            int sum = add(10, 20);
        }

        [TestMethod]
        public void TestMethod2()
        {
            // 创建运行时
            ScriptRuntime runtime = Python.CreateRuntime();
            // 加载类库
            runtime.LoadAssembly(Assembly.Load("EntitySample"));
            // 创建引擎
            ScriptEngine engine = runtime.GetEngine("python");
            // 创建代码
            ScriptSource source = engine.CreateScriptSourceFromString("print 'aa'");

            // 执行代码
            dynamic result1 = source.Execute();
            // 编译代码
            CompiledCode compliedCode = source.Compile();
            dynamic result3 = compliedCode.Execute();
        }

        [TestMethod]
        public void TestMethod3()
        {
            // 创建运行时
            ScriptRuntime runtime = Python.CreateRuntime();
            // 加载命名空间
            runtime.LoadAssembly(Assembly.Load("EntitySample"));
            // 创建命名空间
            ScriptScope scope = runtime.ExecuteFile(@"D:\03_MyProgram\GitHub\test\DynamicLanguageRuntime_Sample\PythonCode\PythonCode.py");
            // 创建引擎
            ScriptEngine engine = runtime.GetEngine("python");
            // 创建代码
            ScriptSource source = engine.CreateScriptSourceFromString(@"
cla = pythonClass()
cla.add(1,2)
");
            // 编译代码
            CompiledCode compliedCode = source.Compile();
            dynamic result4 = compliedCode.Execute(scope);
            // 执行代码
            dynamic result2 = source.Execute(scope);
        }

        /// <summary>
        /// CodeDom创建脚本
        /// </summary>
        [TestMethod]
        public void TestMethod5()
        {
            // 创建引擎
            ScriptEngine engine = Python.CreateEngine();
            CodeMemberMethod method = new CodeMemberMethod();
            method.Name = "aa";
            CodeMethodInvokeExpression cs1 = new CodeMethodInvokeExpression(null, "print", new CodePrimitiveExpression("Hello World!"));
            method.Statements.Add(cs1);
            ScriptSource source = engine.CreateScriptSource(method);
            // 执行代码
            dynamic actual = source.Execute();
        }

        /// <summary>
        /// python 脚本调用.net对象方法
        /// </summary>
        [TestMethod]
        public void TestMethod6()
        {
            // 创建引擎
            ScriptEngine engine = Python.CreateEngine();
            // 创建命名空间
            ScriptScope scope = engine.CreateScope();
            scope.SetVariable("test", new CSharpCode());

            // 创建代码
            ScriptSource source1 = engine.CreateScriptSourceFromString("test.Add(1,2)");
            // 执行代码
            dynamic result1 = source1.Execute(scope);

            ScriptSource source2 = engine.CreateScriptSourceFromString("test.StaticAdd(3,2)");
            // 执行代码
            dynamic result2 = source2.Execute(scope);
        }

        /// <summary>
        /// python 脚本调用.net对象方法
        /// </summary>
        [TestMethod]
        public void TestMethod7()
        {
            // 创建引擎
            ScriptEngine engine = Python.CreateEngine();
            // 创建命名空间
            ScriptScope scope = engine.CreateScope();

            // 创建代码
            ScriptSource source1 = engine.CreateScriptSourceFromString("def a():");
            // 执行代码
            source1.Compile();

        }

    }
}
