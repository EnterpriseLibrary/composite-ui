//===============================================================================
// Microsoft patterns & practices
// Object Builder Application Block
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

namespace Microsoft.Practices.ObjectBuilder.Tests.Location
{
	[TestClass]
	public class ReadOnlyLocatorFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullInnerLocatorThrows()
		{
			ReadOnlyLocator locator = new ReadOnlyLocator(null);
		}

		[TestMethod]
		public void CannotCastAReadOnlyLocatorToAReadWriteLocator()
		{
			Locator innerLocator = new Locator();
			ReadOnlyLocator locator = new ReadOnlyLocator(innerLocator);

			Assert.IsTrue(locator.ReadOnly);
			Assert.IsNull(locator as IReadWriteLocator);
		}

		[TestMethod]
		public void ReadOnlyLocatorCountReflectsInnerLocatorCount()
		{
			Locator innerLocator = new Locator();
			ReadOnlyLocator locator = new ReadOnlyLocator(innerLocator);

			innerLocator.Add(1, 1);
			innerLocator.Add(2, 2);

			Assert.AreEqual(innerLocator.Count, locator.Count);
		}

		[TestMethod]
		public void ParentLocatorOfReadOnlyLocatorIsAlsoReadOnly()
		{
			Locator parentInnerLocator = new Locator();
			Locator childInnerLocator = new Locator(parentInnerLocator);
			ReadOnlyLocator childLocator = new ReadOnlyLocator(childInnerLocator);

			IReadableLocator parentLocator = childLocator.ParentLocator;

			Assert.IsTrue(parentLocator.ReadOnly);
			Assert.IsNull(parentLocator as IReadWriteLocator);
		}

		[TestMethod]
		public void ItemsContainedInLocatorContainedInReadOnlyLocator()
		{
			Locator innerLocator = new Locator();
			ReadOnlyLocator locator = new ReadOnlyLocator(innerLocator);

			innerLocator.Add(1, 1);
			innerLocator.Add(2, 2);

			Assert.IsTrue(locator.Contains(1));
			Assert.IsTrue(locator.Contains(2));
			Assert.IsFalse(locator.Contains(3));
		}

		[TestMethod]
		public void CanEnumerateItemsInReadOnlyLocator()
		{
			Locator innerLocator = new Locator();
			ReadOnlyLocator locator = new ReadOnlyLocator(innerLocator);

			innerLocator.Add(1, 1);
			innerLocator.Add(2, 2);

			bool sawOne = false;
			bool sawTwo = false;

			foreach (KeyValuePair<object, object> pair in locator)
			{
				if (pair.Key.Equals(1))
					sawOne = true;
				if (pair.Key.Equals(2))
					sawTwo = true;
			}

			Assert.IsTrue(sawOne);
			Assert.IsTrue(sawTwo);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidCastException))]
		public void GenericGetEnforcesDataType()
		{
			Locator innerLocator = new Locator();
			ReadOnlyLocator locator = new ReadOnlyLocator(innerLocator);

			innerLocator.Add(1, 2);

			locator.Get<string>(1);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidCastException))]
		public void GenericGetWithSearchModeEnforcesDataType()
		{
			Locator innerLocator = new Locator();
			ReadOnlyLocator locator = new ReadOnlyLocator(innerLocator);

			innerLocator.Add(1, 2);

			locator.Get<string>(1, SearchMode.Local);
		}
	}
}
