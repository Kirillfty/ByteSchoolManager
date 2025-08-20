namespace ByteSchoolManager.Repository;

public interface IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken ct = default);
}

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken ct = default) => await context.SaveChangesAsync(ct);
}