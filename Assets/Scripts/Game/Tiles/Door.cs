﻿using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Door : Tile {
	
	public Door(){
	}
	
	public Door(JSONNode data, SpriteSheet sheet):base(data,sheet){
		
	}
	
	public override void OnEntityEnter(Entity e, TileData data){
		if(e.gameObject.tag == "Player" && data["level"] != null){
			Game.LevelSpawn = new Vector2((int)data["spawnx"],(int)data["spawny"]);
			Game.LoadLevel((string)data["level"]);
		}
	}
	
	public override void ReadData(JSONNode node, TileData data){
		if(node["level"] != null){
			data["level"] = node["level"].Value;
			
			data["spawnx"] = node["spawnx"].AsInt;
			data["spawny"] = node["spawny"].AsInt;
		}
	}
	
	public override TileData GetDefaultTileData(int x, int y){
		TileData data = new TileData(x,y);
		data["level"] = "map1";
		data["spawnx"] = 0;
		data["spawny"] = 0;
		return data;
	}
}
