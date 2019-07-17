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

using System.Diagnostics;

namespace Microsoft.Practices.CompositeUI.Tests
{
	internal class MockListener : TraceListener
	{
		public bool WasCalled = false;

		public override void Write(string message)
		{
			WasCalled = true;
		}

		public override void WriteLine(string message)
		{
			WasCalled = true;
		}
	}
}