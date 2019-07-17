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
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.WinForms.UIElements;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests.UIElements
{
	[TestClass]
	public class ToolStripItemCollectionAdapterFixture
	{
		[TestMethod]
		public void AddMethodsAppendsToEmptyCollection()
		{
			ToolStrip strip = new ToolStrip();
			ToolStripItemCollectionUIAdapter adapter = new ToolStripItemCollectionUIAdapter(strip.Items);

			ToolStripButton button = new ToolStripButton();
			adapter.Add(button);

			Assert.AreSame(button, strip.Items[0]);
		}

		[TestMethod]
		public void AddAppendsToCollectionWithItems()
		{
			ToolStrip strip = new ToolStrip();
			ToolStripButton button1 = new ToolStripButton();
			strip.Items.Add(button1);
			ToolStripItemCollectionUIAdapter adapter = new ToolStripItemCollectionUIAdapter(strip.Items);

			ToolStripButton button2 = new ToolStripButton();
			adapter.Add(button2);

			Assert.AreEqual(2, strip.Items.Count);
			Assert.AreSame(button1, strip.Items[0]);
			Assert.AreSame(button2, strip.Items[1]);
		}

		[TestMethod]
		public void CanRemoveAnItem()
		{
			ToolStrip strip = new ToolStrip();
			ToolStripButton button1 = new ToolStripButton();
			strip.Items.Add(button1);
			ToolStripItemCollectionUIAdapter adapter = new ToolStripItemCollectionUIAdapter(strip.Items);

			ToolStripButton button2 = new ToolStripButton();
			adapter.Add(button2);

			adapter.Remove(button1);

			Assert.AreEqual(1, strip.Items.Count);
			Assert.AreSame(button2, strip.Items[0]);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateWithNullThrows()
		{
			new ToolStripItemCollectionUIAdapter(null);
		}
	}
}
