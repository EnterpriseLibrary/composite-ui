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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Windows.Forms;
using System.ComponentModel;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Diagnostics;

namespace Microsoft.Practices.CompositeUI.Tests.SmartParts
{
	[TestClass]
	public class SmartPartInfoFixture
	{
		[TestMethod]
		public void CanSetTitleAndDesccription()
		{
			SmartPartInfo info = new SmartPartInfo();

			info.Title = "Title";
			info.Description = "Description";

			Assert.AreEqual("Title", info.Title);
			Assert.AreEqual("Description", info.Description);
		}

		public void GenerateCodeForSmartPartInfoCompiles()
		{
			DesignerSerializerAttribute attr = (DesignerSerializerAttribute)Attribute.GetCustomAttribute(
				typeof(SmartPartInfo), typeof(DesignerSerializerAttribute));
			CodeDomSerializer serializer = (CodeDomSerializer)Activator.CreateInstance(Type.GetType(attr.SerializerTypeName));

			UserControl smartPart = new UserControl();

			Container container = new Container();
			SmartPartInfo info1 = new SmartPartInfo("foo", "");
			container.Add(info1, "info1");
			MockSPI info2 = new MockSPI("bar", "");
			container.Add(info2, "info2");

			MockManager manager = new MockManager(container.Components);
			manager.Services.Add(typeof(IDesignerHost), new MockDesignerHost(smartPart, container));
			manager.Services.Add(typeof(IReferenceService), new MockReferenceService(container.Components));
			manager.Services.Add(typeof(IContainer), container);
			manager.Services.Add(typeof(IDesignerSerializationManager), manager);

			CodeTypeDeclaration declaration = new CodeTypeDeclaration("UserControl1");
			CodeMemberMethod init = new CodeMemberMethod();
			init.Name = "InitializeComponent";
			declaration.Members.Add(init);
			// Add empty fields for both SPIs.
			declaration.Members.Add(new CodeMemberField(typeof(ISmartPartInfo), "info1"));
			declaration.Members.Add(new CodeMemberField(typeof(ISmartPartInfo), "info2"));

			manager.Services.Add(typeof(CodeTypeDeclaration), declaration);

			serializer.Serialize(manager, info1);
			serializer.Serialize(manager, info2);

			StringWriter sw = new StringWriter();

			new Microsoft.CSharp.CSharpCodeProvider().GenerateCodeFromType(
				declaration, sw, new CodeGeneratorOptions());

			sw.Flush();

			//Console.WriteLine(sw.ToString());

			CompilerResults results = new Microsoft.CSharp.CSharpCodeProvider().CompileAssemblyFromSource(
				new CompilerParameters(new string[] {
					"System.dll", 
					new Uri(typeof(SmartPartInfo).Assembly.CodeBase).LocalPath}),
					sw.ToString());

			Assert.IsFalse(results.Errors.HasErrors, ErrorsToString(results.Errors));
		}
		
		private static string ErrorsToString(CompilerErrorCollection errors)
		{
			StringBuilder sb = new StringBuilder();
			foreach (CompilerError err in errors)
			{
				sb.AppendLine(err.ToString());
			}

			return sb.ToString();
		}

		#region Helper classes

		class MockSPI : SmartPartInfo
		{
			public MockSPI(string title, string description) : base(title, description) { }
		}

		class MockManager : IDesignerSerializationManager
		{
			public Dictionary<Type, object> Services = new Dictionary<Type, object>();
			ContextStack stack = new ContextStack();
			ComponentCollection components;

			public MockManager(ComponentCollection components)
			{
				this.components = components;
			}

			private void HideCompilerWarningsForUnusedInterfaceMethods()
			{
				ResolveName(null, null);
				SerializationComplete(null, null);
			}

			#region IDesignerSerializationManager Members

			public void AddSerializationProvider(IDesignerSerializationProvider provider)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public ContextStack Context
			{
				get { return stack; }
			}

			public object CreateInstance(Type type, System.Collections.ICollection arguments, string name, bool addToContainer)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public object GetInstance(string name)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public string GetName(object value)
			{
				foreach (IComponent component in components)
				{
					if (components == value)
					{
						return component.Site.Name;
					}
				}

