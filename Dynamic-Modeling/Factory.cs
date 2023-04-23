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
        public AssemblyDepartment DetailsAssemblyDepartment { get; private set; }
        public int ModelingTime { get; private set; }
        public int[] MadeProductsAmount { get; private set; }

        private float alpha;
        private float muCritical;
        private float delayTimeMin;
        private float delayTimeAverage;
        private float delayTimeMax;
        private int ADetailsAmountToMakeProduct;
        private int BDetailsAmountToMakeProduct;
        private int currentTime;

        private readonly int TypeAHandlersAmount = 4;
        private readonly int TypeBHandlersAmount = 3;

        public Factory(int modelingTime, int startDetailsPerTact, int startADetailsToStorage,
            int startBDetailsToStorage, int startHandlersQueueAmount, int startProcessedADetailsAmount,
            int startProcessedBDetailsAmount, int ADetailsAmountToMakeProduct, int BDetailsAmountToMakeProduct,
            int delayTimeMin, int delayTimeAverage, int delayTimeMax, float muCritical, float alpha)
        {
            if (modelingTime <= 0)
                throw new ArgumentOutOfRangeException(nameof(modelingTime));

            ModelingTime = modelingTime;

            LineA = new Handler[TypeAHandlersAmount];
            LineB = new Handler[TypeBHandlersAmount];
            MadeProductsAmount = new int[modelingTime];
            DetailsAssemblyDepartment = new AssemblyDepartment(modelingTime, startProcessedADetailsAmount, startProcessedBDetailsAmount);
            
            InitializeLines(LineA, modelingTime, startHandlersQueueAmount, startDetailsPerTact, startADetailsToStorage, delayTimeAverage);
            InitializeLines(LineB, modelingTime, startHandlersQueueAmount, startDetailsPerTact, startBDetailsToStorage, delayTimeAverage);
            
            this.muCritical = muCritical;
            this.alpha = alpha;
            this.delayTimeMin = delayTimeMin;
            this.delayTimeAverage = delayTimeAverage;
            this.delayTimeMax = delayTimeMax;
            this.ADetailsAmountToMakeProduct = ADetailsAmountToMakeProduct;
            this.BDetailsAmountToMakeProduct = BDetailsAmountToMakeProduct;

            currentTime = 1;
        }

        public bool StartModeling()
        {
            for (; currentTime < ModelingTime; currentTime++)
            {
                Dictionary<LineType, Instruction> Instructions = GetChangingLinesProcessingTimeInstructions();
                //Dictionary<LineType, Instruction> Instructions = new Dictionary<LineType, Instruction>()
                //{
                //    { LineType.A, Instruction.None },
                //    { LineType.B, Instruction.None }
                //};


                foreach (KeyValuePair<LineType, Instruction> instruction in Instructions)
                    ChangeLinesQueue(instruction.Key);

                foreach (KeyValuePair<LineType, Instruction> instruction in Instructions)
                    ChangeLineDetailsPerTact(instruction.Key, instruction.Value);

                MakeProducts();

                if (!(AddDetailsToStorage(LineA) && AddDetailsToStorage(LineB))) ;
                    //return false;
            }

            return true;
        }

        private void MakeProducts()
        {
            MadeProductsAmount[currentTime] = MadeProductsAmount[currentTime - 1];

            while (DetailsAssemblyDepartment.ADetailsAmount[currentTime] >= ADetailsAmountToMakeProduct
                && DetailsAssemblyDepartment.BDetailsAmount[currentTime] >= BDetailsAmountToMakeProduct)
            {
                MadeProductsAmount[currentTime]++;

                DetailsAssemblyDepartment.ADetailsAmount[currentTime] -= ADetailsAmountToMakeProduct;
                DetailsAssemblyDepartment.BDetailsAmount[currentTime] -= BDetailsAmountToMakeProduct;
            }
        }

        private void ChangeLinesQueue(LineType lineType)
        {
            Handler[] line = (lineType == LineType.A) ? LineA : LineB;
            int detailsToMove;

            line[0].Queue[currentTime] = line[0].Queue[currentTime - 1];

            for (int i = 1; i <= line.Length; i++)
            {
                detailsToMove = ((line[i - 1].Queue[currentTime] - line[i - 1].DetailsPerTact[currentTime - 1]) >= 0)
                    ? line[i - 1].DetailsPerTact[currentTime - 1]
                    : line[i - 1].Queue[currentTime];

                line[i - 1].Queue[currentTime] -= detailsToMove;

                if (i != line.Length)
                    line[i].Queue[currentTime] = line[i].Queue[currentTime - 1] + detailsToMove;
                else
                    DetailsAssemblyDepartment[lineType][currentTime] = DetailsAssemblyDepartment[lineType][currentTime - 1] + detailsToMove;
            }
        }

        private bool AddDetailsToStorage(Handler[] line)
        {
            if (line[0].Queue[currentTime] < 0.05f * line[0].Queue[0])
                return false;

            if (line[0].Queue[currentTime] < 0.2f * line[0].Queue[0])
                line[0].Queue[currentTime] += GetNewDetails();

            return true;
        }

        private int GetNewDetails()
        {
            return 500;
        }

        private void ChangeLineDetailsPerTact(LineType lineType, Instruction instruction)
        {
            Handler[] line = (lineType == LineType.A) ? LineA : LineB;
            
            float sign = (instruction == Instruction.Increase) ? 1 : -1;

            for (int i = 0; i < line.Length; i++)
            {
                if (instruction != Instruction.None)
                {
                    line[i].Delay[currentTime] = (int)Math.Ceiling((delayTimeMin + delayTimeAverage * line[i].Queue[currentTime]
                        / line[i].Delay[currentTime - 1] + sign * alpha * delayTimeMax));

                    // НИЧЕГО НЕ РАБОТАЕТ!!!!!!!!!!!!!!!

                    if (line[i].Delay[currentTime] <= delayTimeMin)
                        line[i].Delay[currentTime] = (int)delayTimeMin;

                    if (lineType == LineType.A && i == 0) ;
                    Console.WriteLine($"{line[i].Queue[currentTime]} {line[i].Delay[currentTime - 1]}");

                    line[i].DetailsPerTact[currentTime] = (int)Math.Ceiling(line[i].Queue[currentTime] / (float)line[i].Delay[currentTime]);
                }
                else
                {
                    line[i].Delay[currentTime] = line[i].Delay[currentTime - 1];
                    line[i].DetailsPerTact[currentTime] = line[i].DetailsPerTact[currentTime - 1];
                }
            }
        }

        private Dictionary<LineType, Instruction> GetChangingLinesProcessingTimeInstructions()
        {
            Dictionary<LineType, Instruction> Instructions = new Dictionary<LineType, Instruction>();
            float deltaMu, muA, muB;

            muA = (float)DetailsAssemblyDepartment.ADetailsAmount[currentTime - 1] / ADetailsAmountToMakeProduct;
            muB = (float)DetailsAssemblyDepartment.BDetailsAmount[currentTime - 1] / BDetailsAmountToMakeProduct;
            deltaMu = Math.Abs(muA - muB);

            if (deltaMu <= muCritical)
            {
                Instructions.Add(LineType.A, Instruction.None);
                Instructions.Add(LineType.B, Instruction.None);
            }
            else
            {
                if (muA < muB)
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
            int startDetailsPerTact, int startDetailsAmount, int delayTimeAverage)
        {
            for (int i = 0; i < line.Length; i++)
            {
                line[i] = new Handler(modelingTime);
                line[i].Queue[0] = startHandlersQueueAmount;
                line[i].DetailsPerTact[0] = startDetailsPerTact;
                line[i].Delay[0] = delayTimeAverage;
            }

            line[0].Queue[0] = startDetailsAmount;
        }
    }
}
