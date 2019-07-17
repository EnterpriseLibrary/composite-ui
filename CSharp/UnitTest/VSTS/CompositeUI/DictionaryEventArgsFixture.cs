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

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class DictionaryEventArgsFixture
	{
		private DictionaryEventArgs eventArgs;

		[TestInitialize]
		public void TestSetup()
		{
			eventArgs = new DictionaryEventArgs();
		}

		[TestMethod]
		public void ToStringShowsContents()
		{
			eventArgs.Data.Add("one", "1");
			eventArgs.Data.Add("two", "2");

			Assert.AreEqual("one: 1, two: 2", eventArgs.ToString());
		}
	}
}
