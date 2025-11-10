using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ColorCheckState : IState
{
    public interface IHandler
    {
        public UniTask OnCheckEnter(CancellationToken ct);
        public UniTask OnCheckUpdate(CancellationToken ct);
        public UniTask OnCheckExit(CancellationToken ct);
    }
    public IHandler owner;

    public UniTask OnEnter(CancellationToken ct)
    {
        return owner.OnCheckEnter(ct);
    }

    public UniTask OnUpdate(CancellationToken ct)
    {
        return owner.OnCheckUpdate(ct);
    }

    public UniTask OnExit(CancellationToken ct)
    {
        return owner.OnCheckExit(ct);
    }
}

public partial class ColorSlot : ColorCheckState.IHandler
{
    public UniTask OnCheckEnter(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }

    public UniTask OnCheckUpdate(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }

    public UniTask OnCheckExit(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }
}
