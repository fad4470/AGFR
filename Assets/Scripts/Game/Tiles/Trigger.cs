﻿using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Trigger : Tile {
	
	public Trigger(){
	}
	
	public Trigger(JSONNode data, SpriteSheet sheet):base(data,sheet){
	}
	
	public override void OnEntityEnter(Entity e, TileData data){
		if(data["action"] != null) ((IButtonAction)data["action"]).OnEntityEnter(e);
	}
	public override void OnEntityExit(Entity e, TileData data){
		if(data["action"] != null) ((IButtonAction)data["action"]).OnEntityExit(e);
	}
	public override void Update(TileData data){
		if(data["action"] != null) ((IButtonAction)data["action"]).Update();
	}
	
	public override void ReadData(JSONNode node, TileData data){
		if(node["action"] != null){
			string actionName = node["action"].Value;
			switch(actionName){
			case "toggle lights":
				data["action"] = new ToggleLightsButton(node,data);
				break;
			case "set camera":
				data["action"] = new SetCameraButton(node,data);
				break;
			}
		}
	}
	
	public override TileData GetDefaultTileData(int x, int y){
		TileData data = new TileData(x,y);
		data["action"] = "toggle lights";
		return data;
	}
}
