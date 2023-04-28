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

            BuildAQueueAtStorage();
            BuildBQueueAtStorage();
            BuildAQueueAtAssemblyDepartment();
            BuildBQueueAtAssemblyDepartment();
            BuildAQueueAtLastHandler();
            BuildBQueueAtLastHandler();
            BuildDetailsAmount();
            BuildADetailsPerTactAtLastHandler();
            BuildBDetailsPerTactAtLastHandler();
        }

        private void BuildAQueueAtStorage()
        {
            for (int i = 0; i < viewModel.Factory.ModelingTime; i++)
                viewModel.Points[i] = new Point<float>(i, viewModel.Factory.LineA[0].Queue[i]);

            BuildPlot1(viewModel.Points, "Уровень дет. A на складе", ChartDashStyle.Dash, Color.Black, 2);
        }

        private void BuildBQueueAtStorage()
        {
            for (int i = 0; i < viewModel.Factory.ModelingTime; i++)
                viewModel.Points[i] = new Point<float>(i, viewModel.Factory.LineB[0].Queue[i]);

            BuildPlot1(viewModel.Points, "Уровень дет. B на складе", ChartDashStyle.Dash, Color.Brown, 2);
        }

        private void BuildAQueueAtAssemblyDepartment()
        {
            for (int i = 0; i < viewModel.Factory.ModelingTime; i++)
                viewModel.Points[i] = new Point<float>(i, viewModel.Factory.DetailsAssemblyDepartment.ADetailsAmount[i]);

            BuildPlot1(viewModel.Points, "Уровень дет. A в сб. отделе", ChartDashStyle.Solid, Color.Blue, 1);
        }

        private void BuildBQueueAtAssemblyDepartment()
        {
            for (int i = 0; i < viewModel.Factory.ModelingTime; i++)
                viewModel.Points[i] = new Point<float>(i, viewModel.Factory.DetailsAssemblyDepartment.BDetailsAmount[i]);

            BuildPlot1(viewModel.Points, "Уровень дет. B в сб. отделе", ChartDashStyle.Solid, Color.Red, 1);
        }

        private void BuildAQueueAtLastHandler()
        {
            for (int i = 0; i < viewModel.Factory.ModelingTime; i++)
                viewModel.Points[i] = new Point<float>(i, viewModel.Factory.LineA[3].Queue[i]);

            BuildPlot3(viewModel.Points, "Уровень дет. A на последнем обработчике", ChartDashStyle.Solid, Color.DeepPink, 2);
        }

        private void BuildBQueueAtLastHandler()
        {
            for (int i = 0; i < viewModel.Factory.ModelingTime; i++)
                viewModel.Points[i] = new Point<float>(i, viewModel.Factory.LineB[2].Queue[i]);

            BuildPlot3(viewModel.Points, "Уровень дет. B на последнем обработчике", ChartDashStyle.Solid, Color.YellowGreen, 2);
        }

        private void BuildDetailsAmount()
        {
            for (int i = 0; i < viewModel.Factory.ModelingTime; i++)
                viewModel.Points[i] = new Point<float>(i, viewModel.Factory.MadeProductsAmount[i]);

            BuildPlot2(viewModel.Points, "Изготовлено изделий", ChartDashStyle.Solid, Color.Black, 1, SeriesChartType.Column);
        }

        private void BuildADetailsPerTactAtLastHandler()
        {
            for (int i = 0; i < viewModel.Factory.ModelingTime; i++)
                viewModel.Points[i] = new Point<float>(i, viewModel.Factory.LineA[3].DetailsPerTact[i]);

            BuildPlot3(viewModel.Points, "Темпы дет. A на последнем обработчике", ChartDashStyle.Dash, Color.DimGray, 2);
        }

        private void BuildBDetailsPerTactAtLastHandler()
        {
            for (int i = 0; i < viewModel.Factory.ModelingTime; i++)
                viewModel.Points[i] = new Point<float>(i, viewModel.Factory.LineB[2].DetailsPerTact[i]);

            BuildPlot3(viewModel.Points, "Темпы дет. B на последнем обработчике", ChartDashStyle.Dash, Color.Brown, 2);
        }

        private void BuildPlot1(Point<float>[] points, string name, ChartDashStyle style, Color color,
            int borderWidth, SeriesChartType seriesChartType = SeriesChartType.Spline)
        {
            chart1.Series.Add(name);
            chart1.Series.FindByName(name).BorderDashStyle = style;
            chart1.Series.FindByName(name).ChartType = seriesChartType;
            chart1.Series.FindByName(name).Color = color;
            chart1.Series.FindByName(name).BorderWidth = borderWidth;
            chart1.Series.FindByName(name).MarkerSize = borderWidth;


            for (int i = 0; i < points.Length; i++)
                chart1.Series.FindByName(name).Points.AddXY(points[i].X, points[i].Y);
        }

        private void BuildPlot2(Point<float>[] points, string name, ChartDashStyle style, Color color,
            int borderWidth, SeriesChartType seriesChartType = SeriesChartType.Spline)
        {
            chart2.Series.Add(name);
            chart2.Series.FindByName(name).BorderDashStyle = style;
            chart2.Series.FindByName(name).ChartType = seriesChartType;
            chart2.Series.FindByName(name).Color = color;
            chart2.Series.FindByName(name).BorderWidth = borderWidth;
            chart2.Series.FindByName(name).MarkerSize = borderWidth;


            for (int i = 0; i < points.Length; i++)
                chart2.Series.FindByName(name).Points.AddXY(points[i].X, points[i].Y);
        }

        private void BuildPlot3(Point<float>[] points, string name, ChartDashStyle style, Color color,
            int borderWidth, SeriesChartType seriesChartType = SeriesChartType.Spline)
        {
            chart3.Series.Add(name);
            chart3.Series.FindByName(name).BorderDashStyle = style;
            chart3.Series.FindByName(name).ChartType = seriesChartType;
            chart3.Series.FindByName(name).Color = color;
            chart3.Series.FindByName(name).BorderWidth = borderWidth;
            chart3.Series.FindByName(name).MarkerSize = borderWidth;


            for (int i = 0; i < points.Length; i++)
                chart3.Series.FindByName(name).Points.AddXY(points[i].X, points[i].Y);
        }
    }
}
