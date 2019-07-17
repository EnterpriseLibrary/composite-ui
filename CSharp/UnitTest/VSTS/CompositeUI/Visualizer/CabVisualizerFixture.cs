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
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.Configuration;
using System.Collections.Generic;

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class CabVisualizerFixture
	{
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InitializingVisualizerTwiceThrowsException()
		{
			CabVisualizer visualizer = new CabVisualizer();

			visualizer.Initialize(new WorkItem(), new Builder());
			visualizer.Initialize(new WorkItem(), new Builder());
		}

		[TestMethod]
		public void VisualizationCanGetVisualizerAsService()
		{
			TestableVisualizer visualizer = CreateVisualizer();

			MockVisualization visualization = visualizer.AddNewVisualization<MockVisualization>();

			Assert.AreSame(visualizer, visualization.Visualizer);
		}

		[TestMethod]
		public void VisualizerLoadsVisualizationsFromConfiguration()
		{
			TestableRootWorkItem wi = new TestableRootWorkItem();
			TestableVisualizer visualizer = new TestableVisualizer();

			visualizer.AddVisualizersFromConfig = true;
			visualizer.Initialize(wi, wi.Builder);

			Assert.AreEqual(1, visualizer.Visualizations.Count);
			foreach (object vis in visualizer.Visualizations)
				Assert.IsTrue(vis is MockVisualization);
		}

		TestableVisualizer CreateVisualizer()
		{
			TestableRootWorkItem wi = new TestableRootWorkItem();
			TestableVisualizer visualizer = new TestableVisualizer();
			visualizer.Initialize(wi, wi.Builder);

			return visualizer;
		}

		class TestableVisualizer : CabVisualizer
		{
			public bool AddVisualizersFromConfig = false;

			protected override VisualizationElementCollection Configuration
			{
				get
				{
					VisualizationElementCollection result = new VisualizationElementCollection();

					if (AddVisualizersFromConfig)
					{
						VisualizationElement elt = new VisualizationElement();
						elt.Type = typeof(MockVisualization);
						result.Add(elt);
					}

					return result;
				}
			}

			public new TVisualization AddNewVisualization<TVisualization>()
			{
				return base.AddNewVisualization<TVisualization>();
			}

			public new ICollection<object> Visualizations
			{
				get
				{
					return base.Visualizations;
				}
			}
		}

		class MockVisualization
		{
			public IVisualizer Visualizer;

			[InjectionConstructor]
			public MockVisualization([ServiceDependency] IVisualizer visualizer)
			{
				Visualizer = visualizer;
			}
		}
	}
}
