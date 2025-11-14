using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class LevelGenerate : MonoBehaviour
{
   public LevelConvert levelConvert;
   [ReadOnly, ShowInInspector]
   public LevelProposal levelProposal => LevelDesign.Instance.levelDesignFromProposal.GetLevelProposal(level);
   
   public int level;

   
   [Button(ButtonSizes.Gigantic), GUIColor(1f, .2f, 0f)]
   private void CreateLevelAndSave()
   {
      var dir = $"Assets/Resources/Level/";
      var jsonObj = JsonUtility.ToJson(levelConvert, true);
      var json = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonObj));
      var path = Path.Combine(dir, $"Level{level}.txt");
      File.WriteAllText(path, json);
      AssetDatabase.Refresh();
   }

   private LevelConvert GetLevelConvert()
   {
      var data = AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/Resources/Level/Level{level}.txt");
      if (data == null)
         return null;
      var levelGet = MyCache.DecodeBase64Json<LevelConvert>(data.text);
      return levelGet;
   }

   [Button(ButtonSizes.Gigantic)]
   private void LoadLevel()
   {
      var levelGet = GetLevelConvert();
      if (levelGet != null)
      {
         levelConvert = levelGet;
      }
      else
      {
         levelConvert = CreateNewLevelConvert();
      }

      if (levelConvert.colorLineMachines == null)
         CreateNewLineMachines();
   }

   private void CreateNewLineMachines()
   {
      levelConvert.colorLineMachines = new ColorLineMachine[7];
      for (int i = 0; i < 7; i++)
      {
         levelConvert.colorLineMachines[i] = new ColorLineMachine();
         levelConvert.colorLineMachines[i].colorSlots = new ColorSlotMachine[4];
      }
   }

   [Button]
   private LevelConvert CreateNewLevelConvert()
   {
      var newLevelConvert = new LevelConvert();
      newLevelConvert.level = level;
      newLevelConvert.colorShowFirstIndex = new[] { 0, 0, 0, 0 };
      newLevelConvert.colorSecretIndex = new[] { 0, 0, 0, 0 };
      newLevelConvert.colorInLevelIndex = new[] { 0, 0, 0, 0 };
      newLevelConvert.colorLineMachines = new ColorLineMachine[7];
      for (int i = 0; i < 7; i++)
      {
         newLevelConvert.colorLineMachines[i] = new ColorLineMachine();
         newLevelConvert.colorLineMachines[i].colorSlots = new ColorSlotMachine[4];
      }
      return newLevelConvert;
   }
}
