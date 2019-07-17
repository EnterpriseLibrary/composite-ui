using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI.WinForms;

namespace ShellApplication
{
	public class ShellApplication : FormShellApplication<ShellWorkItem, ShellForm>
	{
		[STAThread]
		static void Main()
		{
			new ShellApplication().Run();
		}
	}
}
