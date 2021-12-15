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

            var zeroPos = (Vector2) zeroPoint.position
                          + new Vector2(
                              -(configField.sizeX * configCell.cellSize) / 2,
                              -(configField.sizeY * configCell.cellSize) / 2);

            var scale = new Vector3(configCell.cellSize, configCell.cellSize, 1);

            var cellPrefab = configCellPrefab.cellPrefab;

            for (var x = 0; x < sizeX; x++)
            for (var y = 0; y < sizeY; y++)
            {
                var pos = zeroPos + new Vector2(
                    configCell.cellSize * x,
                    configCell.cellSize * y);

                var newCell = Instantiate(cellPrefab, pos, Quaternion.identity, fieldParent);
                newCell.transform.localScale = scale;
                cellList.Add(newCell);
                newCell.Init(configCell);
            }

            SetTargetsToField(cellList);

            return cellList;
        }

        private void SetTargetsToField(List<AbstractCell> cellList)
        {
            var idlist = GetCellListWithTarget();

            foreach (var id in idlist)
                if (cellList.Count > id && cellList[id] != null)
                    cellList[id].InitTarget();
        }

        private List<int> GetCellListWithTarget()
        {
            var targetFieldChance = chanceController.GetChanceForField();
            float max = configField.sizeX * configField.sizeY;

            var cellsWithTarget = (int) (max * targetFieldChance);
            var diff = (int) max - cellsWithTarget;

            var allWalues = new List<int>();
            for (var i = 0; i < max; i++) allWalues.Add(i);

            for (var i = 0; i < diff; i++)
                if (allWalues.Count > 0)
                    allWalues.RemoveAt(Random.Range(0, allWalues.Count));

            return allWalues;
        }

        public override bool RestoreSave(SaveParameters save)
        {
            if (save.intList.Count < 3)
                return false;

            var sizeX = save.intList[0];
            var sizeY = save.intList[1];
            var count = save.intList[2];

            if (save.intList.Count != 3 + count * 4)
                return false;

            var cellList = SpawnField(sizeX, sizeY);

            if (count != cellList.Count)
                return false;

            var shift1 = 103;
            var shift2 = 203;
            var shift3 = 303;

            for (var i = 0; i < count; i++)
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
            var count = configField.sizeX * configField.sizeY;

            if (cellList == null || count != cellList.Count)
                return null;

            var allData = new List<int>();

            allData.Add(configField.sizeX);
            allData.Add(configField.sizeY);
            allData.Add(count);

            for (var i = 0; i < count; i++) allData.Add(cellList[i].GetCurrentState());

            for (var i = 0; i < count; i++) allData.Add(cellList[i].GetTargetLayer());

            for (var i = 0; i < count; i++) allData.Add(cellList[i].GetTargetCollected());

            for (var i = 0; i < count; i++) allData.Add(cellList[i].GetCellFinished());

            var save = new SaveParameters();
            save.name = "field";
            save.intList = allData;
            return save;
        }
    }
}