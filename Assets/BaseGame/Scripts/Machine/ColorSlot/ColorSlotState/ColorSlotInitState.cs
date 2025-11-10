using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ColorSlotInitState : IState
{
    public interface IHandler
    {
        public UniTask OnInitEnter(CancellationToken ct);
        public UniTask OnInitUpdate(CancellationToken ct);
        public UniTask OnInitExit(CancellationToken ct);
    }
    public IHandler owner;
    public UniTask OnEnter(CancellationToken ct)
    {
        return owner.OnInitEnter(ct);
    }

    public UniTask OnUpdate(CancellationToken ct)
    {
        return owner.OnInitUpdate(ct);
    }

    public UniTask OnExit(CancellationToken ct)
    {
        return owner.OnInitExit(ct);
    }
}

public partial class ColorSlot : ColorSlotInitState.IHandler
{
    private ColorSlotInitState colorSlotInitStateCache { get; set; }
    public ColorSlotInitState ColorSlotInitState => colorSlotInitStateCache ??= new ColorSlotInitState { owner = this };
    
    public UniTask OnInitEnter(CancellationToken ct)
    {
        InitColor();
        return UniTask.CompletedTask;
    }

    public UniTask OnInitUpdate(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }

    public UniTask OnInitExit(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }

    private void InitColor()
    {
        var color = ColorGlobalConfig.Instance.GetColorByIndex(data);
        colorSlotGraphic.SetColor(color);
     
    }
}
