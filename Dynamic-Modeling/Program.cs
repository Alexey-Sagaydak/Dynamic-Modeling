using System;
using System.Windows.Forms;

namespace Dynamic_Modeling
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new ChartForm());

            Factory factory = new Factory(100, 70, 700, 1000, 200, 30, 40, 200, 300, 4, 12, 20, 0.1f, 0.5f);
            factory.StartModeling();
        }
    }
}
