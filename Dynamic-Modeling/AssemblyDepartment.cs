namespace Dynamic_Modeling
{
    public struct AssemblyDepartment
    {
        public int[] ADetailsAmount;
        public int[] BDetailsAmount;

        public AssemblyDepartment(int modelingTime, int startADetailsAmount, int startBDetailsAmount)
        {
            ADetailsAmount = new int[modelingTime];
            BDetailsAmount = new int[modelingTime];

            ADetailsAmount[0] = startADetailsAmount;
            BDetailsAmount[0] = startBDetailsAmount;
        }

        public int[] this[LineType lineType]
        {
            get
            {
                if (lineType == LineType.A)
                    return ADetailsAmount;
                else
                    return BDetailsAmount;
            }
        }
    }
}
