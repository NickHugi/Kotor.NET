namespace KotorDotNET.Patching
{
    public class Memory
    {
        private Dictionary<int, string> twodaMemory = new();
        private Dictionary<int, int> tlkMemory = new();

        public string From2DAToken(int tokenID)
        {
            return twodaMemory[tokenID];
        }

        public int FromTLKToken(int tokenID)
        {
            return tlkMemory[tokenID];
        }

        public void Set2DAToken(int tokenID, string value)
        {
            twodaMemory[tokenID] = value;
        }

        public void SetTLKToken(int tokenID, int value)
        {
            tlkMemory[tokenID] = value;
        }
    }
}
