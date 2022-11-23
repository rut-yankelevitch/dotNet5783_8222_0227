using System;
using System.Collections;

public interface ICrud<T>
{
	public int Add(T obj);
	public void Update(T obj);
	public void Delete(int id);
	public T GetById(int id);
	//לשאול
    public IEnumerable<T> GetAll() ;
	

}
