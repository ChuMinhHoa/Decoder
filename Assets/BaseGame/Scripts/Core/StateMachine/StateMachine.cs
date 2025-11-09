using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
[System.Serializable]
public class StateMachine
{
    public IState CurrentState;
    private Queue<IState> StateQueue { get; set; } = new ();
    private CancellationTokenSource CancellationTokenSource { get; set; }
    private bool IsRunning { get; set; } = false;

    public void Run()
    {
        CancellationTokenSource = new CancellationTokenSource();
        IsRunning = true;
        ExecuteState().Forget();
    }
    
    public void RegisterState(IState state)
    {
        StateQueue.Enqueue(state);
    }

    public async UniTask ChangeState(IState state)
    {
        if (CurrentState != null) await CurrentState.OnExit(CancellationTokenSource.Token);
        CurrentState = state;
        if (CurrentState != null) await CurrentState.OnEnter(CancellationTokenSource.Token);
    }
    
    public void Stop()
    {
        if (!IsRunning) return;
        IsRunning = false;
        CancellationTokenSource.Cancel();
        CancellationTokenSource.Dispose();
    }
    
    public async UniTask ExecuteState()
    {
        UniTaskCancelableAsyncEnumerable<AsyncUnit> asyncEnumerable = UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(CancellationTokenSource.Token);   
        await foreach (AsyncUnit _ in asyncEnumerable)
        {
            while (StateQueue.Count > 0)
            {
                await ChangeState(StateQueue.Dequeue());
            }

            if (CurrentState != null) await CurrentState.OnUpdate(CancellationTokenSource.Token);
        }
    }
}
