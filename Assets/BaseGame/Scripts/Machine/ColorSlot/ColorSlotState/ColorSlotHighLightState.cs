using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ColorSlotHighLightState : IState
{
    public interface IHandler
    {
        public UniTask OnHighLightEnter(CancellationToken ct);
        public UniTask OnHighLightUpdate(CancellationToken ct);
        public UniTask OnHighLightExit(CancellationToken ct);
    }
    public IHandler owner;
    public UniTask OnEnter(CancellationToken ct)
    {
        return owner.OnHighLightEnter(ct);
    }

    public UniTask OnUpdate(CancellationToken ct)
    {
        return owner.OnHighLightUpdate(ct);
    }

    public UniTask OnExit(CancellationToken ct)
    {
        return owner.OnHighLightExit(ct);
    }
}

public partial class ColorSlot : ColorSlotHighLightState.IHandler
{
    private ColorSlotHighLightState colorSlotHighLightState { get; set; }
    public ColorSlotHighLightState ColorSlotHighLightState => colorSlotHighLightState ??= new ColorSlotHighLightState { owner = this };
    
    public UniTask OnHighLightEnter(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }

    public UniTask OnHighLightUpdate(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }

    public UniTask OnHighLightExit(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }
}
