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
	public class ToolStripItemOwnerCollectionUIAdapterFixture
	{
		[TestMethod]
		public void AddInsertAfterAttachedItem()
		{
			ToolStrip strip = new ToolStrip();
			ToolStripSeparator extensionSite = new ToolStripSeparator();
			strip.Items.Add( extensionSite );
			ToolStripItemOwnerCollectionUIAdapter adapter = new ToolStripItemOwnerCollectionUIAdapter(extensionSite);

			ToolStripButton button = new ToolStripButton();
			adapter.Add(button);

			Assert.AreEqual(2, strip.Items.Count);
			Assert.AreSame(extensionSite, strip.Items[0]);
			Assert.AreSame(button, strip.Items[1]);
		}

		[TestMethod]
		public void AddInsertsAfterAttachedItemWhenOneFollowsIt()
		{
			ToolStrip strip = new ToolStrip();
			ToolStripSeparator extensionSite = new ToolStripSeparator();
			strip.Items.Add(extensionSite);
			ToolStripButton followingButton = new ToolStripButton();
			strip.Items.Add(followingButton);
			ToolStripItemOwnerCollectionUIAdapter adapter = new ToolStripItemOwnerCollectionUIAdapter(extensionSite);

			ToolStripButton button = new ToolStripButton();
			adapter.Add(button);

			Assert.AreEqual(3, strip.Items.Count);
			Assert.AreSame(extensionSite, strip.Items[0]);
			Assert.AreSame(button, strip.Items[1]);
			Assert.AreSame(followingButton, strip.Items[2]);
		}


		[TestMethod]
		public void ItemCanBeRemovedWhenManagedItemHasAlreadyBeenRemoved()
		{
			ToolStrip strip = new ToolStrip();
			ToolStripSeparator separator = new ToolStripSeparator();
			strip.Items.Add(separator);
			ToolStripItemOwnerCollectionUIAdapter adapterDependingOnSeparator = new ToolStripItemOwnerCollectionUIAdapter(separator);
			ToolStripButton button = new ToolStripButton("Foo");
			adapterDependingOnSeparator.Add(button);

			strip.Items.Remove(separator);
			adapterDependingOnSeparator.Remove(button);

			Assert.IsFalse(strip.Items.Contains(button));
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void AddThrowsIfManagedItemIsRemoved()
		{
			ToolStrip strip = new ToolStrip();
			ToolStripSeparator separator = new ToolStripSeparator();
			strip.Items.Add(separator);
			ToolStripItemOwnerCollectionUIAdapter adapterDependingOnSeparator = new ToolStripItemOwnerCollectionUIAdapter(separator);

			strip.Items.Remove(separator);
			adapterDependingOnSeparator.Add(new ToolStripButton());
		}

		[TestMethod]
		public void RemoveAnItemFromTheOriginalCollectionWhenTheManagedItemIsMovedToAnotherCollection()
		{
			ToolStrip strip1 = new ToolStrip();
			ToolStripSeparator separator = new ToolStripSeparator();
			strip1.Items.Add(separator);
			ToolStripItemOwnerCollectionUIAdapter adapterDependingOnSeparator = new ToolStripItemOwnerCollectionUIAdapter(separator);

			ToolStripButton button = new ToolStripButton("Foo");
			adapterDependingOnSeparator.Add(button);

			strip1.Items.Remove(separator);

			ToolStrip strip2 = new ToolStrip();
			strip2.Items.Add(separator);

			adapterDependingOnSeparator.Remove(button);

			Assert.IsFalse(strip1.Items.Contains(button));
		}

		[TestMethod]
		public void AddInsertsInNewCollectionWhenManagedItemMoved()
		{
			ToolStrip strip1 = new ToolStrip();
			ToolStrip strip2 = new ToolStrip();
			ToolStripSeparator extensionSite = new ToolStripSeparator();
			strip1.Items.Add(extensionSite);
			ToolStripItemOwnerCollectionUIAdapter adapter = new ToolStripItemOwnerCollectionUIAdapter(extensionSite);

			strip1.Items.Remove(extensionSite);
			strip2.Items.Add(extensionSite);

			ToolStripButton button = new ToolStripButton();
			adapter.Add(button);

			Assert.AreEqual(0, strip1.Items.Count);
			Assert.AreEqual(2, strip2.Items.Count);
			Assert.AreSame(extensionSite, strip2.Items[0]);
			Assert.AreSame(button, strip2.Items[1]);
		}


		[TestMethod]
		public void RemovingAnItemFromDisposedStripDoesNotThrow()
		{
			ToolStrip strip1 = new ToolStrip();
			ToolStripSeparator separator = new ToolStripSeparator();
			strip1.Items.Add(separator);

			ToolStripButton button = new ToolStripButton("Foo");
			ToolStripItemOwnerCollectionUIAdapter adapterDependingOnSeparator = new ToolStripItemOwnerCollectionUIAdapter(separator);
			adapterDependingOnSeparator.Add(button);

			strip1.Dispose();

			adapterDependingOnSeparator.Remove(button);
		}
	}
}
