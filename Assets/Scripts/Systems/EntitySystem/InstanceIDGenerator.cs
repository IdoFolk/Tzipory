namespace Tzipory.Systems
{
    public static class InstanceIDGenerator
    {
        private static int _count = 0;

        public static int GetInstanceID()
        {
            int temp = _count;
            _count++;
            return temp;
        }
    }
}