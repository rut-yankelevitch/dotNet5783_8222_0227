namespace BO
{
    /// <summary>
    /// A class for enums
    /// </summary>
    public enum Category
    {
        Vacum_Cleaner, Cofee_Machine, Toaster, Iron, Mixer, Blender, None
    }

    public enum OrderStatus
    {
        Confirmed_Order, Send_Order, Provided_Order, None
    }

    public enum Filter
    {
        Filter_By_Category,Filter_By_Bigger_Than_Price,Filter_By_Smaller_Than_Price,None
    }
}
