namespace ByteSchoolManager.Repository;

public interface IRepository<T>
{
    public List<T> GetAll();
    public T? GetById(int id);
    public int? Create(T entity);
    public bool Update(T entity);
    public bool Delete(int entityId);
}