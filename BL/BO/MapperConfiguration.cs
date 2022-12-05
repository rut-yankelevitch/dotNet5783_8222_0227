namespace BO
{
    internal class MapperConfiguration
    {
        private Func<object, object> value;

        public MapperConfiguration(Func<object, object> value)
        {
            this.value = value;
        }
    }
}