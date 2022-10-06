using System;
using System.Windows.Forms;

namespace SimpleRGB;

/// <summary>
/// The program basics.
/// </summary>
public static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        ApplicationConfiguration.Initialize();
        // Application.Run();
    }
}
