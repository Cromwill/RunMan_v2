using System.Collections;

public interface IEnemySpawnGenerator
{
    TileGeneration[] GetTilesToSpawn(TileGeneration[] tiles, TileGeneration currentTile);
    TileGeneration GetTileForEnemy();

}
