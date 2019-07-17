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
using Microsoft.Practices.CompositeUI;
using System.Windows.Forms;

namespace GPSModule
{
	[Service(typeof(IDistanceCalculatorService), AddOnDemand = true)]
	public class DistanceCalculatorService : IDistanceCalculatorService
	{
		public DistanceCalculatorService()
		{
			MessageBox.Show("This is DistanceCalculatorService being constructed");
		}

		public int ComputeDistance(int latitude, int longitude)
		{
			return 1234;
		}
	}
}
