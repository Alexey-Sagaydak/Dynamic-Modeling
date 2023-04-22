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
        public int MadeProductsAmount { get; private set; }

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

        public Factory(int modelingTime, int startProcessingTime, int startADetailsToStorage,
            int startBDetailsToStorage, int startHandlersQueueAmount, int startProcessedADetailsAmount,
            int startProcessedBDetailsAmount, int ADetailsAmountToMakeProduct, int BDetailsAmountToMakeProduct,
            int delayTimeMin, int delayTimeAverage, int delayTimeMax, float muCritical, float alpha)
        {
            if (modelingTime <= 0)
                throw new ArgumentOutOfRangeException(nameof(modelingTime));

            ModelingTime = modelingTime;

            LineA = new Handler[TypeAHandlersAmount];
            LineB = new Handler[TypeBHandlersAmount];
            DetailsAssemblyDepartment = new AssemblyDepartment(modelingTime, startProcessedADetailsAmount, startProcessedBDetailsAmount);
            
            InitializeLines(LineA, modelingTime, startHandlersQueueAmount, startProcessingTime, startADetailsToStorage);
            InitializeLines(LineB, modelingTime, startHandlersQueueAmount, startProcessingTime, startBDetailsToStorage);
            
            this.muCritical = muCritical;
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
                {
                    // Пересчет уровней будет здесь...

                    ChangeLineProcessingTime(instruction.Key, instruction.Value);

                    if (!(AddDetailsToStorage(LineA) && AddDetailsToStorage(LineB)))
                        break;
                }
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

        private void ChangeLineProcessingTime(LineType lineType, Instruction instruction)
        {
            Handler[] line = (lineType == LineType.A) ? LineA : LineB;
            
            float sign = (instruction == Instruction.Increase) ? 1 : -1;

            for (int i = 0; i < line.Length; i++)
            {
                if (instruction != Instruction.None)
                {
                    line[i].Delay[currentTime] = (int)Math.Round(delayTimeMin + delayTimeAverage * line[i].Queue[currentTime]
                        / line[i].Delay[currentTime - 1] + sign * alpha * delayTimeMax);

                    line[i].ProcessingTime[currentTime] = (int)Math.Round(line[i].Queue[currentTime] / (float)line[i].Delay[currentTime]);
                }
                else
                {
                    line[i].Delay[currentTime] = line[i].Delay[currentTime - 1];
                    line[i].ProcessingTime[currentTime] = line[i].ProcessingTime[currentTime - 1];
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

            if (deltaMu <= muCritical)
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
            int startProcessingTime, int startDetailsAmount)
        {
            for (int i = 0; i < line.Length; i++)
            {
                line[i] = new Handler(modelingTime);
                line[i].Queue[0] = startHandlersQueueAmount;
                line[i].ProcessingTime[0] = startProcessingTime;
            }

            line[0].Queue[0] = startDetailsAmount;
        }
    }
}
