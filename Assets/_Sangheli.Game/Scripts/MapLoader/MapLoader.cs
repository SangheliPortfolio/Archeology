using System.Collections.Generic;
using Sangheli.Config;
using Sangheli.Save;
using UnityEngine;

namespace Sangheli.Game
{
    public class MapLoader : AbstractMapLoader
    {
        [Header("Configs")] [SerializeField] private ConfigField configField;

        [SerializeField] private ConfigCellPrefab configCellPrefab;

        [SerializeField] private ConfigCell configCell;

        [SerializeField] private ChanceController chanceController;

        [Space] [Header("Field")] [SerializeField]
        private Transform zeroPoint;

        [SerializeField] private Transform fieldParent;

        private List<AbstractCell> cellList;

        private bool levelLoaded;

        public override void SpawnField()
        {
            SpawnField(default, default);
        }

        public override List<AbstractCell> SpawnField(int sizeX = -1, int sizeY = -1)
        {
            if (levelLoaded)
                return null;

            levelLoaded = true;

            cellList = new List<AbstractCell>();
            zeroPoint.gameObject.SetActive(false);

            sizeX = sizeX > 0 ? sizeX : configField.sizeX;
            sizeY = sizeY > 0 ? sizeY : configField.sizeY;

            Vector2 zeroPos = (Vector2) zeroPoint.position
                              + new Vector2(
                                  -(configField.sizeX * configCell.cellSize) / 2,
                                  -(configField.sizeY * configCell.cellSize) / 2);

            Vector3 scale = new Vector3(configCell.cellSize, configCell.cellSize, 1);

            AbstractCell cellPrefab = configCellPrefab.cellPrefab;

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    Vector2 pos = zeroPos + new Vector2(
                        configCell.cellSize * x,
                        configCell.cellSize * y);

                    AbstractCell newCell = Instantiate(cellPrefab, pos, Quaternion.identity, fieldParent);
                    newCell.transform.localScale = scale;
                    cellList.Add(newCell);
                    newCell.Init(configCell);
                }
            }

            SetTargetsToField(cellList);

            return cellList;
        }

        private void SetTargetsToField(List<AbstractCell> cellList)
        {
            List<int> idlist = GetCellListWithTarget();

            foreach (int id in idlist)
            {
                if (cellList.Count > id && cellList[id] != null)
                {
                    cellList[id].InitTarget();
                }
            }
        }

        private List<int> GetCellListWithTarget()
        {
            float targetFieldChance = chanceController.GetChanceForField();
            float max = configField.sizeX * configField.sizeY;

            int cellsWithTarget = (int) (max * targetFieldChance);
            int diff = (int) max - cellsWithTarget;

            List<int> allWalues = new List<int>();
            for (int i = 0; i < max; i++)
            {
                allWalues.Add(i);
            }

            for (int i = 0; i < diff; i++)
            {
                if (allWalues.Count > 0)
                    allWalues.RemoveAt(Random.Range(0, allWalues.Count));
            }

            return allWalues;
        }

        public override bool RestoreSave(SaveParameters save)
        {
            if (save.intList.Count < 3)
                return false;

            int sizeX = save.intList[0];
            int sizeY = save.intList[1];
            int count = save.intList[2];

            if (save.intList.Count != 3 + count * 4)
                return false;

            List<AbstractCell> cellList = SpawnField(sizeX, sizeY);

            if (count != cellList.Count)
                return false;

            int shift1 = 103;
            int shift2 = 203;
            int shift3 = 303;

            for (int i = 0; i < count; i++)
            {
                cellList[i].SetCurrentState(save.intList[i + 3]);
                cellList[i].SetTargetLayer(save.intList[i + shift1]);
                cellList[i].SetTargetCollected(save.intList[i + shift2]);
                cellList[i].SetCellFinished(save.intList[i + shift3]);
                cellList[i].InitCellSaveData();
            }

            return true;
        }

        public override SaveParameters GetSave()
        {
            int count = configField.sizeX * configField.sizeY;

            if (cellList == null || count != cellList.Count)
                return null;

            List<int> allData = new List<int>();

            allData.Add(configField.sizeX);
            allData.Add(configField.sizeY);
            allData.Add(count);

            for (int i = 0; i < count; i++)
            {
                allData.Add(cellList[i].GetCurrentState());
            }

            for (int i = 0; i < count; i++)
            {
                allData.Add(cellList[i].GetTargetLayer());
            }

            for (int i = 0; i < count; i++)
            {
                allData.Add(cellList[i].GetTargetCollected());
            }

            for (int i = 0; i < count; i++)
            {
                allData.Add(cellList[i].GetCellFinished());
            }

            SaveParameters save = new SaveParameters();
            save.name = "field";
            save.intList = allData;
            return save;
        }
    }
}