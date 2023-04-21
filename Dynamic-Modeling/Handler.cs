using System;

namespace Dynamic_Modeling
{
    public struct Handler
    {
        public int[] ProcessingTime {get; set; }
        public int[] Queue { get; set; }
        public int[] Delay { get; set; }

        public Handler(int modelingTime)
        {
            if (modelingTime <= 0)
                throw new ArgumentOutOfRangeException(nameof(modelingTime));

            ProcessingTime = new int[modelingTime];
            Queue = new int[modelingTime];
            Delay = new int[modelingTime];
        }
    }
}
