using System;
using System.Collections.Generic;
using System.Text;

namespace GPSModule
{
	public interface IDistanceCalculatorService
	{
		int ComputeDistance(int latitude, int longitude);
	}
}
