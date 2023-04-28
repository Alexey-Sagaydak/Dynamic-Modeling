using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Dynamic_Modeling
{
    public partial class ChartForm : Form
    {
        private FactoryViewModel viewModel;

        public ChartForm()
        {
            InitializeComponent();

            viewModel = new FactoryViewModel();
            /*
             * Построить графики:
             * Уровень деталей А и В на складе и в сборочном отделе
             * Уровень и темпы деталей на последнем станке каждой линии
             * Итого: 8 графиков
             */
        }

        private void BuildPlot(Point<float>[] points, string name, ChartDashStyle style, Color color,
            int borderWidth, SeriesChartType seriesChartType = SeriesChartType.Spline)
        {
            chart.Series.Add(name);
            chart.Series.FindByName(name).BorderDashStyle = style;
            chart.Series.FindByName(name).ChartType = seriesChartType;
            chart.Series.FindByName(name).Color = color;
            chart.Series.FindByName(name).BorderWidth = borderWidth;
            chart.Series.FindByName(name).MarkerSize = borderWidth;


            for (int i = 0; i < points.Length; i++)
                chart.Series.FindByName(name).Points.AddXY(points[i].X, points[i].Y);
        }

        private void UpdatePointsInfo()
        {
            StringBuilder sr = new StringBuilder();

            foreach (Point<float> point in viewModel.Points)
            {
                sr.Append($"({point.X}; {point.Y})\n");
            }
        }
    }
}
