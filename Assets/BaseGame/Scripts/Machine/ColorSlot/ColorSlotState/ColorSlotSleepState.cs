using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ColorSlotSleepState : IState
{
    
    public interface IHandler
    {
        public UniTask OnSleepEnter(CancellationToken ct);
        public UniTask OnSleepUpdate(CancellationToken ct);
        public UniTask OnSleepExit(CancellationToken ct);
    }
    
    public IHandler owner { get; set; }

    public UniTask OnEnter(CancellationToken ct)
    {
        return owner.OnSleepEnter(ct);
    }

    public UniTask OnUpdate(CancellationToken ct)
    {
        return owner.OnSleepUpdate(ct);
    }

    public UniTask OnExit(CancellationToken ct)
    {
        return owner.OnSleepExit(ct);
    }
}

public partial class ColorSlot : ColorSlotSleepState.IHandler
{
    private ColorSlotSleepState colorSlotSleepStateCache { get; set; }
    public ColorSlotSleepState ColorSlotSleepState => colorSlotSleepStateCache ??= new ColorSlotSleepState { owner = this };
    public UniTask OnSleepEnter(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }

    public UniTask OnSleepUpdate(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }

    public UniTask OnSleepExit(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }
}
