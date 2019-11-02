using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyService
{
    class ServiceWork
    {
        WebClient client;

        public ServiceWork()
        {
            client = new WebClient();
        }

        public void UpdateInfo(HardWare hard)
        {
            string data = JsonConvert.SerializeObject(hard);
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            var response = client.UploadString("http://localhost:51200/api/load", "POST", data);
        }
        public void GetInfo()
        {
            HardWare hard = new HardWare();
            using (StreamWriter writer = new StreamWriter("C:\\log.txt", true))
            {
                try
                {
                    ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                    ManagementObjectCollection moc = mc.GetInstances();
                    if (moc.Count != 0)
                    {
                        foreach (ManagementObject mo in mc.GetInstances())
                        {
                            hard.pc_name = mo["Name"].ToString();
                            hard.manufacturer = mo["Manufacturer"].ToString();
                        }
                    }

                    mc = new ManagementClass("Win32_UserAccount");
                    moc = mc.GetInstances();
                    if (moc.Count != 0)
                    {
                        List<string> users = new List<string>();
                        foreach (ManagementObject mo in mc.GetInstances())
                        {
                            if (mo["Status"].ToString().Contains("OK"))
                            {
                                users.Add(mo["Name"].ToString());
                            }
                        }
                        string res = String.Join(", ", users.ToArray());
                        hard.users = res;
                    }

                    mc = new ManagementClass("Win32_Processor");
                    moc = mc.GetInstances();
                    if (moc.Count != 0)
                    {
                        foreach (ManagementObject mo in mc.GetInstances())
                        {
                            hard.cpu = Int32.Parse(mo["LoadPercentage"].ToString());
                        }
                    }

                    mc = new ManagementClass("Win32_OperatingSystem");
                    moc = mc.GetInstances();
                    if (moc.Count != 0)
                    {
                        foreach (ManagementObject mo in mc.GetInstances())
                        {
                            double free_phys_memory = Double.Parse(mo["FreePhysicalMemory"].ToString());
                            double total_vis_memory = Double.Parse(mo["TotalVisibleMemorySize"].ToString());
                            int percent = (int)Math.Round((total_vis_memory - free_phys_memory) / total_vis_memory * 100);
                            hard.ram = percent;
                        }
                    }
                    hard.date = DateTime.Now.ToString();
                    UpdateInfo(hard);

                }catch (Exception ex)
                {
                    writer.WriteLine(ex);
                    writer.Flush();
                }
            }
        }
    }
}
