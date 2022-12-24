﻿
/// <summary>
/// icrud generic interface that defines functions on the data entities
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICrud<T>
{
    /// <summary>
    /// A function that defines adding a data entity
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int Add(T obj);


    /// <summary>
    /// A function that defines update a data entity
    /// </summary>
    /// <param name="obj"></param>
    public void Update(T obj);


    /// <summary>
    /// A function that defines delete a data entity
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id);


    /// <summary>
    /// A function that defines returning a list of data entities by condition
    /// </summary>
    /// <returns></returns>
    public IEnumerable<T?> GetAll(Func<T?,bool>? predicate=null);


    /// <summary>
    /// A function that defines returning aentitie by condition
    /// </summary>
    /// <returns></returns>
    public T GetByCondition(Func<T?, bool>? predicate);
    
	

}
