using UnityEngine;
using System.Collections;
using SimpleJSON;

[AddComponentMenu("Scripts/Map/Map Loader")]
public class MapLoader : MonoBehaviour {
	public Map map;
	public EntityLayer entities;
	public EntityLayer sprites;
	
	public PremadeContainer entityList;
	
	private JSONNode data;
	private Vector2 dimensions;
	private Vector2 spawn;
	
	//load for game
	public void Load(TextAsset mapFile){
		if(entities)entities.Clear();
		if(sprites)sprites.Clear();
		data = JSON.Parse(mapFile.text);
		ParseData ();
		map.MarkDirty();
		
		if(Game.LevelSpawn.x != -1) spawn = Game.LevelSpawn;
		map.CenterCameraOn(spawn);
		Game.Player = entities.SpawnEntity(entityList["player"],spawn);
	}
	
	//load for editor
	public void Load(MapData mapData, TextAsset mapFile){
		data = JSON.Parse(mapFile.text);
		this.map = mapData.map;
		ParseData ();
		mapData.map.MarkDirty();
		mapData.spawnX = (int)this.spawn.x;
		mapData.spawnY = (int)this.spawn.y;
		
		foreach(Tile t in mapData.map.Tiles.Values){
			mapData.userTiles.Add(new EditorItem(t));
		}
	}
	
	private void ParseData(){
		dimensions = new Vector2(data["info"]["width"].AsInt, data["info"]["height"].AsInt);
		spawn = new Vector2(data["info"]["spawn"][0].AsInt, data["info"]["spawn"][1].AsInt);
		map.Init(dimensions);
		PopulateTiles(data["tiles"]);
		PopulateMap(data["map"]);
		if(data["entities"] != null 
			&& Game.Mode == Game.GameMode.GAME)  //Temporary until editor entites
			PopulateEntities(data["entities"]);
	}
	
	private void PopulateMap(JSONNode mapData){
		for(int x = 0; x < dimensions.x; x++){
			for(int y = 0; y < dimensions.y; y++){
				JSONNode tile = mapData[(int)dimensions.y-y-1][x];
				if(tile.GetType() == typeof(SimpleJSON.JSONClass)){
					Tile t = map.GetTile(tile["style"].Value.Trim());
					TileData data = map.GetTileDataAt(x,y);
					t.ReadData(tile, data);
					map.SetTileAndTileDataAt(x,y, tile["style"].Value.Trim(),data);
				}
				else {
					map.SetTileAt(x,y, tile);
				}
			}
		}
	}
	
	private void PopulateTiles(JSONNode tileJData){
		int count = tileJData.Count;
		for (int i = 0; i < count; i++){
			JSONNode t = tileJData[i];
			string name = t["name"].Value.Trim();
			string type = t["type"].Value.Trim();
			if(type == null) type = "tile";
			map.SetTile(name, MakeTileInstance(type, t, map.sheet));
		}
	}
	
	private void PopulateEntities(JSONNode tileJData){
		int count = tileJData.Count;
		for (int i = 0; i < count; i++){
			JSONNode t = tileJData[i];
			int x = t["x"].AsInt;
			int y = t["y"].AsInt;
			string name = t["name"].Value;
			entities.SpawnEntity(entityList[name],new Vector2(x,y));
		}
	}
	
	public static Color ReadColor(JSONNode colorNode){
		float r = colorNode["r"].AsFloat;
		float g = colorNode["g"].AsFloat;
		float b = colorNode["b"].AsFloat;
		float a = colorNode["a"].AsFloat;
		if(r > 1 || g > 1 || b > 1){
			r /= 255f;
			g /= 255f;
			b /= 255f;
		}
		if(a > 1){ //Alpha seperate because I forget to change that sometimes
			a /= 255f;
		}
		return new Color(r,g,b,a);
		
	}
	
	//Converts tile-type to a tile class for the map
	public static Tile MakeTileInstance(string type, JSONNode file=null, SpriteSheet sheet=null){
		Tile rtn = null;
		switch(type){
		default: rtn = new Tile(); break;
		case "button": rtn = new Trigger(); break;
		case "sign": rtn = new Sign(); break;
		case "door": rtn = new Door(); break;
		case "light": rtn = new LitTile(); break;
		}
		
		if(file != null)
			rtn.Init(file,sheet);
		
		return rtn;
	}
}
