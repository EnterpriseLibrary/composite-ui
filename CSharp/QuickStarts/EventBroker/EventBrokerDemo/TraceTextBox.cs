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
using System.Diagnostics;
using System.Windows.Forms;

namespace EventBrokerDemo
{
	// This control shows a simple usage of the diagnostics
	// capabilities included in CAB.
	// It uses a TextBox and a custom listener to report the 
	// log messages issued by the EventBroker subsystem.
	public partial class TraceTextBox : UserControl
	{
		private TextBoxTraceListener listener;

		public TraceTextBox()
		{
			InitializeComponent();
			listener = new TextBoxTraceListener(logBox);

			// Just add the listener to the trace system.
			Trace.Listeners.Add(listener);
		}

		private void enableLogging_CheckedChanged(object sender, EventArgs e)
		{
			listener.Enabled = enableLogging.Checked;
		}

		// This is our custom TraceListener which just write the messages
		// to a supplied TextBox control.
		private class TextBoxTraceListener : TraceListener
		{
			TextBox log;

			private bool enabled = true;

			public bool Enabled
			{
				get { return enabled; }
				set { enabled = value; }
			}

			public TextBoxTraceListener(TextBox log)
			{
				this.log = log;
			}

			public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
			                                string format, params object[] args)
			{
				if (args.Length > 0)
				{
					WriteLine(String.Format(format, args));
				}
				else
				{
					WriteLine(format);
				}
			}

			private delegate void InvokedWrite(string message);

			public override void Write(string message)
			{
				if (enabled)
				{
					if (log.InvokeRequired)
					{
						log.BeginInvoke(new InvokedWrite(Write), message);
						return;
					}
					log.Text = log.Text + Environment.NewLine + message;
					log.Select(log.Text.Length - 1, 0);
					log.ScrollToCaret();
				}
			}


			public override void WriteLine(string message)
			{
				Write(message);
			}
		}
	}
}