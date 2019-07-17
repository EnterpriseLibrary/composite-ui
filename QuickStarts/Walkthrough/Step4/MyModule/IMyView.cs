using System;
using System.Collections.Generic;
using System.Text;

namespace MyModule
{
	public interface IMyView
	{
		event EventHandler Load;
		string Message { get; set; }
	}
}
