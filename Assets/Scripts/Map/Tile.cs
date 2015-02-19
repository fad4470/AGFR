﻿using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class Tile {
	
	private SpriteSheet sheet;
	
	public string name;
	private int spriteIndex;
	private Color mainColor;
	private Color swapColor;
	private int solidity;
	
	private Color[] pixelData;
	private Color[] modifiedPixelData;
	
	public string Name { get { return name; } }
	public int Sprite { get { return spriteIndex; } }
	public Color MainColor { get { return mainColor; } }
	public Color SwapColor { get { return swapColor; } }
	public int Solidity { get { return solidity; } }
	public Color[] Pixels { get { return modifiedPixelData; } }
	public Color[] UnmodifiedPixels { get { return pixelData; } }
	
	public Tile(JSONNode data, SpriteSheet sheet){
		this.sheet = sheet;
		name = data["name"];
		JSONNode mcNode = data["main_color"];
		mainColor = new Color(mcNode["r"].AsFloat,mcNode["g"].AsFloat,mcNode["b"].AsFloat,mcNode["a"].AsFloat);
		JSONNode sNode = data["swap_color"];
		swapColor = new Color(sNode["r"].AsFloat,sNode["g"].AsFloat,sNode["b"].AsFloat,sNode["a"].AsFloat);
		solidity = data["solidity"].AsInt;
		spriteIndex = data["sprite"].AsInt;
		
		pixelData = sheet.GetPixelData(spriteIndex);
		modifiedPixelData = sheet.GetPixelData(spriteIndex);
		PerformColorSwap(modifiedPixelData, swapColor, mainColor);
	}
	
	public Tile(string name, int spriteIndex, Color mainColor, Color swapColor, int solidity, SpriteSheet sheet){
		this.name = name;
		this.spriteIndex = spriteIndex;
		this.mainColor = mainColor;
		this.swapColor = swapColor;
		this.solidity = solidity;
		this.sheet = sheet;
		
		this.pixelData = new Color[sheet.tileResolution*sheet.tileResolution];
		this.modifiedPixelData = new Color[sheet.tileResolution*sheet.tileResolution];
		
		PerformColorSwap(modifiedPixelData,mainColor, swapColor);
	}
	
	private void PerformColorSwap(Color[] data, Color key, Color result){
		for(int i = 0; i < data.Length; i++){
			if(data[i] == key)
				data[i] = result;
		}
	}
	
	public void OnEntityEnter(Entity e, TileData data){
	}
	public void OnEntityExit(Entity e, TileData data){
	}
	public void OnEntityAttack(Entity e, TileData data){
		
	}
	public void Update(TileData data){
	
	}
}
