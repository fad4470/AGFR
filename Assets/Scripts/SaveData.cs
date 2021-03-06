﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveData {

	private Dictionary<string,object> data = new Dictionary<string, object>();

	public object this[string key] {
		get { 
			return data[key];
		}
		set {
			data[key] = value;
		}
	}
	
	public bool HasKey(string key){
		return data.ContainsKey(key);
	}
	//TODO implement save and load
}
