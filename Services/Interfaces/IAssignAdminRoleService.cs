namespace WebApplication2.Services.Interfaces
{
    public interface IAssignAdminRoleService
    {
        Task<string> AssignAdminRole(string userName);
    }
}
