using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class MachineInitState : IState
{
   public interface IHandler
   {
       public UniTask OnInitMachineEnter(CancellationToken ct);
       public UniTask OnInitMachineUpdate(CancellationToken ct);
       public UniTask OnInitMachineExit(CancellationToken ct);
   }

   public IHandler onwer { get; set; }

   public UniTask OnEnter(CancellationToken ct)
   {
       return onwer.OnInitMachineEnter(ct);
   }

   public UniTask OnUpdate(CancellationToken ct)
   {
       return onwer.OnInitMachineUpdate(ct);
   }

   public UniTask OnExit(CancellationToken ct)
   {
       return onwer.OnInitMachineExit(ct);
   }
}
public partial class Machine : MachineInitState.IHandler
{
    private MachineInitState machineInitStateCache { get; set; }
    public MachineInitState MachineInitState => machineInitStateCache ??= new MachineInitState { onwer = this };
    public async UniTask OnInitMachineEnter(CancellationToken ct)
    {
        await machineAnim.LightStart();
        machineAnim.PlayLightAnim(AnimLightMachine.Idle);
        await LoadData();
        ChangeEnableButtonGamePlay(true);
        ChangeEnableButtonPlay(false);
    }

    public UniTask OnInitMachineUpdate(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }

    public UniTask OnInitMachineExit(CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }
    
    [Button]
    private async UniTask LoadData()
    {
        ResetMachine();
        levelConfig = LevelGlobalConfig.Instance.GetLevelData(currentLevel);
        var data = levelConfig.levelData.text;
        levelConvert = MyCache.DecodeBase64Json<LevelConvert>(data);
        GameManager.Instance.CreateNewColor(levelConvert);
        
        await colorExpertLine.InitData(levelConvert.colorInLevelIndex);
        await colorLines[0].InitData(levelConvert.colorShowFirstIndex);
        await colorLines[0].CheckHintColor();
        
        maxColor = levelConvert.colorInLevelIndex.Length;
        colorLines[1].ChangeColorSlot(maxColor, 1);
        colorLines[1].SelectFirstSlot();
    }
}