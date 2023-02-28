namespace BlApi;

public interface IUser
{
    /// <summary>
    /// Definition of a function add User
    /// </summary>
    /// <param name="u"></param>
    /// <returns>int?</returns>
    public int? AddUser(BO.User u);


    /// <summary>
    /// Definition of a function update user 
    /// </summary>
    /// <param name="u"></param>
    public void UpdateUser(BO.User u);


    /// <summary>
    /// Definition of a function check if user Is Registered
    /// </summary>
    /// <param name="email"></param>
    /// <param name="pass"></param>
    /// <returns>g=int?</returns>
    public int? IsRegistered(string email, string pass);

}
