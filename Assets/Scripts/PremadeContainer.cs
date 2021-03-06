﻿using UnityEngine;
using System.Collections;

public class PremadeContainer : ScriptableObject , INamed {

	public string name;
	public GameObject[] prefabs;
	
	public string Name(){ return this.name; }
	
	public GameObject this[string key] {
		get {
			foreach(GameObject go in prefabs){
				if(((INamed)go.GetComponent(typeof(INamed))).Name() == key) return go;
			}
			return null;
		}
	}
}
