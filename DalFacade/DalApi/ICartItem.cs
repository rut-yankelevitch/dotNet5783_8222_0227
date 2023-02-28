using System.Runtime.CompilerServices;
namespace DalApi;

public interface ICartItem : ICrud<DO.CartItem>
{
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(Func<DO.CartItem, bool> f);
}

