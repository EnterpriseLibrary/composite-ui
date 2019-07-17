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

namespace Microsoft.Practices.CompositeUI.WinForms.Tests.Visualizer
{
	[TestClass]
	public class WinFormsVisualizerFixture
	{
		[TestMethod]
		public void MainWorkspaceIsAvailableWhenVisualizerInitialized()
		{
			TestableWinFormsVisualizer viz = new TestableWinFormsVisualizer();
			viz.Initialize(null, null);

			Assert.IsNotNull(viz.RootWorkItem.Workspaces["MainWorkspace"]);
		}

		class TestableWinFormsVisualizer : WinFormsVisualizer
		{
			public new WorkItem RootWorkItem
			{
				get { return base.RootWorkItem; }
			}
		}
	}
}
