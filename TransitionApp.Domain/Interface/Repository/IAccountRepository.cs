using TransitionApp.Domain.ReadModel.Account;

namespace TransitionApp.Domain.Interface.Repository
{
    public interface IAccountRepository
    {
        AccountReadModel Get(string userName, string password);
        bool IsExixtAccount(string userName);
    }
}