				return Guid.NewGuid().ToString();
			}

			public object GetSerializer(Type objectType, Type serializerType)
			{
				DesignerSerializerAttribute attr = (DesignerSerializerAttribute)Attribute.GetCustomAttribute(
					objectType, typeof(DesignerSerializerAttribute));
				if (attr == null)
				{
					if (objectType == typeof(Component))
					{
						Type t = Type.GetType("System.ComponentModel.Design.Serialization.ComponentCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
						return Activator.CreateInstance(t);
					}
					return null;
				}
				else
				{
					return (CodeDomSerializer)Activator.CreateInstance(Type.GetType(attr.SerializerTypeName));
				}
			}

			public Type GetType(string typeName)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public System.ComponentModel.PropertyDescriptorCollection Properties
			{
				get { return TypeDescriptor.GetProperties(typeof(SmartPartInfo)); }
			}

			public void RemoveSerializationProvider(IDesignerSerializationProvider provider)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void ReportError(object errorInformation)
			{
				Console.WriteLine(errorInformation);
			}

			public event ResolveNameEventHandler ResolveName;

			public event EventHandler SerializationComplete;

			public void SetName(object instance, string name)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion

			#region IServiceProvider Members

			public object GetService(Type serviceType)
			{
				if (!Services.ContainsKey(serviceType))
				{
					Debug.Fail("Requested service not found " + serviceType.ToString());
				}
				object service = null;
				Services.TryGetValue(serviceType, out service);

				return service;
			}

			#endregion
		}

		class MockDesignerHost : IDesignerHost
		{
			IComponent rootComponent;
			IContainer container;

			public MockDesignerHost(IComponent rootComponent, IContainer container)
			{
				this.rootComponent = rootComponent;
				this.container = container;
			}

			private void HideCompilerWarningsForUnusedInterfaceMethods()
			{
				Activated(null, null);
				Deactivated(null, null);
				LoadComplete(null, null);
				TransactionClosed(null, null);
				TransactionClosing(null, null);
				TransactionOpened(null, null);
				TransactionOpening(null, null);
			}

			#region IDesignerHost Members

			public void Activate()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public event EventHandler Activated;

			public System.ComponentModel.IContainer Container
			{
				get { return this.container; }
			}

			public System.ComponentModel.IComponent CreateComponent(Type componentClass, string name)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public System.ComponentModel.IComponent CreateComponent(Type componentClass)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public DesignerTransaction CreateTransaction(string description)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public DesignerTransaction CreateTransaction()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public event EventHandler Deactivated;

			public void DestroyComponent(System.ComponentModel.IComponent component)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public IDesigner GetDesigner(System.ComponentModel.IComponent component)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public Type GetType(string typeName)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public bool InTransaction
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public event EventHandler LoadComplete;

			public bool Loading
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public System.ComponentModel.IComponent RootComponent
			{
				get { return rootComponent; }
			}

			public string RootComponentClassName
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public event DesignerTransactionCloseEventHandler TransactionClosed;

			public event DesignerTransactionCloseEventHandler TransactionClosing;

			public string TransactionDescription
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public event EventHandler TransactionOpened;

			public event EventHandler TransactionOpening;

			#endregion

			#region IServiceContainer Members

			public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void AddService(Type serviceType, ServiceCreatorCallback callback)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void AddService(Type serviceType, object serviceInstance, bool promote)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void AddService(Type serviceType, object serviceInstance)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void RemoveService(Type serviceType, bool promote)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void RemoveService(Type serviceType)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion

			#region IServiceProvider Members

			public object GetService(Type serviceType)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion
		}

		class MockReferenceService : IReferenceService
		{
			ComponentCollection components;

			public MockReferenceService(ComponentCollection components)
			{
				this.components = components;
			}

			#region IReferenceService Members

			public IComponent GetComponent(object reference)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public string GetName(object reference)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public object GetReference(string name)
			{
				return components[name];
			}

			public object[] GetReferences(Type baseType)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public object[] GetReferences()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion
		}

		#endregion

	}
}
