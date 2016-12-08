using IronPython.Hosting;
using Microsoft.CSharp;
using Microsoft.Scripting.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class TestCodeDom
    {
        [TestMethod]
        public CodeMemberMethod test1()
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            CodeMemberMethod method = new CodeMemberMethod();
            method.Name = "GetNextQuestion";
            method.ReturnType = new CodeTypeReference("System.String");
            // CodeCommentStatement comment = new CodeCommentStatement("注释");
            // method.Comments.Add(comment);

            method.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "text"));

            CodeVariableReferenceExpression variableRef1 = new CodeVariableReferenceExpression("context");
            CodeMethodInvokeExpression cs1 = new CodeMethodInvokeExpression(new CodeSnippetExpression("context"), "GetQuestionByID", new CodePrimitiveExpression("Hello World!"));
            method.Statements.Add(cs1);

            CodeMethodReferenceExpression methodRef1 = new CodeMethodReferenceExpression(
                new CodeThisReferenceExpression(),
                "GetQuestionByID", new CodeTypeReference[] {
                    new CodeTypeReference("System.Decimal"),
                    new CodeTypeReference("System.Int32")});
            method.Statements.Add(methodRef1);

            // TODO python 下 return 功能不能实现
            method.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("text")));

            CodeSnippetExpression literalExpression = new CodeSnippetExpression("return text");
            method.Statements.Add(literalExpression);

            ScriptSource source = engine.CreateScriptSource(method);
            Console.WriteLine(source.GetCode());

            // 执行代码
            dynamic actual = source.Execute(scope);
            Func<string, string> fun;
            scope.TryGetVariable("GetNextQuestion", out fun);
            string result = fun("aaa");
           
            return method;
        }

        [TestMethod]
        public CodeCompileUnit test2()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace samples = new CodeNamespace("Samples");
            samples.Imports.Add(new CodeNamespaceImport("System"));
            compileUnit.Namespaces.Add(samples);
            CodeTypeDeclaration class1 = new CodeTypeDeclaration("Class1");
            samples.Types.Add(class1);

            CodeEntryPointMethod start = new CodeEntryPointMethod();
            CodeMethodInvokeExpression cs1 = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("System.Console"), "WriteLine", new CodePrimitiveExpression("Hello World!"));
            // cs1.Parameters.Add(Class2.test1());
            CodeIndexerExpression a = new CodeIndexerExpression();

            start.Statements.Add(cs1);
            class1.Members.Add(start);
            class1.Members.Add(this.test1());

            return compileUnit;
        }

        [TestMethod]
        public void test3()
        {
            this.GenerateCSharpCode(this.test2());
        }

        public CodeCompileUnit test()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace samples = new CodeNamespace("Samples");
            samples.Imports.Add(new CodeNamespaceImport("System"));
            compileUnit.Namespaces.Add(samples);
            CodeTypeDeclaration class1 = new CodeTypeDeclaration("Class1");
            samples.Types.Add(class1);

            CodeEntryPointMethod start = new CodeEntryPointMethod();
            CodeMethodInvokeExpression cs1 = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("System.Console"), "WriteLine", new CodePrimitiveExpression("Hello World!"));
            // cs1.Parameters.Add(Class2.test1());
            CodeIndexerExpression a = new CodeIndexerExpression();

            start.Statements.Add(cs1);
            class1.Members.Add(start);
            return compileUnit;
        }

        public string GenerateCSharpCode(CodeCompileUnit compileunit)
        {
            // Generate the code with the C# code provider.
            CSharpCodeProvider provider = new CSharpCodeProvider();

            // Build the output file name.
            string sourceFile;
            if (provider.FileExtension[0] == '.')
            {
                sourceFile = "HelloWorld" + provider.FileExtension;
            }
            else
            {
                sourceFile = "HelloWorld." + provider.FileExtension;
            }

            // Create a TextWriter to a StreamWriter to the output file.
            using (StreamWriter sw = new StreamWriter(sourceFile, false))
            {
                IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");

                // Generate source code using the code provider.
                provider.GenerateCodeFromCompileUnit(compileunit, tw, new CodeGeneratorOptions());

                // Close the output file.
                tw.Close();
            }

            return sourceFile;
        }

        public bool CompileCSharpCode(string sourceFile, string exeFile)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();

            // Build the parameters for source compilation.
            CompilerParameters cp = new CompilerParameters();

            // Add an assembly reference.
            cp.ReferencedAssemblies.Add("System.dll");

            // Generate an executable instead of
            // a class library.
            cp.GenerateExecutable = true;

            // Set the assembly file name to generate.
            cp.OutputAssembly = exeFile;

            // Save the assembly as a physical file.
            cp.GenerateInMemory = false;

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);
            // provider.CompileAssemblyFromDom(cp,)
            
            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                Console.WriteLine("Errors building {0} into {1}", sourceFile, cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.WriteLine("  {0}", ce.ToString());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Source {0} built into {1} successfully.", sourceFile, cr.PathToAssembly);
            }

            // Return the results of compilation.
            if (cr.Errors.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
