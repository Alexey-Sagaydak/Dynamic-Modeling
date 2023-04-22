using System;
using System.Collections.Generic;

namespace Dynamic_Modeling
{
    public enum Instruction
    {
        Increase, Decrease, None
    }

    public enum LineType
    {
        A, B
    }

    public class Factory
    {
        public Handler[] LineA { get; private set; }
        public Handler[] LineB { get; private set; }
        public Storage DetailsStorage { get; private set; }
        public AssemblyDepartment DetailsAssemblyDepartment { get; private set; }
        public int ModelingTime { get; private set; }
        public int MadeProductsAmount { get; private set; }

        private float alpha;
        private float MuCritical;
        private float delayTimeMin;
        private float delayTimeAverage;
        private float delayTimeMax;
        private int ADetailsAmountToMakeProduct;
        private int BDetailsAmountToMakeProduct;
        private int currentTime;

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
            
            this.MuCritical = deltaMu;
            this.alpha = alpha;
            this.delayTimeMin = delayTimeMin;
            this.delayTimeAverage = delayTimeAverage;
            this.delayTimeMax = delayTimeMax;
            this.ADetailsAmountToMakeProduct = ADetailsAmountToMakeProduct;
            this.BDetailsAmountToMakeProduct = BDetailsAmountToMakeProduct;

            currentTime = 1;
        }

        public void StartModeling()
        {
            for (; currentTime <= ModelingTime; currentTime++)
            {
                Dictionary<LineType, Instruction> Instructions = GetChangingLinesProcessingTimeInstructions();

                foreach (KeyValuePair<LineType, Instruction> instruction in Instructions)
                    ChangeLineProcessingTime(instruction.Key, instruction.Value);
            }
        }

        private void ChangeLineProcessingTime(LineType lineType, Instruction instruction)
        {
            Handler[] line = (lineType == LineType.A) ? LineA : LineB;
            
            if (instruction != Instruction.None)
            {
                float sign = (instruction == Instruction.Increase) ? 1 : -1;

                for (int i = 0; i < line.Length; i++)
                {
                    line[i].Delay[currentTime] = (int)Math.Round(delayTimeMin + delayTimeAverage * line[i].Queue[currentTime]
                        / line[i].Delay[currentTime - 1] + sign * delayTimeMax);
                }
            }
        }

        private Dictionary<LineType, Instruction> GetChangingLinesProcessingTimeInstructions()
        {
            Dictionary<LineType, Instruction> Instructions = new Dictionary<LineType, Instruction>();
            float deltaMu, muA, muB;

            muA = DetailsAssemblyDepartment.ADetailsAmount[currentTime] / ADetailsAmountToMakeProduct;
            muB = DetailsAssemblyDepartment.BDetailsAmount[currentTime] / BDetailsAmountToMakeProduct;
            deltaMu = Math.Abs(muA - muB);

            if (deltaMu <= MuCritical)
            {
                Instructions.Add(LineType.A, Instruction.None);
                Instructions.Add(LineType.B, Instruction.None);
            }
            else
            {
                if (muA > muB)
                {
                    Instructions.Add(LineType.A, Instruction.Decrease);
                    Instructions.Add(LineType.B, Instruction.Increase);
                }
                else
                {
                    Instructions.Add(LineType.A, Instruction.Increase);
                    Instructions.Add(LineType.B, Instruction.Decrease);
                }
            }

            return Instructions;
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
