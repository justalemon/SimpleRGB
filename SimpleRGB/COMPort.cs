using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace SimpleRGB;

public partial class COMPort : Form
{
    public COMPort()
    {
        InitializeComponent();
    }

    private void RefreshPorts()
    {
        string[] ports = SerialPort.GetPortNames();

        foreach (string port in ports)
        {
            PortsComboBox.Items.Add(port);
        }
    }

    private void COMPort_Shown(object sender, EventArgs e) => RefreshPorts();

    private void RefreshButton_Click(object sender, EventArgs e) => RefreshPorts();
}
