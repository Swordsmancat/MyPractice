//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEditor;
using UnityEngine;

namespace Farm.Editor.DataTableTools
{
    public sealed class DataTableGeneratorMenu
    {
        [MenuItem("Farm/Generate DataTables")]
        private static void GenerateDataTables()
        {
          string[] DataTableNames = new string[]
        {
            "Character",
            "Armor",
            "Enemy",
            "Entity",
            "Music",
            "Scene",
            "Sound",
            "Thruster",
            "UIForm",
            "UISound",
            "Weapon",
            "NPC",
            "Skill",
            "Item",
            "PlayerSkillGain",
        };
           // foreach (string dataTableName in ProcedurePreload.DataTableNames)
            foreach (string dataTableName in DataTableNames)
            {
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }

                DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
                DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }

            AssetDatabase.Refresh();
        }
    }
}
