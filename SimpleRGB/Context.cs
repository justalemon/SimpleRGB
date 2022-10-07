using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;

namespace SimpleRGB;

/// <summary>
/// The custom Windows Forms context.
/// </summary>
public class Context : ApplicationContext
{
    private readonly NotifyIcon notifyIcon;
    private readonly ContextMenuStrip strip;
    private readonly Icon icon;
    private readonly COMPort configureCOM = new COMPort();
    
    /// <summary>
    /// Creates a new custom Context.
    /// </summary>
    public Context()
    {
        icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
        strip = new ContextMenuStrip();
        notifyIcon = new NotifyIcon();

        ToolStripMenuItem configure = new ToolStripMenuItem("Configure COM Port");
        configure.Click += ConfigureOnClick;
        ToolStripMenuItem exit = new ToolStripMenuItem("Exit");
        exit.Click += ExitOnClick;

        strip.Items.Add(configure);
        strip.Items.Add(exit);

        notifyIcon.Icon = icon;
        notifyIcon.ContextMenuStrip = strip;
        notifyIcon.Visible = true;
        
        ThreadExit += OnThreadExit;
        ToastNotificationManagerCompat.OnActivated += ToastNotificationManagerCompatOnOnActivated;

        new ToastContentBuilder()
            .AddArgument("com", "COM")
            .AddText("You need to configure the COM port!")
            .AddText("Tap or Click this notification to select your COM port.")
            .Show();
    }

    private void ExitOnClick(object sender, EventArgs e) => ExitThread();

    private void ConfigureOnClick(object sender, EventArgs e) => configureCOM.ShowDialog();

    private void ToastNotificationManagerCompatOnOnActivated(ToastNotificationActivatedEventArgsCompat e)
    {
        ToastArguments args = ToastArguments.Parse(e.Argument);

        if (args.Contains("com"))
        {
            configureCOM.ShowDialog();
        }
    }

    private void OnThreadExit(object sender, EventArgs e)
    {
        notifyIcon.Dispose();
        strip.Dispose();
        icon.Dispose();
    }
}
