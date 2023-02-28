using BlApi;
namespace BlImplementation
{
    /// <summary>
    /// A class that implements the ibl interface and returns all the main logical entities
    /// </summary>
    sealed internal class Bl : IBl
    {
        /// <summary>
        /// A method that returns the Product entity
        /// </summary>
        public IProduct Product {get;}=new BlImplementation.Product();

        /// <summary>
        /// A method that returns the order entity
        /// </summary>
        public IOrder Order {get;}=new BlImplementation.Order();

        /// <summary>
        /// A method that returns the cart entity
        /// </summary>
        public ICart Cart {get;}= new BlImplementation.Cart();

        /// <summary>
        /// A method that returns the user entity
        /// </summary>
        public IUser User { get; } = new BlImplementation.User();
    }
}
