using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using Microsoft.Scripting.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnitTest
{
    /// <summary>
    /// ash 表达式树测试
    /// </summary>
    [TestClass]
    public class TestExpression
    {
        /// <summary>
        /// 获取 表达式树
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            ScriptRuntime runtime = Python.CreateRuntime();
            ScriptEngine engine = runtime.GetEngine("python");
            ScriptSource source = engine.CreateScriptSourceFromString("print 'aa'");

            LanguageContext languageContext = HostingHelpers.GetLanguageContext(engine);
            SourceUnit sourceUnit = HostingHelpers.GetSourceUnit(source);
            CompilerContext compilerContext = new CompilerContext(sourceUnit, new IronPython.Compiler.PythonCompilerOptions(), ErrorSink.Null);
            IronPython.Compiler.Parser parser = IronPython.Compiler.Parser.CreateParser(compilerContext, new IronPython.PythonOptions());
            IronPython.Compiler.Ast.PythonAst ast1 = parser.ParseFile(true); // 表达式树

            //Expression<Action> le = Expression.Lambda<Action>(ast1);
            //Action act = le.Compile();
        }

        /// <summary>
        /// linq 执行表达式树
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            // Creating a parameter expression.
            ParameterExpression value = Expression.Parameter(typeof(int), "value");

            // Creating an expression to hold a local variable. 
            ParameterExpression result = Expression.Parameter(typeof(int), "result");

            // Creating a label to jump to from a loop.
            LabelTarget label = Expression.Label(typeof(int));

            // Creating a method body.
            BlockExpression block = Expression.Block(
                // Adding a local variable.
                new[] { result },
                // Assigning a constant to a local variable: result = 1
                Expression.Assign(result, Expression.Constant(1)),
                    // Adding a loop.
                    Expression.Loop(
                       // Adding a conditional block into the loop.
                       Expression.IfThenElse(
                           // Condition: value > 1
                           Expression.GreaterThan(value, Expression.Constant(1)),
                           // If true: result *= value --
                           Expression.MultiplyAssign(result,
                               Expression.PostDecrementAssign(value)),
                           // If false, exit the loop and go to the label.
                           Expression.Break(label, result)
                       ),
                   // Label to jump to.
                   label
                )
            );

            // Compile and execute an expression tree.
            Expression<Func<int, int>> le = Expression.Lambda<Func<int, int>>(block, value);
            Func<int, int> fun = le.Compile();
            int a = fun(5);
            int factorial = Expression.Lambda<Func<int, int>>(block, value).Compile()(5);
            Console.WriteLine(factorial);
        }

        /// <summary>
        /// linq 执行表达式树
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            Expression exp = Expression.Add(Expression.Constant(0), Expression.Constant(1));
            Expression<Func<int>> exp2 = Expression.Lambda<Func<int>>(exp);
            Func<int> func1 = exp2.Compile();
            int result1 = func1();

            // The expression tree to execute.
            BinaryExpression be = Expression.Power(Expression.Constant(2D), Expression.Constant(3D));

            // Create a lambda expression.
            Expression<Func<double>> le = Expression.Lambda<Func<double>>(be);

            // Compile the lambda expression.
            Func<double> compiledExpression = le.Compile();

            //// 其他信息: 无法序列化以下委托: 非托管函数指针的委托、动态方法的委托或位于委托创建者程序集之外的方法的委托。
            //// 序列化测试
            //BinaryFormatter b = new BinaryFormatter();
            //FileStream fileStream1 = new FileStream(@"d:\temp.dat", FileMode.Create);
            //b.Serialize(fileStream1, compiledExpression);
            //fileStream1.Close();
            //// 反序列化
            //Func<double> fun;
            //FileStream fileStream2 = new FileStream(@"d:\temp.dat", FileMode.Open, FileAccess.Read, FileShare.Read);
            //fun = b.Deserialize(fileStream2) as Func<double>;
            //fileStream2.Close();

            // Execute the lambda expression.
            double result = compiledExpression();

            // Display the result.
            Console.WriteLine(result);

            // This code produces the following output:
            // 8
        }

        [TestMethod]
        public void TestMethod4()
        {

        }
    }
}
