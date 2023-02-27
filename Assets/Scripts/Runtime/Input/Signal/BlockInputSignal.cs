namespace Runtime.Input.Signal
{
    public class BlockInputSignal
    {
        public bool IsBlockInput { get; set; }

        public BlockInputSignal(bool isBlockInput) { this.IsBlockInput = isBlockInput; }
    }
}