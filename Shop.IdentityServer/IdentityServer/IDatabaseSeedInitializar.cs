namespace Shop.IdentityServer.IdentityServer
{
    public interface IDatabaseSeedInitializar
    {
        Task InitializeSeedRoles();
        Task InitializeSeedUsers();
    }
}
