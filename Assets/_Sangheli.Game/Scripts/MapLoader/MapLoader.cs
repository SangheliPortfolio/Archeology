using Sangheli.Config;
using System.Collections;
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
		}
	}
}