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
using System.Threading;

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class ReaderLockFixture
	{
		[TestMethod]
		public void CanAcquireAndReleaseReaderLock()
		{
			ReaderWriterLock rwLock = new ReaderWriterLock();

			using (new ReaderLock(rwLock))
			{
				Assert.IsTrue(rwLock.IsReaderLockHeld);
			}

			Assert.IsFalse(rwLock.IsReaderLockHeld);
		}

		[TestMethod]
		public void CanAcquireReaderLockWithTimoutMilliseconds()
		{
			ReaderWriterLock rwLock = new ReaderWriterLock();

			using (new ReaderLock(rwLock, 5000))
			{
				Assert.IsTrue(rwLock.IsReaderLockHeld);
			}

			Assert.IsFalse(rwLock.IsReaderLockHeld);

		}

		[TestMethod]
		public void CanAcquireReaderLockWithTimoutTimeSpan()
		{
			ReaderWriterLock rwLock = new ReaderWriterLock();

			using (new ReaderLock(rwLock, TimeSpan.FromSeconds(1)))
			{
				Assert.IsTrue(rwLock.IsReaderLockHeld);
			}

			Assert.IsFalse(rwLock.IsReaderLockHeld);

		}

		[TestMethod]
		public void ReaderTimedOutIsSetIfTimeoutMilliseconds()
		{
			ReaderWriterLock rwLock = new ReaderWriterLock();
			Thread t = new Thread(new ParameterizedThreadStart(AcquireWriterLock));
			t.Start(rwLock);

			Thread.Sleep(20);

			using (ReaderLock l = new ReaderLock(rwLock, 10))
			{
				Assert.IsTrue(l.TimedOut);
			}

			Assert.IsFalse(rwLock.IsReaderLockHeld);
		}

		[TestMethod]
		public void ReaderTimedOutIsSetIfTimeoutTimespan()
		{
			ReaderWriterLock rwLock = new ReaderWriterLock();
			Thread t = new Thread(new ParameterizedThreadStart(AcquireWriterLock));
			t.Start(rwLock);

			Thread.Sleep(20);

			using (ReaderLock l = new ReaderLock(rwLock, TimeSpan.FromMilliseconds(10)))
			{
				Assert.IsTrue(l.TimedOut);
			}

			Assert.IsFalse(rwLock.IsReaderLockHeld);
		}

		private void AcquireReaderLock(object state)
		{
			((ReaderWriterLock)state).AcquireReaderLock(-1);
			Thread.Sleep(100);
		}

		private void AcquireWriterLock(object state)
		{
			((ReaderWriterLock)state).AcquireWriterLock(-1);
			Thread.Sleep(100);
		}
	}
}
