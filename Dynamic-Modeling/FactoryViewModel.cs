namespace Dynamic_Modeling
{
    public class FactoryViewModel
    {
        public Factory Factory;
        public Point<float>[] Points;

        public FactoryViewModel()
        {
            Factory = new Factory(100, 20, 700, 1000, 70, 30, 40, 200, 300, 4, 12, 20, 0.1f, 0.5f);
            Points = new Point<float>[Factory.ModelingTime];

            Factory.StartModeling();
        }
    }
}
