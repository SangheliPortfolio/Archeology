using Sangheli.Config;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

		public override void SpawnField()
		{
			this.cellList = new List<AbstractCell>();
			this.zeroPoint.gameObject.SetActive(false);

			int sizeX = this.configField.sizeX;
			int sizeY = this.configField.sizeY;

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
		}

		private void SetTargetsToField(List<AbstractCell> cellList)
		{
			List<int> idlist = this.GetCellListWithTarget();

			foreach(int id in idlist)
			{
				if(cellList.Count>id && cellList[id] != null)
				{
					cellList[id].SetTarget();
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
	}
}