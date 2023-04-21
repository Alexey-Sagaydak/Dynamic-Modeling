using System;

namespace Dynamic_Modeling
{
    public class Factory
    {
        public Handler[] LineA { get; private set; }
        public Handler[] LineB { get; private set; }
        public Storage DetailsStorage { get; private set; }
        public AssemblyDepartment DetailsAssemblyDepartment { get; private set; }
        public int ModelingTime { get; private set; }
        public int MadeProductsAmount { get; private set; }

        private float alpha;
        private float deltaMu;
        private float delayTimeMin;
        private float delayTimeAverage;
        private float delayTimeMax;
        private int ADetailsAmountToMakeProduct;
        private int BDetailsAmountToMakeProduct;

        private readonly int TypeADetails = 0;
        private readonly int TypeBDetails = 1;
        private readonly int TypeAHandlersAmount = 3;
        private readonly int TypeBHandlersAmount = 2;

        public Factory(int modelingTime, int startProcessingTime, int startADetailsAtStorage,
            int startBDetailsAtStorage, int startHandlersQueueAmount, int startProcessedADetailsAmount,
            int startProcessedBDetailsAmount, int ADetailsAmountToMakeProduct, int BDetailsAmountToMakeProduct,
            int delayTimeMin, int delayTimeAverage, int delayTimeMax, float deltaMu, float alpha)
        {
            if (modelingTime <= 0)
                throw new ArgumentOutOfRangeException(nameof(modelingTime));

            ModelingTime = modelingTime;

            LineA = new Handler[TypeAHandlersAmount];
            LineB = new Handler[TypeBHandlersAmount];
            DetailsStorage = new Storage(modelingTime, startADetailsAtStorage, startBDetailsAtStorage, startProcessingTime);
            DetailsAssemblyDepartment = new AssemblyDepartment(modelingTime, startProcessedADetailsAmount, startProcessedBDetailsAmount);
            
            InitializeLines(LineA, modelingTime, startHandlersQueueAmount, startProcessingTime);
            InitializeLines(LineB, modelingTime, startHandlersQueueAmount, startProcessingTime);
            
            this.deltaMu = deltaMu;
            this.alpha = alpha;
            this.delayTimeMin = delayTimeMin;
            this.delayTimeAverage = delayTimeAverage;
            this.delayTimeMax = delayTimeMax;
            this.ADetailsAmountToMakeProduct = ADetailsAmountToMakeProduct;
            this.BDetailsAmountToMakeProduct = BDetailsAmountToMakeProduct;
        }

        private void InitializeLines(Handler[] line, int modelingTime, int startHandlersQueueAmount,
            int startProcessingTime)
        {
            for (int i = 0; i < line.Length; i++)
            {
                line[i] = new Handler(modelingTime);
                line[i].Queue[0] = startHandlersQueueAmount;
                line[i].ProcessingTime[0] = startProcessingTime;
            }
        }
    }
}
