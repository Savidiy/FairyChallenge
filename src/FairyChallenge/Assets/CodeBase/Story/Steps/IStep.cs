using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Fairy
{
    public interface IStep
    {
        UniTask Execute(CancellationToken token);
    }
}