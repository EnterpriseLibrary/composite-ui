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
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Microsoft.Practices.CompositeUI.Tests.SmartParts
{
	[TestClass]
	public class SmartPartInfoProviderFixture
	{
		private static SmartPartInfoProvider provider;

		[TestInitialize]
		public void SetUp()
		{
			provider = new SmartPartInfoProvider();
		}

		[TestMethod]
		public void CanRegisterSPI()
		{
			provider.Items.Add(new SmartPartInfo());
		}

		[TestMethod]
		public void CanRetrieveSpecificSPI()
		{
			MockSPI spi = new MockSPI();
			provider.Items.Add(new SmartPartInfo());
			provider.Items.Add(spi);

			MockSPI result = (MockSPI)provider.GetSmartPartInfo(typeof(MockSPI));

			Assert.AreSame(spi, result);
		}

		class MockSPI : ISmartPartInfo 
		{
			#region ISmartPartInfo Members

			public string Description
			{
				get
				{
					throw new Exception("The method or operation is not implemented.");
				}
				set
				{
					throw new Exception("The method or operation is not implemented.");
				}
			}

			public string Title
			{
				get
				{
					throw new Exception("The method or operation is not implemented.");
				}
				set
				{
					throw new Exception("The method or operation is not implemented.");
				}
			}

			#endregion
		}
	}
}
