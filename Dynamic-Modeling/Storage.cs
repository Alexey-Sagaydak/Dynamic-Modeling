namespace Dynamic_Modeling
{
    public struct Storage
    {
        public Handler ADetails { get; set; }
        public Handler BDetails { get; set; }

        public Storage(int modelingTime, int startADetailsAmount, int startBDetailsAmount, int startProcessingTime)
        {
            ADetails = new Handler(modelingTime);
            BDetails = new Handler(modelingTime);

            ADetails.Queue[0] = startADetailsAmount;
            BDetails.Queue[0] = startBDetailsAmount;

            ADetails.ProcessingTime[0] = startProcessingTime;
            BDetails.ProcessingTime[0] = startProcessingTime;
        }
    }
}
