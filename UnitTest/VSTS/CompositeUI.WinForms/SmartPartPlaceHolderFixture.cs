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
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests
{
	[TestClass]
	public class SmartPartPlaceholderFixture
	{
		private static SmartPartPlaceholder placeholder;

		[TestInitialize]
		public void Setup()
		{
			placeholder = new SmartPartPlaceholder();
		}

		[TestMethod]
		public void DefaultSmartPartNameIsSettable()
		{
			placeholder.SmartPartName = "TestSP";

			Assert.AreEqual("TestSP", placeholder.SmartPartName);
		}

		[TestMethod]
		public void ControlIsVisibleWhenSmartPartSet()
		{
			MockSmartPart sp = new MockSmartPart();
			sp.Visible = false;

			placeholder.SmartPart = sp;

			Assert.IsTrue(sp.Visible);
		}

		[TestMethod]
		public void ControlDockFillWhenSmartPartSet()
		{
			MockSmartPart sp = new MockSmartPart();

			placeholder.SmartPart = sp;

			Assert.AreEqual(DockStyle.Fill, sp.Dock);
		}

        [TestMethod]
        public void PreviousControlsRemovedWhenSmartPartSet()
        {
            MockSmartPart sp = new MockSmartPart();
            Control other = new Control();
            placeholder.Controls.Add(other);

            placeholder.SmartPart = sp;

            Assert.IsNull(other.Parent);
        }

		[TestMethod]
		public void ControlIsContainedInAreaWhenSmartPartSet()
		{
			MockSmartPart sp = new MockSmartPart();

			placeholder.SmartPart = sp;

			Assert.AreSame(sp, placeholder.Controls[0]);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsIfSmartPartNotControl()
		{
			placeholder.SmartPart = new NonControlSmartPart();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfSmartPartIsNull()
		{
			placeholder.SmartPart = null;
		}

		[TestMethod]
		public void PlaceholderClearsControlsWhenControlAdded()
		{
			placeholder.SmartPart = new MockSmartPart();
			placeholder.SmartPart = new MockSmartPart();

			Assert.AreEqual(1, placeholder.Controls.Count);
		}

		[TestMethod]
		public void PlaceholderFiresSmartShownEvent()
		{
			bool smartPartShown = false;
			placeholder.SmartPartShown += delegate { smartPartShown = true; };

			placeholder.SmartPart = new MockSmartPart();

			Assert.IsTrue(smartPartShown);
		}

		[TestMethod]
		public void PlaceHolderPassesCorrectSmartPartWhenShown()
		{
			object argsSmartPart = null;
			placeholder.SmartPartShown +=
				delegate(object sender, SmartPartPlaceHolderEventArgs args) { argsSmartPart = args.SmartPart; };
			MockSmartPart smartPart = new MockSmartPart();

			placeholder.SmartPart = smartPart;

			Assert.AreSame(smartPart, argsSmartPart);
		}

		[TestMethod]
		public void HolderEventArgsContainsSmartPart()
		{
			object smartPart = new object();
			SmartPartPlaceHolderEventArgs args = new SmartPartPlaceHolderEventArgs(smartPart);

			Assert.IsNotNull(args.SmartPart);
			Assert.AreSame(smartPart, args.SmartPart);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void HolderEventArgsThrowsIfNullSmartPart()
		{
			SmartPartPlaceHolderEventArgs args = new SmartPartPlaceHolderEventArgs(null);
		}

		[TestMethod]
		public void BackGroundIstransparent()
		{
			Assert.AreEqual(Color.Transparent, placeholder.BackColor);
		}

		#region Supporting classes

		[SmartPart]
		private class NonControlSmartPart : Object {}

		[SmartPart]
		private class MockSmartPart : Control {}

		#endregion
	}
}
