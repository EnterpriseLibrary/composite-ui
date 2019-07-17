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
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.WinForms;
using Microsoft.Practices.CompositeUI.UIElements;

namespace CommandsQuickStart
{
	public class CommandsApplication : FormShellApplication<MainWorkItem, ShellForm>
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			new CommandsApplication().Run();
		}

		/// <summary>
		/// Once the shell is created, we will create a UIElementExtention point
		/// under the File menu that the MainWorkItem can use to add menu items.
		/// </summary>
		protected override void  AfterShellCreated()
		{
			base.AfterShellCreated();

			ToolStripMenuItem fileItem = (ToolStripMenuItem)Shell.MainMenuStrip.Items["File"];
			RootWorkItem.UIExtensionSites.RegisterSite("File", fileItem.DropDownItems);
		}

		#region Unhandled Exception
		
		public override void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;

			if (ex != null)
			{
				MessageBox.Show(BuildExceptionString(ex));
			}
			else
			{
				MessageBox.Show("An Exception has occured, unable to get details");
			}

			Environment.Exit(0);
		}

		private string BuildExceptionString(Exception exception)
		{
			string errMessage = string.Empty;

			errMessage +=
				exception.Message + Environment.NewLine + exception.StackTrace;

			while (exception.InnerException != null)
			{
				errMessage += BuildInnerExceptionString(exception.InnerException);

				exception = exception.InnerException;
			}

			return errMessage;
		}

		private string BuildInnerExceptionString(Exception innerException)
		{
			string errMessage = string.Empty;

			errMessage += Environment.NewLine + " InnerException ";
			errMessage += Environment.NewLine + innerException.Message + Environment.NewLine + innerException.StackTrace;

			return errMessage;
		} 
		#endregion

	}
}