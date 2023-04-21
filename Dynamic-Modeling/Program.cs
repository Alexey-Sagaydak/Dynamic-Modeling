using System;

namespace Dynamic_Modeling
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            Factory factory = new Factory(5, 20, 700, 1000, 70, 30, 40, 200, 300, 4, 12, 20, 0.1f, 0.5f);
        }
    }
}
