namespace TransitionApp.Domain.Model.ValueObject
{
    public class Duration
    {
        public double Value { get; }
        public Duration(double value)
        {
            Value = value;
        }
    }
}
