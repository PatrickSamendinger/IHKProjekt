using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Timers;

namespace LearnSmartGmbhService
{
    public partial class Service1 : ServiceBase
    {
        public static bool isChanged = false;
        private static List<string> changes = new List<string>();
        private string confPath = (AppDomain.CurrentDomain.BaseDirectory + "\\config.xml");
        private string dataPath = (AppDomain.CurrentDomain.BaseDirectory + "\\tools.xml");
        private Timer timer1 = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            AdminData data = AdminData.ReadData();
            HardwareConf conf = new HardwareConf();
            if (!File.Exists(confPath))
            {
                HardwareConf.WriteConfig();
            }
            changes = HardwareConf.CheckDifferences();
            if (isChanged == true)
            {
                HardwareConf.WriteEmail();
            }
            timer1 = new Timer();
            this.timer1.Interval = data.time;
            /*Intervall in der Service ausgeführt wird, durch Editor änderbar */
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;
            WriteErrorLog("Hardware-Service wurde gestartet.");
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            changes.Clear();
            AdminData data = AdminData.ReadData();
            timer1.Interval = data.time;
            WriteErrorLog("Service erfolgreich durchgeführt!");
            changes = HardwareConf.CheckDifferences();
            if (isChanged == true)
            {
                HardwareConf.WriteEmail();
                HardwareConf.WriteConfig();
            }
        }
 
        protected override void OnStop()
        {
            timer1.Enabled = false;
            WriteErrorLog("Hardware-Service wurde angehalten.");
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
        }

        public class AdminData
        {
            public string adminmail;
            public long time;

