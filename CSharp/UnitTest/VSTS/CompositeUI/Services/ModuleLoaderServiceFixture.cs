//===============================================================================
// Microsoft patterns & practices
// CompositeUI Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#endif

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using Microsoft.Practices.CompositeUI.Configuration;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI.Utility;

namespace Microsoft.Practices.CompositeUI.Tests.Services
{
	[TestClass]
	public class ModuleLoaderServiceFixture
	{
		#region private fields

		private static string moduleTemplate =
			@"using System;
            using System.ComponentModel;
            using Microsoft.Practices.CompositeUI;
            using System.Diagnostics;

            #module#
            #dependencies#

            namespace TestModules
            {
	            public class #className#Class : ModuleInit
	            {
                    public override void AddServices()
                    {
                        Trace.Write(""#className#.AddServices"");
                        Console.WriteLine(""#className#.AddServices"");
                    }

                    public override void Load()
                    {
                        Trace.Write(""#className#.Start"");
                        Console.WriteLine(""#className#.Start"");
                    }
                }
            }";

		private static Dictionary<string, Assembly> generatedAssemblies = new Dictionary<string, Assembly>();

		#endregion

		static ModuleLoaderServiceFixture()
		{
			generatedAssemblies.Add("ModuleExposingServices",
					CompileFile(
						"Microsoft.Practices.CompositeUI.Tests.Mocks.Src.ModuleExposingServices.cs",
						@".\ModuleExposingServices1\ModuleExposingServices.dll"));

			generatedAssemblies.Add("ModuleExposingSameServices",
					CompileFile(
						"Microsoft.Practices.CompositeUI.Tests.Mocks.Src.ModuleExposingSameServices.cs",
						@".\ModuleExposingSameServices\ModuleExposingSameServices.dll"));

			CompileFile(@"Microsoft.Practices.CompositeUI.Tests.Mocks.Src.ModuleReferencedAssembly.cs",
						@".\ModuleReferencingAssembly\ModuleReferencedAssembly.dll");

			generatedAssemblies.Add("ModuleReferencingAssembly",
					CompileFile(
						"Microsoft.Practices.CompositeUI.Tests.Mocks.Src.ModuleReferencingAssembly.cs",
						@".\ModuleReferencingAssembly\ModuleReferencingAssembly.dll",
						@".\ModuleReferencingAssembly\ModuleReferencedAssembly.dll"));

			generatedAssemblies.Add("ModuleThrowingException",
					CompileFile(
						"Microsoft.Practices.CompositeUI.Tests.Mocks.Src.ModuleThrowingException.cs",
						@".\ModuleThrowingException\ModuleThrowingException.dll"));

			generatedAssemblies.Add("ModuleExposingOnlyServices",
					CompileFile(
						"Microsoft.Practices.CompositeUI.Tests.Mocks.Src.ModuleExposingOnlyServices.cs",
						@".\ModuleExposingOnlyServices\ModuleExposingOnlyServices.dll"));

			generatedAssemblies.Add("ModuleExposingDuplicatedServices",
					CompileFile(
						"Microsoft.Practices.CompositeUI.Tests.Mocks.Src.ModuleExposingDuplicatedServices.cs",
						@".\ModuleExposingDuplicatedServices\ModuleExposingDuplicatedServices.dll"));

			generatedAssemblies.Add("ModuleDependency1",
					CompileFile("Microsoft.Practices.CompositeUI.Tests.Mocks.Src.ModuleDependency1.cs",
						@".\ModuleDependency1\ModuleDependency1.dll"));

			generatedAssemblies.Add("ModuleDependency2",
					CompileFile("Microsoft.Practices.CompositeUI.Tests.Mocks.Src.ModuleDependency2.cs",
						@".\ModuleDependency2\ModuleDependency2.dll"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullWorkItemThrows()
		{
			ModuleLoaderService loader = new ModuleLoaderService(null);
			loader.Load(null, new MockModuleInfo());
		}

		[TestMethod]
		[ExpectedException(typeof(ModuleLoadException))]
		public void InitializationExceptionsAreWrapped()
		{
			WorkItem mockContainer = new TestableRootWorkItem();
			ModuleLoaderService loader = new ModuleLoaderService(null);

			loader.Load(mockContainer,
					new ModuleInfo(generatedAssemblies["ModuleThrowingException"].CodeBase.Replace(@"file:///", "")));
		}

		[TestMethod]
		public void LoadSampleModule()
		{
			WorkItem container = new TestableRootWorkItem();
			IModuleLoaderService loader = new ModuleLoaderService(null);
			container.Services.Add(typeof(IModuleLoaderService), loader);
			int containerCount = GetItemCount(container);

			ModuleInfo info = new ModuleInfo();
			info.SetAssemblyFile(GenerateDynamicModule("SampleModule", "SampleModule"));

			TextWriter consoleOut = Console.Out;
			StringBuilder sb = new StringBuilder();
			Console.SetOut(new StringWriter(sb));

			loader.Load(container, info);

			Assert.AreEqual(1, GetItemCount(container) - containerCount);

			bool foundUs = false;

			foreach (KeyValuePair<string, object> pair in container.Items)
			{
				if (pair.Value.GetType().FullName == "TestModules.SampleModuleClass")
				{
					foundUs = true;
					break;
				}
			}

			Assert.IsTrue(foundUs);

			Console.SetOut(consoleOut);
		}

		private int GetItemCount(WorkItem container)
		{
			int count = 0;
			foreach (object item in container.Items)
			{
				count++;
			}
			return count;
		}

		[TestMethod]
		[ExpectedException(typeof(ModuleLoadException))]
		public void LoadModuleReferencingMissingAssembly()
		{
			WorkItem mockContainer = new TestableRootWorkItem();
			ModuleLoaderService loader = new ModuleLoaderService(null);

			ModuleInfo info = new ModuleInfo();
			info.SetAssemblyFile(generatedAssemblies["ModuleReferencingAssembly"].CodeBase.Replace(@"file:///", ""));

			loader.Load(mockContainer, info);
		}

		[TestMethod]
		public void LoadProfileWithAcyclicModuleDependencies()
		{
			List<string> assemblies = new List<string>();

			// Create several modules with this dependency graph (X->Y meaning Y depends on X)
			// a->b, b->c, b->d, c->e, d->e, f
			assemblies.Add(GenerateDynamicModule("ModuleA", "ModuleA"));
			assemblies.Add(GenerateDynamicModule("ModuleB", "ModuleB", "ModuleA"));
			assemblies.Add(GenerateDynamicModule("ModuleC", "ModuleC", "ModuleB"));
			assemblies.Add(GenerateDynamicModule("ModuleD", "ModuleD", "ModuleB"));
			assemblies.Add(GenerateDynamicModule("ModuleE", "ModuleE", "ModuleC", "ModuleD"));
			assemblies.Add(GenerateDynamicModule("ModuleF", "ModuleF"));

			ModuleInfo[] Modules = new ModuleInfo[assemblies.Count];
			for (int i = 0; i < assemblies.Count; i++)
			{
				Modules[i] = new ModuleInfo(assemblies[i]);
			}

			TextWriter consoleOut = Console.Out;

			StringBuilder sb = new StringBuilder();
			Console.SetOut(new StringWriter(sb));
			WorkItem mockContainer = new TestableRootWorkItem();
			ModuleLoaderService loader = new ModuleLoaderService(null);
			loader.Load(mockContainer, Modules);

			List<string> trace =
				new List<string>(sb.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
			Assert.AreEqual(12, trace.Count);
			Assert.IsTrue(trace.IndexOf("ModuleE.AddServices") > trace.IndexOf("ModuleC.AddServices"),
							  "ModuleC must precede ModuleE");
			Assert.IsTrue(trace.IndexOf("ModuleE.AddServices") > trace.IndexOf("ModuleD.AddServices"),
							  "ModuleD must precede ModuleE");
			Assert.IsTrue(trace.IndexOf("ModuleD.AddServices") > trace.IndexOf("ModuleB.AddServices"),
							  "ModuleB must precede ModuleD");
			Assert.IsTrue(trace.IndexOf("ModuleC.AddServices") > trace.IndexOf("ModuleB.AddServices"),
							  "ModuleB must precede ModuleC");
			Assert.IsTrue(trace.IndexOf("ModuleB.AddServices") > trace.IndexOf("ModuleA.AddServices"),
							  "ModuleA must precede ModuleB");
			Assert.IsTrue(trace.Contains("ModuleF.AddServices"), "ModuleF must be loaded");
			Console.SetOut(consoleOut);
		}

		[TestMethod]
		[ExpectedException(typeof(CyclicDependencyFoundException))]
		public void FailWhenLoadingModulesWithCyclicDependencies()
		{
			List<string> assemblies = new List<string>();

			// Create several modules with this dependency graph (X->Y meaning Y depends on X)
			// 1->2, 2->3, 3->4, 4->5, 4->2
			assemblies.Add(GenerateDynamicModule("Module1", "Module1"));
			assemblies.Add(GenerateDynamicModule("Module2", "Module2", "Module1", "Module4"));
			assemblies.Add(GenerateDynamicModule("Module3", "Module3", "Module2"));
			assemblies.Add(GenerateDynamicModule("Module4", "Module4", "Module3"));
			assemblies.Add(GenerateDynamicModule("Module5", "Module5", "Module4"));

			ModuleInfo[] modules = new ModuleInfo[assemblies.Count];
			for (int i = 0; i < assemblies.Count; i++)
			{
				modules[i] = new ModuleInfo(assemblies[i]);
			}
			WorkItem mockContainer = new TestableRootWorkItem();
			ModuleLoaderService loader = new ModuleLoaderService(null);
			loader.Load(mockContainer, modules);
		}

		[TestMethod]
		[ExpectedException(typeof(ModuleLoadException))]
		public void FailWhenDependingOnMissingModule()
		{
			ModuleInfo module = new ModuleInfo(GenerateDynamicModule("ModuleK", null, "ModuleL"));

			WorkItem mockContainer = new TestableRootWorkItem();
			ModuleLoaderService loader = new ModuleLoaderService(null);
			loader.Load(mockContainer, module);
		}

		[TestMethod]
		public void CanLoadAnonymousModulesWithDepedencies()
		{
			List<string> assemblies = new List<string>();

			// Create several modules with this dependency graph (X->Y meaning Y depends on X)
			// a->b, b->c, b->d, c->e, d->e, f
			assemblies.Add(GenerateDynamicModule("ModuleX", "ModuleX"));
			assemblies.Add(GenerateDynamicModule("ModuleY", null, "ModuleX"));
			assemblies.Add(GenerateDynamicModule("ModuleP", "ModuleP"));
			assemblies.Add(GenerateDynamicModule("ModuleQ", null, "ModuleP"));

			ModuleInfo[] modules = new ModuleInfo[assemblies.Count];
			for (int i = 0; i < assemblies.Count; i++)
			{
				modules[i] = new ModuleInfo(assemblies[i]);
			}

			TextWriter consoleOut = Console.Out;

			StringBuilder sb = new StringBuilder();
			Console.SetOut(new StringWriter(sb));
			WorkItem mockContainer = new TestableRootWorkItem();
			ModuleLoaderService loader = new ModuleLoaderService(null);
			loader.Load(mockContainer, modules);

			List<string> trace =
				new List<string>(sb.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
			Assert.AreEqual(8, trace.Count);
			Assert.IsTrue(trace.IndexOf("ModuleX.AddServices") < trace.IndexOf("ModuleY.AddServices"),
							  "ModuleX must precede ModuleY");
			Assert.IsTrue(trace.IndexOf("ModuleP.AddServices") < trace.IndexOf("ModuleQ.AddServices"),
							  "ModuleP must precede ModuleQ");
			Console.SetOut(consoleOut);

		}

		[TestMethod]
		[ExpectedException(typeof(ModuleLoadException))]
		public void ThrowsIfAssemblyNotRelativeSolutionProfile()
		{
			WorkItem container = new TestableRootWorkItem();
			ModuleLoaderService service = new ModuleLoaderService(null);
			service.Load(container, new ModuleInfo(@"C:\module.dll"));
		}

		[TestMethod]
		[ExpectedException(typeof(ModuleLoadException))]
		public void ThrowsIfAssemblyRelativeNotUnderRootSolutionProfile()
		{
			WorkItem container = new TestableRootWorkItem();
			ModuleLoaderService service = new ModuleLoaderService(null);
			service.Load(container, new ModuleInfo(@"..\..\module.dll"));
		}

		[TestMethod]
		public void LoadModuleWithServices()
		{
			Assembly compiledAssembly = generatedAssemblies["ModuleExposingServices"];
			WorkItem container = new TestableRootWorkItem();
			ModuleLoaderService service = new ModuleLoaderService(null);
			ModuleInfo info = new ModuleInfo(compiledAssembly.CodeBase.Replace(@"file:///", ""));

			service.Load(container, info);

			Assert.IsNotNull(container.Services.Get(compiledAssembly.GetType("ModuleExposingServices.SimpleService")));
			Assert.IsNotNull(container.Services.Get(compiledAssembly.GetType("ModuleExposingServices.ITestService")));
		}

		[TestMethod]
		[ExpectedException(typeof(ModuleLoadException))]
		public void ModuleAddingDuplicatedServices()
		{
			Assembly moduleService = generatedAssemblies["ModuleExposingDuplicatedServices"];

			ModuleInfo module = new ModuleInfo(moduleService.CodeBase.Replace(@"file:///", ""));

			WorkItem container = new TestableRootWorkItem();
			ModuleLoaderService service = new ModuleLoaderService(null);
			service.Load(container, module);
		}

		[TestMethod]
		public void ServicesCanBeAddedOnDemand()
		{
			Assembly asm = generatedAssemblies["ModuleExposingServices"];
			ModuleInfo module = new ModuleInfo(asm.CodeBase.Replace(@"file:///", ""));

			WorkItem container = new TestableRootWorkItem();
			ModuleLoaderService service = new ModuleLoaderService(null);
			service.Load(container, module);

			Type typeOnDemand = asm.GetType("ModuleExposingServices.OnDemandService");
			FieldInfo fldInfo = typeOnDemand.GetField("ServiceCreated");
			Assert.IsFalse((bool)fldInfo.GetValue(null), "The service was created.");

			container.Services.Get(typeOnDemand);

			Assert.IsTrue((bool)fldInfo.GetValue(null), "The service was not created.");
		}

		[TestMethod]
		public void CanLoadModuleAssemblyWhichOnlyExposesServices()
		{
			Assembly asm = generatedAssemblies["ModuleExposingOnlyServices"];
			WorkItem container = new TestableRootWorkItem();
			ModuleLoaderService service = new ModuleLoaderService(null);
			service.Load(container, new ModuleInfo(asm.CodeBase.Replace(@"file:///", "")));

			Type typeSimpleService = asm.GetType("ModuleExposingOnlyServices.SimpleService");
			Type typeITestService = asm.GetType("ModuleExposingOnlyServices.ITestService");
			Assert.IsNotNull(container.Services.Get(typeSimpleService), "The SimpleService service was not loaded.");
			Assert.IsNotNull(container.Services.Get(typeITestService), "The ITestService service was not loaded.");
		}

		[TestMethod]
		public void CanLoadDependentModulesWithoutInitialization()
		{
			WorkItem container = new TestableRootWorkItem();
			ModuleLoaderService service = new ModuleLoaderService(null);
			service.Load(container,
							 new ModuleInfo(generatedAssemblies["ModuleDependency2"].CodeBase.Replace(@"file:///", "")),
							 new ModuleInfo(generatedAssemblies["ModuleDependency1"].CodeBase.Replace(@"file:///", "")));
		}

		[TestMethod]
		public void CanGetModuleMetaDataFromAssembly()
		{
			Assembly asm = generatedAssemblies["ModuleExposingOnlyServices"];
			ModuleLoaderService service = new ModuleLoaderService(null);
			WorkItem wi = new TestableRootWorkItem();

			bool wasAdded = false;

			wi.Services.Added += delegate(object sender, DataEventArgs<object> e)
			{
				if (e.Data.GetType().Name == "TestService")
					wasAdded = true;
			};

			service.Load(wi, asm);

			Assert.IsTrue(wasAdded);
		}

		[TestMethod]
		public void CanEnumerateLoadedModules()
		{
			Assembly compiledAssembly1 = generatedAssemblies["ModuleDependency1"];
			Assembly compiledAssembly2 = generatedAssemblies["ModuleDependency2"];
			WorkItem wi = new TestableRootWorkItem();
			ModuleLoaderService service = new ModuleLoaderService(null);
			service.Load(wi, compiledAssembly1);
			service.Load(wi, compiledAssembly2);

			Assert.AreEqual(2, service.LoadedModules.Count);

			Assert.AreSame(compiledAssembly1, service.LoadedModules[0].Assembly);
			Assert.AreEqual("module1", service.LoadedModules[0].Name);
			Assert.AreEqual(0, service.LoadedModules[0].Dependencies.Count);

			Assert.AreSame(compiledAssembly2, service.LoadedModules[1].Assembly);
			Assert.AreEqual("module2", service.LoadedModules[1].Name);
			Assert.AreEqual(1, service.LoadedModules[1].Dependencies.Count);
			Assert.AreEqual("module1", service.LoadedModules[1].Dependencies[0]);
		}

		[TestMethod]
		public void CanBeNotifiedOfAddedModules()
		{
			WorkItem wi = new TestableRootWorkItem();
			IModuleLoaderService svc = new ModuleLoaderService(null);
			LoadedModuleInfo lmi = null;
			Assembly assm = generatedAssemblies["ModuleDependency1"];

			svc.ModuleLoaded += delegate(object sender, DataEventArgs<LoadedModuleInfo> e)
			{
				lmi = e.Data;
			};

			svc.Load(wi, assm);

			Assert.IsNotNull(lmi);
			Assert.AreSame(assm, lmi.Assembly);
		}

		class MockApplication : CabApplication<MockWorkItem>
		{
			protected override void Start()
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		class MockWorkItem : WorkItem
		{
		}

		class MockModuleInfo : IModuleInfo
		{
			public string AssemblyFile
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public string UpdateLocation
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public IList<string> AllowedRoles
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}
		}

		#region Helper methods

		public static string GenerateDynamicModule(string assemblyName, string moduleName, params string[] dependencies)
		{
			string assemblyFile = assemblyName + ".dll";
			if (!Directory.Exists(assemblyName))
			{
				Directory.CreateDirectory(assemblyName);
			}
			string outpath = Path.Combine(assemblyName, assemblyFile);
			if (File.Exists(outpath))
			{
				File.Delete(outpath);
			}

			// Create temporary module.
			string moduleCode = moduleTemplate.Replace("#className#", assemblyName);
			if (moduleName != null && moduleName.Length > 0)
			{
				moduleCode = moduleCode.Replace("#module#", @"[assembly: Module(""" + moduleName + @""")]");
			}
			else
			{
				moduleCode = moduleCode.Replace("#module#", "");
			}

			string depString = String.Empty;
			foreach (string module in dependencies)
			{
				depString += String.Format("[assembly: ModuleDependency(\"{0}\")]\r\n", module);
			}
			moduleCode = moduleCode.Replace("#dependencies#", depString);

			CompileCode(moduleCode, outpath);

			return outpath;
		}


		public static Assembly CompileFile(string input, string output, params string[] references)
		{
			CreateOutput(output);

			List<string> referencedAssemblies = new List<string>(references.Length + 3);

			referencedAssemblies.AddRange(references);
			referencedAssemblies.Add("System.dll");
			referencedAssemblies.Add(typeof(IModule).Assembly.CodeBase.Replace(@"file:///", ""));

			CSharpCodeProvider codeProvider = new CSharpCodeProvider();
			CompilerParameters cp = new CompilerParameters(referencedAssemblies.ToArray(), output);

			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(input))
			{
				if (stream == null)
				{
					throw new ArgumentException("input");
				}

				StreamReader reader = new StreamReader(stream);
				string source = reader.ReadToEnd();
				CompilerResults results = codeProvider.CompileAssemblyFromSource(cp, source);
				ThrowIfCompilerError(results);
				return results.CompiledAssembly;
			}
		}

		public static Assembly CompileCode(string code, string output)
		{
			CreateOutput(output);
			CodeCompileUnit unit = new CodeSnippetCompileUnit(code);
			unit.ReferencedAssemblies.Add("System");
			unit.ReferencedAssemblies.Add("Microsoft.Practices.CompositeUI");

			CompilerResults results = new CSharpCodeProvider().CompileAssemblyFromSource(
				new CompilerParameters(
					new string[] { "System.dll", typeof(IModule).Assembly.CodeBase.Replace(@"file:///", "") },
					output), code);

			ThrowIfCompilerError(results);

			return results.CompiledAssembly;
		}

		public static void CreateOutput(string output)
		{
			string dir = Path.GetDirectoryName(output);
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
		}

		public static void ThrowIfCompilerError(CompilerResults results)
		{
			if (results.Errors.HasErrors)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Compilation failed.");
				foreach (CompilerError error in results.Errors)
				{
					sb.AppendLine(error.ToString());
				}
				Assert.IsFalse(results.Errors.HasErrors, sb.ToString());
			}
		}

		#endregion
	}
}
