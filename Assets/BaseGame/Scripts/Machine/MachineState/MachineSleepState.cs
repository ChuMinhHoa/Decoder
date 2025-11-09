using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MachineSleepState : IState
{
    public interface IHandler
    {
        public UniTask OnSleepEnter(CancellationToken ct);
        public UniTask OnSleepUpdate(CancellationToken ct);
        public UniTask OnSleepExit(CancellationToken ct);
    }
    
    public IHandler onwer { get; set; }
    
    public UniTask OnEnter(CancellationToken ct)
    {
        return onwer.OnSleepEnter(ct);
    }

    public UniTask OnUpdate(CancellationToken ct)
    {
        return onwer.OnSleepUpdate(ct);
    }

    public UniTask OnExit(CancellationToken ct)
    {
        return onwer.OnSleepExit(ct);
    }
}

public partial class Machine : MachineSleepState.IHandler
{
    private MachineSleepState machineSleepState;
    public MachineSleepState MachineSleepState => machineSleepState ??= new MachineSleepState { onwer = this };
    public UniTask OnSleepEnter(CancellationToken ct)
    {
        ChangeEnableButtonPlay(true);
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
