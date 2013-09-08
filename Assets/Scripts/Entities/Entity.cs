using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour {
	public abstract void Init();
	public abstract void InitPhysics();
	public abstract void Update ();
	
	public Vector2 Size{ get; set; }
	public Vector2 Position{ get; set; }
	public Vector2 Velocity{ get; set; }
	public Vector2 Aceleration{ get; set; }
	public Vector2 Impact{ get; set; }
	public float Rotation{ get; set;}
	public float AngularDrag{ get; set;}
	public float Mass{ get; set;}
	public float Bounciness{ get; set;}
	public float FrictionDyn{ get; set;}
	public float FrictionSta{ get;set;}
	public InGamePage GamePage{ get; set;}
	public FSprite Sprite{ get;set;}
	public FContainer Holder{ get;set;}
}