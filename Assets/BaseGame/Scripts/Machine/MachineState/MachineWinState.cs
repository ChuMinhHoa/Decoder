using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class MachineWinState : IState
{
    public interface IHandler
    {
        public UniTask OnWinEnter(CancellationToken ct);
        public UniTask OnWinUpdate(CancellationToken ct);
        public UniTask OnWinExit(CancellationToken ct);
    }
    
    public IHandler onwer { get; set; }

    public UniTask OnEnter(CancellationToken ct)
    {
        return onwer.OnWinEnter(ct);
    }

    public UniTask OnUpdate(CancellationToken ct)
    {
        return onwer.OnWinUpdate(ct);
    }

    public UniTask OnExit(CancellationToken ct)
    {
        return onwer.OnWinExit(ct);
    }
}

public partial class Machine : MachineWinState.IHandler
{
    private MachineWinState machineWinStateCache { get; set; }
    public MachineWinState MachineWinState => machineWinStateCache ??= new MachineWinState { onwer = this };
    
    public async UniTask OnWinEnter(CancellationToken ct)
    {
        SoundManager.Instance.PlaySfx(SoundType.Win);
        machineAnim.PlayLightAnim(AnimLightMachine.Win); 
        for (var i = 0; i < 2; i++)
        {
            _ = AnimWin();
            await UniTask.Delay(1000, cancellationToken: ct);
        }
        await AnimWin();   
        ResetMachine();
        await UniTask.Delay(1000, cancellationToken: ct);
        await machineAnim.LightEnd();
        SoundManager.Instance.PlaySfx(SoundType.PlayGear);
        _= machineAnim.AnimLightQuit();
        await stateMachine.ChangeState(MachineSleepState);
    }

    [Button]
    private async UniTask AnimWin()
    {
        for (var i = 0; i < colorLines.Length; i++)
        {
            _= colorLines[i].AnimWin();
            await UniTask.Delay(100);
        }
    }

    public UniTask OnWinUpdate(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }

    public UniTask OnWinExit(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }
}
