﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Entity")]
public class Entity : MonoBehaviour, INamed {
	private Vector3 GRAPHIC_OFFSET = new Vector3(8f,8f,0); //Why 9? Don't fucking know! Ask unitys fucking unit measurements
	
	public Map map;
	public EntityLayer entlayer;
	public Vector2 loc;
	public string name;
	
	private Vector2 lastLoc = new Vector2(-1,-1);
	public Vector2 LastLoc { get{ return lastLoc; } }
	
	public string Name() { return name; }
	
	private Renderer ren;
	
	// Use this for initialization
	public virtual void Start () {
		ren = this.GetComponent<Renderer>();
		//this.loc = new Vector2((this.transform.localPosition.x - GRAPHIC_OFFSET.x)/18,(this.transform.localPosition.y - GRAPHIC_OFFSET.y)/18);
	}
	
	// Update is called once per frame
	public virtual void Update () {
	}
	
	public void Move(Vector2 dir){
		if(dir == Vector2.zero) return;
		if(CanMove(dir)) {
			entlayer.NotifyMove(this, loc+dir, loc);
			lastLoc = loc;
			loc += dir;
			this.transform.localPosition = (Vector3)this.entlayer.ConvertEntityPosToScenePos(loc) + (Vector3)GRAPHIC_OFFSET;//new Vector3(loc.x * 18 + GRAPHIC_OFFSET.x, loc.y*18 + GRAPHIC_OFFSET.y, 0);
			
			UpdateVisible();
		}
	}
	
	public void UpdateVisible(){
		if(!ren) 
			ren = this.GetComponent<Renderer>();
		ren.enabled = this.map.IsLocVisible(this.loc);
	}
	
	public void Knock(string direction, int amt){
		Vector2 push = Direction.ConvertToVector(direction);
		for(int i = 1; i <= amt; i++){
			Vector2 mul = push * i;
			bool can = CanMove(mul);
			if(!can){
				if(i != 1) { //Must be against wall if it equals 1 and failed already
					this.Move(push*(i-1));//Only some of distance available, go there
				}
				break;//Reached obstacle
			}else if(can && amt == i){//Full reach of knock available
				this.Move(mul);
				break;
			}
		}
	}
	
	public void SetPos(Vector2 pos, bool resetLastLoc=false){
		if(resetLastLoc)lastLoc = pos;
		else lastLoc = pos;
		
		entlayer.NotifyMove(this, pos, new Vector2(-1,-1));
		loc = pos;
		this.transform.localPosition = (Vector3)this.entlayer.ConvertEntityPosToScenePos(loc) + (Vector3)GRAPHIC_OFFSET;//new Vector3(loc.x * 18 + GRAPHIC_OFFSET.x, loc.y*18 + GRAPHIC_OFFSET.y, 0);
	}
	
	public bool CanMove(Vector2 dir){
		Vector2 pos = loc;
		Vector2 newPos = loc;
		newPos += dir;
		if(entlayer.Occupied(newPos)) return false;
		
		Tile to = map.GetTileAt((int)newPos.x, (int)newPos.y);
		Tile fro = map.GetTileAt((int)pos.x, (int)pos.y);
		return to.Solidity <= fro.Solidity;
	}
	
	public void Death(){
		Destroy (this.gameObject);
	}
	
	public void OnDestroy(){
		this.entlayer.RemoveEntity(this);
	}
}
