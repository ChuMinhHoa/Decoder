using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class MachineLoseState : IState
{
    public interface IHandler
    {
        public UniTask OnEnter(CancellationToken ct);
        public UniTask OnUpdate(CancellationToken ct);
        public UniTask OnExit(CancellationToken ct);
    }

    public IHandler owner;
    
    public UniTask OnEnter(CancellationToken ct)
    {
        return owner.OnEnter(ct);
    }

    public UniTask OnUpdate(CancellationToken ct)
    {
        return owner.OnUpdate(ct);
    }

    public UniTask OnExit(CancellationToken ct)
    {
        return owner.OnExit(ct);
    }
}

public partial class Machine : MachineLoseState.IHandler
{
    private MachineLoseState machineLoseStateCache { get; set; }
    public MachineLoseState MachineLoseState => machineLoseStateCache ??= new MachineLoseState { owner = this };
    public async UniTask OnEnter(CancellationToken ct)
    {
        SoundManager.Instance.PlaySfx(SoundType.Lose);
        machineAnim.PlayLightAnim(AnimLightMachine.Lose); 
        for (var i = 0; i < 2; i++)
        {
            _ = AnimLose();
            await UniTask.Delay(1000, cancellationToken: ct);
        }
        await AnimLose();   
        ResetMachine();
        await UniTask.Delay(1000, cancellationToken: ct);
        await machineAnim.LightEnd();
        SoundManager.Instance.PlaySfx(SoundType.PlayGear);
        _= machineAnim.AnimLightQuit();
        await stateMachine.ChangeState(MachineSleepState);
    }
    
    [Button]
    private async UniTask AnimLose()
    {
        for (var i = 0; i < colorLines.Length; i++)
        {
            _= colorLines[i].AnimLose();
            await UniTask.Delay(100);
        }
    }

    public UniTask OnUpdate(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }

    public UniTask OnExit(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }
}