            public static AdminData ReadData()
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(AdminData));
                System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\tools.xml");
                AdminData admin = (AdminData)reader.Deserialize(file);
                file.Close();
                return admin;
            }

            public static void WriteData(AdminData data)
            {
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(AdminData));
                var path = (AppDomain.CurrentDomain.BaseDirectory + "\\tools.xml");
                System.IO.FileStream file = System.IO.File.Create(path);
                writer.Serialize(file, data);
                file.Close();
            }
        }

        public class HardwareConf
        {
            public string ComputerName;
            public string Prozessor;
            public string HDDName;
            public double HDDSize;
            public string grakaName;
            public string grakaRAM;
            public string NetworkCardName;
            public string NetworkCardPA;
            public string SoundCardName;
            public string SoundCardID;
            public string Memory;

            public HardwareConf GetHardware()
            {
                HardwareConf specs = new HardwareConf();
                ManagementObjectSearcher Grafikkarte = new ManagementObjectSearcher("select * from Win32_VideoController");
                foreach (var Graka in Grafikkarte.Get())
                {
                    specs.grakaName = Graka["Name"].ToString();
                    specs.grakaRAM = Graka["AdapterRAM"].ToString();
                }
                DriveInfo[] FestPlatten = DriveInfo.GetDrives();
                foreach (DriveInfo d in FestPlatten)
                {
                    if (d.DriveType == DriveType.Fixed)
                    {
                        specs.HDDName = d.Name.ToString();
                        if (d.IsReady == true)
                        {
                            specs.HDDSize = d.TotalSize / 1024 / 1024 / 1024;
                        }
                    }
                }
                ManagementObjectSearcher Prozessoren = new ManagementObjectSearcher("select * from Win32_Processor");
                foreach (ManagementObject obj in Prozessoren.Get())
                {
                    specs.Prozessor = obj["Name"].ToString();
                }
                NetworkInterface[] Netzwerk = NetworkInterface.GetAllNetworkInterfaces();
                if (Netzwerk == null || Netzwerk.Length < 1)
                {
                    Console.WriteLine("Keine Netzwerkkarte gefunden");
                }
                else
                {
                    foreach (NetworkInterface adapter in Netzwerk)
                    {
                        if (!adapter.Description.Contains("VMware") && !adapter.Description.Contains("Software"))
                        {
                            IPInterfaceProperties properties = adapter.GetIPProperties();
                            specs.NetworkCardName = adapter.Description;
                            specs.NetworkCardPA = adapter.GetPhysicalAddress().ToString();
                        }
                    }
                }
                ManagementObjectSearcher Soundkarte = new ManagementObjectSearcher("select * from Win32_SoundDevice");
                foreach (ManagementObject obj in Soundkarte.Get())
                {
                    specs.SoundCardName = obj["Name"].ToString();
                    specs.SoundCardID = obj["DeviceID"].ToString();
                }
                double memorynumber = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
                specs.Memory = Convert.ToString(Math.Ceiling(memorynumber / 1024 / 1024 / 1024) + " GB");
                specs.ComputerName = new Microsoft.VisualBasic.Devices.Computer().Name;
                return specs;
            }

            public static void WriteConfig()
            {
                HardwareConf conf = new HardwareConf();
                conf = conf.GetHardware();

                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(HardwareConf));
                var path = (AppDomain.CurrentDomain.BaseDirectory + "\\config.xml");
                System.IO.FileStream file = System.IO.File.Create(path);
                writer.Serialize(file, conf);
                file.Close();
            }

            public static HardwareConf ReadConfig()
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(HardwareConf));
                System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\config.xml");
                HardwareConf conf = (HardwareConf)reader.Deserialize(file);
                file.Close();

                return conf;
            }

            public static List<string> CheckDifferences()
            {
                HardwareConf conf = ReadConfig();
                HardwareConf actual = new HardwareConf().GetHardware();

                changes.Add("Betroffener Computer : " + actual.ComputerName);

                if (conf.grakaName != actual.grakaName)
                {
                    changes.Add("Fehler bei Grafikkarte festgestellt! " + actual.grakaName + " statt " + conf.grakaName);
                    isChanged = true;
                }
                if (conf.grakaRAM != actual.grakaRAM)
                {
                    changes.Add("Fehler bei Grafikkarte festgestellt! " + actual.grakaRAM + " statt " + conf.grakaRAM);
                    isChanged = true;
                }
                if (conf.HDDSize != actual.HDDSize)
                {
                    changes.Add("Fehler bei  Festplatte festgestellt " + actual.HDDName + " " + actual.HDDSize + " statt " + conf.HDDName + conf.HDDSize);
                    isChanged = true;
                }
                if (conf.Prozessor != actual.Prozessor)
                {
                    changes.Add("Fehler bei Prozessor festgestellt,  " + actual.Prozessor + " statt " + conf.Prozessor);
                    isChanged = true;
                }
                if (conf.NetworkCardPA != actual.NetworkCardPA)
                {
                    changes.Add("Fehler bei Netzwerkkarte festgestellt, " + actual.NetworkCardName + " statt " + conf.NetworkCardName);
                    isChanged = true;
                }
                if (conf.SoundCardID != actual.SoundCardID)
                {
                    changes.Add("Fehler bei  Soundkarte festgestellt ,  " + actual.SoundCardName + " statt " + conf.SoundCardName);
                    isChanged = true;
                }
                if (conf.Memory != actual.Memory)
                {
                    changes.Add("Fehler bei Arbeitsspeicher festgestellt , " + actual.Memory + " statt " + conf.Memory);
                    isChanged = true;
                }

                return changes;
            }

            public static void WriteEmail()
            {
                HardwareConf conf = ReadConfig();
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();
                AdminData temp = AdminData.ReadData();

                try
                {
                    m.From = new MailAddress("pstagesbericht@gmail.com");
                    m.To.Add(temp.adminmail);
                    m.Subject = "Tagesbericht";
                    m.IsBodyHtml = true;
                    foreach (var item in changes)
                    {
                        m.Body += item + "<br>";
                    }
                    sc.Host = "smtp.gmail.com";
                    sc.Port = 587;
                    sc.Credentials = new System.Net.NetworkCredential("pstagesbericht@gmail.com", "Reddevil89");
                    sc.EnableSsl = true;
                    sc.Send(m);
                }
                catch (Exception ex)
                {
                    WriteErrorLog(ex);
                }
            }
        }

        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logfile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        public static void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logfile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch { }
        }
    }
}