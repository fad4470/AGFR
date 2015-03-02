﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Map/Map Render")]
public class MapRender : MonoBehaviour {
	
	public Color backgroundColor = new Color(32f/255f,32f/255f,32f/255f,1f);
	public Map map;
	
	private Texture2D tex;
	private int tileRes;
	// Use this for initialization
	void Start () {
		tileRes = Game.GameObject.GetComponent<SpriteSheet>().tileResolution;
		int tileSize = tileRes+2;
		tex = new Texture2D(tileSize*(int)Map.MAPDIM.x, tileSize*(int)Map.MAPDIM.y);
		this.renderer.material.mainTexture = tex;
		tex.filterMode = FilterMode.Point;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(map && map.IsDirty){
			RepaintMap();
			map.MarkClean();
		}
	}
	
	public void RepaintMap(){
		int tileSize = map.sheet.tileResolution+2;
		
		for(int x = 0; x< Map.MAPDIM.x*tileSize; x++){ //Clear
			for(int y = 0; y< Map.MAPDIM.y*tileSize; y++){
				tex.SetPixel(x,y,this.backgroundColor);
			}
		}
 		for(int x = 0; x< Map.MAPDIM.x; x++){
			for(int y = 0; y< Map.MAPDIM.y; y++){
				Tile t = map.GetVisibleTileAt(x,y);
				if(t != Map.errTile && t != Map.emptyTile){
					Color[] pixels = map.GetPixelsAt(x,y);
					tex.SetPixels(x*tileSize+1,y*tileSize+1,tileRes, tileRes, pixels);
					TileData td = map.GetTileDataVisibleAt(x,y);
					object get = td["highlight"];
					if(get == null) continue;
					Color highlight = (Color)get;
					if(highlight != Color.clear){
						int max = map.sheet.tileResolution+2;
						for(int hx = -1; hx <= max; hx++){
							for(int hy = -1; hy <= max; hy++){
								if(hx == -1 || hy == -1 || hx == max || hy == max) //Only edges
									tex.SetPixel(x*tileSize+hx,y*tileSize+hy,highlight);
							}
						}
					}
				}
				
				if(t == Map.emptyTile && Game.Mode == Game.GameMode.LEVEL_EDITOR && map.IsScenePosWithinMap(new Vector2(x,y))){ //Making empty squares for drawing
					int max = map.sheet.tileResolution+2;
					for(int hx = -1; hx <= max; hx++){
						for(int hy = -1; hy <= max; hy++){
							tex.SetPixel(x*tileSize+hx,y*tileSize+hy,Color.clear);
						}
					}
				}
			}
		}
		
		tex.Apply ();
	}
}
