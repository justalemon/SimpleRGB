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
    private readonly ColorDialog colorDialog = new ColorDialog();
    
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
        
        notifyIcon.DoubleClick += NotifyIconOnDoubleClick;
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

    private void NotifyIconOnDoubleClick(object sender, EventArgs e)
    {
        if (configureCOM.Visible || configureCOM.LastOpenedPort == null || !configureCOM.LastOpenedPort.IsOpen)
        {
            return;
        }

        if (colorDialog.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        Color color = colorDialog.Color;
        int r = color.R == 0 ? -1 : color.R;
        int g = color.G == 0 ? -1 : color.G;
        int b = color.B == 0 ? -1 : color.B;
        configureCOM.LastOpenedPort.Write($"{r},{g},{b}\n");
    }

    private void OnThreadExit(object sender, EventArgs e)
    {
        notifyIcon.Dispose();
        strip.Dispose();
        icon.Dispose();
    }
}
