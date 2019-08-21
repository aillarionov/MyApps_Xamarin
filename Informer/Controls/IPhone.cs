using System.Threading.Tasks;

namespace Informer.Controls
{
    public interface IPhone
    {
        Task Call(string phoneNumber);
    }
}