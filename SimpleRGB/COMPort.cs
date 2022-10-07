using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace SimpleRGB;

public partial class COMPort : Form
{
    /// <summary>
    /// The last COM Port that was opened.
    /// </summary>
    public SerialPort LastOpenedPort { get; private set; }

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

    private void SaveButton_Click(object sender, EventArgs e)
    {
        if (PortsComboBox.SelectedIndex == -1)
        {
            return;
        }
        
        try
        {
            if (LastOpenedPort != null && LastOpenedPort.IsOpen)
            {
                LastOpenedPort.Close();
            }
            
            SerialPort port = new SerialPort();
            port.PortName = (string)PortsComboBox.SelectedItem;
            port.BaudRate = 9600;
            port.Open();

            LastOpenedPort = port;

            DialogResult = DialogResult.OK;
            
            Close();
        }
        catch (UnauthorizedAccessException exception)
        {
            MessageBox.Show($"Unable to open {PortsComboBox.SelectedItem}: {exception}", "Unable to open COM Port", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
