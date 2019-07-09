using System;
using System.Windows.Forms;
using static LearnSmartGmbhService.Service1;

namespace LearnSmartGmbHEditor
{
    public partial class Form1 : Form
    {
        private AdminData data = new AdminData();

        public Form1()
        {
            InitializeComponent();
            showConfig();
            
        }
        public void showConfig()
        {
            data = AdminData.ReadData();
            HardwareConf conf = HardwareConf.ReadConfig();
            listBox1.Items.Add("verwendete Mail-Adresse: " + data.adminmail);
            listBox1.Items.Add("verwendeter Zeitintervall " + data.time + " ms");
            listBox1.Items.Add("------------------------------------------------------");
            listBox1.Items.Add("Verwendeter Computer: " + conf.ComputerName);
            listBox1.Items.Add("Grafikkarte: " + conf.grakaName);
            listBox1.Items.Add("Festplattenname: " + conf.HDDName);
            listBox1.Items.Add("Festplattengröße: " + conf.HDDSize+" GB");
            listBox1.Items.Add("Arbeitsspeicher: " + conf.Memory);
            listBox1.Items.Add("Netzwerkkarte: " + conf.NetworkCardName);
            listBox1.Items.Add("Prozessor: " + conf.Prozessor);
            listBox1.Items.Add("Name der Soundkarte: " + conf.SoundCardName);
            listBox1.Items.Add("------------------------------------------------------");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMail.Text) || !txtMail.Text.Contains("@"))
            {
                listBox1.Items.Add("Bitte geben Sie eine Email Adresse ein!");
            }
            else
            {
                data.adminmail = txtMail.Text;
                AdminData.WriteData(data);
                listBox1.Items.Add("Berichte werden ab sofort an " + data.adminmail + " versendet!");
            }
        }

        private void btnSaveLong_Click(object sender, EventArgs e)
        {
            long number;

            if (long.TryParse(txtInt.Text, out number))
            {
                data.time = number;
                AdminData.WriteData(data);
                listBox1.Items.Add("Zeitintervall erfolgreich auf " + number + " ms festgelegt!");
            }
            else
            {
                listBox1.Items.Add("Bitte geben Sie eine Zahl zwischen -9,223,372,036,854,775,808 bis 9,223,372,036,854,775,807 ein !");
                AdminData.WriteData(data);
            }
        }
    }
}