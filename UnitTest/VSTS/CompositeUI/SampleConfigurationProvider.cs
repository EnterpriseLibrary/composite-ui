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

using System.Collections.Specialized;
using System.Configuration;
using System.Windows.Forms;

namespace Microsoft.Practices.CompositeUI.Tests
{
	public class SampleConfigurationProvider : LocalFileSettingsProvider
	{
		public override void Initialize(string name, NameValueCollection values)
		{
			MessageBox.Show("Initialize");
		}

		public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection values)
		{
			MessageBox.Show("SetPropertyValues");
			base.SetPropertyValues(context, values);
		}

		public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context,
		                                                                  SettingsPropertyCollection properties)
		{
			MessageBox.Show("GetPropertyValues");
			return base.GetPropertyValues(context, properties);
		}
	}
}