using NutriFitLogBackend.Domain.Repositories;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure;

public class UnitOfWork : IUnitOfWork
{   
    //TODO Change repositories
    /*public IPermissionRepository PermissionRepository { get; set; }
    public IPermissionTypeRepository PermissionTypeRepository { get; set; }
    */

    private readonly NutriFitLogContext _nutriFitLogContext;

    public UnitOfWork(NutriFitLogContext nutriFitLogContext/*, IPermissionRepository permissionRepository, IPermissionTypeRepository permissionTypeRepository*/)
    {
        _nutriFitLogContext = nutriFitLogContext;

        /*PermissionRepository = permissionRepository;
        PermissionTypeRepository = permissionTypeRepository;*/
    }

    public async Task<int> SaveAsync() => await _nutriFitLogContext.SaveChangesAsync();

    public void Dispose() => _nutriFitLogContext.Dispose();
}