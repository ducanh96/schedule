using System.Threading.Tasks;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel.Account;

namespace TransitionApp.Domain.Interface.Repository
{
    public interface IAccountRepository
    {
        bool ResetPassword(Account account);
        AccountReadModel Get(string userName, string password);
        bool IsExixtAccount(string userName);
        bool UpdateAccount(string userName, string password);
    }
}
