using System.Threading.Tasks;

namespace YoCli.Commons
{
    interface ICommand
    {
        Task<int> OnExecute();
    }
}
