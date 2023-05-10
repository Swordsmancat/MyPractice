//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-05-09 10:10:55.123
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// 玩家技能属性配置表。
    /// </summary>
    public class DRPlayerSkillGain : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取编号（唯一编号，不可重复）。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取伤害加成倍数（只有大于0.1的倍数才会生效）。
        /// </summary>
        public float DamageGain
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取精力值（扣减）。
        /// </summary>
        public float TakeEnergyValue
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取怒气值/士气值（回复最大值100）。
        /// </summary>
        public int MoraleValue
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取躯干值为空时伤害的加成。
        /// </summary>
        public float TrunkNullDamageGain
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取躯干值（扣减）。
        /// </summary>
        public int TakeTrunkValue
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            DamageGain = float.Parse(columnStrings[index++]);
            TakeEnergyValue = float.Parse(columnStrings[index++]);
            MoraleValue = int.Parse(columnStrings[index++]);
            TrunkNullDamageGain = float.Parse(columnStrings[index++]);
            TakeTrunkValue = int.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    DamageGain = binaryReader.ReadSingle();
                    TakeEnergyValue = binaryReader.ReadSingle();
                    MoraleValue = binaryReader.Read7BitEncodedInt32();
                    TrunkNullDamageGain = binaryReader.ReadSingle();
                    TakeTrunkValue = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
