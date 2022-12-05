
namespace BlApi
{
 /// <summary>
/// interface
/// </summary>
    public interface IBl
    {
        public IProduct Product { get; }
        public IOrder Order { get; }
        public ICart cart { get; }
    }
}