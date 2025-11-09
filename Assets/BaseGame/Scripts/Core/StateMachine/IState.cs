using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IState
{
    UniTask OnEnter(CancellationToken ct);
    UniTask OnUpdate(CancellationToken ct);
    UniTask OnExit(CancellationToken ct);
}
