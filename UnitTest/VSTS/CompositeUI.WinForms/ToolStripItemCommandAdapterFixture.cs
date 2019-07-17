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
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.Commands;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests
{
	[TestClass]
	public class ToolStripItemCommandAdapterFixture
	{
		private static Form mainForm;
		private static ToolStripItem item;

		[TestInitialize]
		public void SetUp()
		{
			mainForm = new Form();
			item = new ToolStripMenuItem();
			ToolStrip strip = new ToolStrip();
			strip.Items.Add(item);

			mainForm.Controls.Add(strip);
			mainForm.Show();
		}

		[TestCleanup]
		public void TearDown()
		{
			mainForm.Close();
		}

		[TestMethod]
		public void DisabledCommandDisablesButShowsItem()
		{
			Command command = new Command();
			ToolStripItemCommandAdapter adapter = new ToolStripItemCommandAdapter(item, "Click");
			command.AddCommandAdapter(adapter);

			command.Status = CommandStatus.Disabled;

			Assert.IsFalse(item.Enabled);
			Assert.IsTrue(item.Visible);
		}

		[TestMethod]
		public void EnabledCommandEnablesAndShowsItem()
		{
			Command command = new Command();
			ToolStripItemCommandAdapter adapter = new ToolStripItemCommandAdapter(item, "Click");
			command.AddCommandAdapter(adapter);
			command.Status = CommandStatus.Disabled;

			command.Status = CommandStatus.Enabled;

			Assert.IsTrue(item.Enabled);
			Assert.IsTrue(item.Visible);
		}

		[TestMethod]
		public void UnavailableCommandDisablesAndHidesItem()
		{
			Command command = new Command();
			ToolStripItemCommandAdapter adapter = new ToolStripItemCommandAdapter(item, "Click");
			command.AddCommandAdapter(adapter);

			command.Status = CommandStatus.Unavailable;

			Assert.IsFalse(item.Enabled);
			Assert.IsFalse(item.Visible);
		}
	}
}
