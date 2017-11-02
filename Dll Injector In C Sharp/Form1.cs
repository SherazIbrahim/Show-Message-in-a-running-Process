using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
namespace Dll_Injector_In_C_Sharp
{
    public partial class Form1 : Form
    {

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int AccessType, bool InheritHandle, int ProcessId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string IpModuleName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr ModuleHandle, string processname);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hprocess, IntPtr ipaddress, int dwsize, uint flallocationtype, uint flprotect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hprocess, IntPtr IpBaseAddress, byte[] IpBuffer, int nsize, out UIntPtr IpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hprocess, IntPtr IpThreadAttributes, uint dwstakesize, IntPtr IpStartAddress, IntPtr IpParameter, uint dwCreationFlags, IntPtr IpThreadId);



        const int PROCESS_CREATETHREAD = 0X002;
        const int PROCESS_QUERY_INFORMATION = 0X0400;
        const int PROCESS_VM_OPERATIONS = 0X0008;
        const int PROCESS_VM_WRITE = 0X0020;
        const int PROCESS_VM_READ = 0X0010;

        const uint MEM_COMMIT = 0X00001000;
        const uint MEM_RESERVE = 0X0002000;
        const uint PAGE_READWRITE = 4;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    textBox2.Text = openFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void inject(String Processname)
        {
            try
            {
                if (textBox1.Text.Length > 0)
                {
                    string path;
                    path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\SMP.dll";
                    File.WriteAllBytes(path, Properties.Resources.ShowMsgInProcess);
                    IntPtr LoadLibraryA = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                    Process processm = Process.GetProcessesByName(Processname)[0];
                    IntPtr ProcessHandle = OpenProcess(PROCESS_CREATETHREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATIONS | PROCESS_VM_WRITE | PROCESS_VM_READ, false, processm.Id);
                    byte[] bytes = Encoding.ASCII.GetBytes(path);
                    IntPtr allocateMemAddress = VirtualAllocEx(ProcessHandle, IntPtr.Zero, bytes.Length + 1, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
                    UIntPtr bytesWritten;
                    WriteProcessMemory(ProcessHandle, allocateMemAddress, bytes, bytes.Length, out bytesWritten);
                    CreateRemoteThread(ProcessHandle, IntPtr.Zero, 0, LoadLibraryA, allocateMemAddress, 0, IntPtr.Zero);
                    MessageBox.Show("Dll Injected.....");
                }
                else
                {
                    MessageBox.Show("Fill all Boxes........");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
            private void button2_Click(object sender, EventArgs e)
        {
          
            inject(textBox1.Text);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text.Length > 0)
                {
                    string path;
                    path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Message.txt";
                    File.WriteAllText(path, textBox2.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

