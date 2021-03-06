using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Items/Spawn Sprite On Swing")]
public class SpawnSpriteOnSwing : MonoBehaviour {

	public PremadeContainer sprites;
	public string spriteName;
	public int damage = 1;

	private EntityLayer spriteLayer;
	private Entity player;
	
	private string lastDirection = "";
	void Start(){
		spriteLayer = GameObject.FindGameObjectWithTag("SpriteLayer").GetComponent<EntityLayer>();
		player = Game.Player.GetComponent<Entity>();
	}

	void OnSwing(string direction){
		Vector2 dir = Direction.ConvertToVector(direction);
		Entity e = spriteLayer.SpawnEntity(sprites[spriteName], player.loc + dir).GetComponent<Entity>();
		//Sending because sprites can't save last direction (they werent born yet!)
		e.SendMessage("SetLastDirection", lastDirection, SendMessageOptions.DontRequireReceiver); 
		e.SendMessage("SetDirection", direction);
		e.SendMessage("SetOwner", player, SendMessageOptions.DontRequireReceiver);
		e.SendMessage("SetDamage", damage, SendMessageOptions.DontRequireReceiver);
		lastDirection = direction;
	}
	
	void OnLevelLoaded(){
		player = Game.Player.GetComponent<Entity>();
	}
}
