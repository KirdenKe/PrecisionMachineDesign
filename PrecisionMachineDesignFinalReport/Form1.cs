using System;
using System.Windows.Forms;

namespace PrecisionMachineDesignFinalReport
{
    public partial class Form1 : Form
    {
        BackendComputingArea backendComputingArea = new BackendComputingArea();
        public Form1()
        {
            InitializeComponent();
        }

        private void button_Generatecsv_Click(object sender, EventArgs e)
        {
            textBox_output.Text = backendComputingArea.OutputText(0);
        }

        private void button_Generatemd_Click(object sender, EventArgs e)
        {
            textBox_output.Text = backendComputingArea.OutputText(1);
        }
    }
}
