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

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Practices.CompositeUI;

[assembly: Module("module2")]
[assembly: ModuleDependency("module1")]

namespace Microsoft.Practices.CompositeUI.Tests.Mocks.Src
{
	public class ModuleDependency2 : ModuleInit
	{
	}
}
