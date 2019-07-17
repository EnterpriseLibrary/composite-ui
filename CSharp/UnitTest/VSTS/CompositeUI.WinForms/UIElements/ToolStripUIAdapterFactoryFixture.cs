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
using Microsoft.Practices.CompositeUI.WinForms.UIElements;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.UIElements;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests.UIElements
{
	[TestClass]
	public class ToolStripUIAdapterFactoryFixture
	{
		[TestMethod]
		public void FactorySupportsCorrectTypes()
		{
			ToolStripUIAdapterFactory factory = new ToolStripUIAdapterFactory();
			ToolStrip strip = new ToolStrip();

			Assert.IsTrue(factory.Supports(new ToolStrip()));
			Assert.IsTrue(factory.Supports(new MenuStrip()));
			Assert.IsTrue(factory.Supports(new ToolStripButton())); // Derived from ToolStripItem
			Assert.IsTrue(factory.Supports(new ToolStripMenuItem()));
			Assert.IsTrue(factory.Supports(new ToolStripDropDownMenu()));
			Assert.IsTrue(factory.Supports(strip.Items));
		}

		[TestMethod]
		public void FactoryCreatesCorrectAdapterForToolStrip()
		{
			ToolStripUIAdapterFactory factory = new ToolStripUIAdapterFactory();
			IUIElementAdapter adapter = factory.GetAdapter(new ToolStrip());
			Assert.IsTrue(adapter is ToolStripItemCollectionUIAdapter);
		}

		[TestMethod]
		public void FactoryCreatesCorrectAdapterForMenuStrip()
		{
			ToolStripUIAdapterFactory factory = new ToolStripUIAdapterFactory();
			IUIElementAdapter adapter = factory.GetAdapter(new MenuStrip());
			Assert.IsTrue(adapter is ToolStripItemCollectionUIAdapter);
		}

		[TestMethod]
		public void FactoryCreatesCorrectAdapterForToolStripButton()
		{
			ToolStripUIAdapterFactory factory = new ToolStripUIAdapterFactory();
			ToolStrip strip = new ToolStrip();
			ToolStripButton item = new ToolStripButton();
			strip.Items.Add(item);
			IUIElementAdapter adapter = factory.GetAdapter(item);
			Assert.IsTrue(adapter is ToolStripItemOwnerCollectionUIAdapter);
		}

		[TestMethod]
		public void FactoryCreatesCorrectAdapterForToolStripMenuItem()
		{
			ToolStripUIAdapterFactory factory = new ToolStripUIAdapterFactory();
			ToolStrip strip = new ToolStrip();
			ToolStripMenuItem item = new ToolStripMenuItem();
			strip.Items.Add(item);
			IUIElementAdapter adapter = factory.GetAdapter(item);
			Assert.IsTrue(adapter is ToolStripItemOwnerCollectionUIAdapter);
		}

		[TestMethod]
		public void FactoryCreatesCorrectAdapterForToolStripDropDownMenu()
		{
			ToolStripUIAdapterFactory factory = new ToolStripUIAdapterFactory();
			IUIElementAdapter adapter = factory.GetAdapter(new ToolStripDropDownMenu());
			Assert.IsTrue(adapter is ToolStripItemCollectionUIAdapter);
		}

		[TestMethod]
		public void FactoryCreatesCorrectAdapterForToolStripItemCollection()
		{
			ToolStripUIAdapterFactory factory = new ToolStripUIAdapterFactory();
			MenuStrip strip = new MenuStrip();
			IUIElementAdapter adapter = factory.GetAdapter(strip.Items);
			Assert.IsTrue(adapter is ToolStripItemCollectionUIAdapter);
		}
	}
}
