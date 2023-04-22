using System;

namespace Dynamic_Modeling
{
    public struct Handler
    {
        public int[] DetailsPerTact {get; set; }
        public int[] Queue { get; set; }
        public int[] Delay { get; set; }

        public Handler(int modelingTime)
        {
            if (modelingTime <= 0)
                throw new ArgumentOutOfRangeException(nameof(modelingTime));

            DetailsPerTact = new int[modelingTime];
            Queue = new int[modelingTime];
            Delay = new int[modelingTime];
        }
    }
}
