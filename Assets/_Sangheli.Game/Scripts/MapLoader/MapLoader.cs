using Sangheli.Config;
using Sangheli.Save;
using System.Collections.Generic;
using UnityEngine;

namespace Sangheli.Game
{
	public class MapLoader : AbstractMapLoader
	{
		[Header("Configs")]
		[SerializeField]
		private ConfigField configField;

		[SerializeField]
		private ConfigCellPrefab configCellPrefab;

		[SerializeField]
		private ConfigCell configCell;

		[SerializeField]
		private ChanceController chanceController;

		[Space]
		[Header("Field")]
		[SerializeField]
		private Transform zeroPoint;

		[SerializeField]
		private Transform fieldParent;

		private List<AbstractCell> cellList;

		private bool levelLoaded = false;

		public override void SpawnField()
		{
			this.SpawnField(default,default);
		}

		public override List<AbstractCell> SpawnField(int _sizeX = -1,int _sizeY = -1)
		{
			if (this.levelLoaded)
				return null;

			this.levelLoaded = true;

			this.cellList = new List<AbstractCell>();
			this.zeroPoint.gameObject.SetActive(false);

			int sizeX = _sizeX > 0 ? _sizeX : this.configField.sizeX;
			int sizeY = _sizeY > 0 ? _sizeY : this.configField.sizeY;

			Vector2 zeroPos = (Vector2)this.zeroPoint.position 
				+ new Vector2(
					- (this.configField.sizeX* this.configCell.cellSize) / 2,
				- (this.configField.sizeY* this.configCell.cellSize) / 2);

			Vector3 scale = new Vector3(this.configCell.cellSize, this.configCell.cellSize, 1);

			AbstractCell cellPrefab = this.configCellPrefab.cellPrefab;

			for (int x = 0; x < sizeX; x++)
			{
				for (int y = 0; y < sizeY; y++)
				{
					Vector2 pos = zeroPos + new Vector2(
						this.configCell.cellSize * x,
						this.configCell.cellSize * y);

					AbstractCell newCell = Instantiate(cellPrefab, pos,Quaternion.identity, this.fieldParent);
					newCell.transform.localScale = scale;
					this.cellList.Add(newCell);
					newCell.Init(this.configCell);
				}
			}

			this.SetTargetsToField(this.cellList);

			return this.cellList;
		}

		private void SetTargetsToField(List<AbstractCell> cellList)
		{
			List<int> idlist = this.GetCellListWithTarget();

			foreach(int id in idlist)
			{
				if(cellList.Count>id && cellList[id] != null)
				{
					cellList[id].InitTarget();
				}
			}
		}

		private List<int> GetCellListWithTarget()
		{
			float targetFieldChance = this.chanceController.GetChanceForField();
			float max = this.configField.sizeX * this.configField.sizeY;

			int cellsWithTarget = (int)(max * targetFieldChance);
			int diff = (int)max - cellsWithTarget;

			List<int> allWalues = new List<int>();
			for(int i = 0; i < max; i++)
			{
				allWalues.Add(i);
			}

			for(int i = 0; i < diff; i++)
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

			List<AbstractCell> cellList = this.SpawnField(sizeX, sizeY);

			if (count != cellList.Count)
				return false;

			int shift1 = 103;
			int shift2 = 203;
			int shift3 = 303;

			for(int i = 0; i < count; i++)
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
			int count = this.configField.sizeX * this.configField.sizeY;

			if (this.cellList == null || count != this.cellList.Count)
				return null;

			List<int> allData = new List<int>();

			allData.Add(this.configField.sizeX);
			allData.Add(this.configField.sizeY);
			allData.Add(count);

			for (int i = 0; i < count; i++)
			{
				allData.Add(this.cellList[i].GetCurrentState());
			}

			for (int i = 0; i < count; i++)
			{
				allData.Add(this.cellList[i].GetTargetLayer());
			}

			for (int i = 0; i < count; i++)
			{
				allData.Add(this.cellList[i].GetTargetCollected());
			}

			for (int i = 0; i < count; i++)
			{
				allData.Add(this.cellList[i].GetCellFinished());
			}

			SaveParameters save = new SaveParameters();
			save.name = "field";
			save.intList = allData;
			return save;
		}
	}
}