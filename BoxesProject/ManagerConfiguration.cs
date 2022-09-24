using System;

namespace BoxesProject
{
    public class ManagerConfiguration
    {
        public static readonly ManagerConfiguration Default = new ManagerConfiguration(100, 20, 0.5, TimeSpan.FromSeconds(3600), TimeSpan.FromSeconds(360));

        public int MaximumAmountAllowed { get; private set; }
        public int MinimumAmountAllowed { get; private set; }
        public double MaximumExceedanceAllowed { get; private set; }
        public TimeSpan Validity { get; private set; }
        public TimeSpan TimerInterval { get; private set; }

        public ManagerConfiguration(int maximumAmountAllowed, int minimumAmountAllowed, double maximumExceedanceAllowed, TimeSpan validity, TimeSpan timerInterval)
        {
            PositiveIntValidate(maximumAmountAllowed);
            PositiveIntValidate(minimumAmountAllowed);
            if (minimumAmountAllowed >= maximumAmountAllowed) throw new ArgumentException("Minimum value cannot be bigger or equals to Maximum Value.");
            NonNegativeDoubleValidate(maximumExceedanceAllowed);
            TimeSpanValidate(validity);
            TimeSpanValidate(timerInterval);

            MaximumAmountAllowed = maximumAmountAllowed;
            MinimumAmountAllowed = minimumAmountAllowed;
            MaximumExceedanceAllowed = maximumExceedanceAllowed;
            Validity = validity;
            TimerInterval = timerInterval;
        }

        private static void TimeSpanValidate(TimeSpan time)
        {
            if (time <= TimeSpan.Zero) throw new ArgumentException("Time cannot be zero or negative.");
        }

        private static void NonNegativeDoubleValidate(double number)
        {
            if (number < 0) throw new ArgumentException("MaximumExceedanceAllowed cannot be negative");
        }

        private static void PositiveIntValidate(int number)
        {
            if (number <= 0) throw new ArgumentException("Maximum /Minimum must be positive");
        }
    }
}
