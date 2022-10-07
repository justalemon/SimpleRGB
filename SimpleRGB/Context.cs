using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace SimpleRGB;

/// <summary>
/// The custom Windows Forms context.
/// </summary>
public class Context : ApplicationContext
{
    private readonly NotifyIcon notifyIcon;
    private readonly ContextMenuStrip strip;
    private readonly Icon icon;
    
    /// <summary>
    /// Creates a new custom Context.
    /// </summary>
    public Context()
    {
        icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
        strip = new ContextMenuStrip();
        notifyIcon = new NotifyIcon();

        notifyIcon.Icon = icon;
        notifyIcon.ContextMenuStrip = strip;
        notifyIcon.Visible = true;
        
        ThreadExit += OnThreadExit;
    }

    private void OnThreadExit(object sender, EventArgs e)
    {
        notifyIcon.Dispose();
        strip.Dispose();
        icon.Dispose();
    }
}
