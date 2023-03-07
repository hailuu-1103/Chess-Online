namespace Runtime.Input.Signal
{
    public class OutOfTimeSignal
    {
        public bool IsWhiteTime { get; set; }

        public OutOfTimeSignal(bool isWhiteTime) { this.IsWhiteTime = isWhiteTime; }
    }
}