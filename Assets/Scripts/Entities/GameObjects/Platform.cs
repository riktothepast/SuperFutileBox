using UnityEngine;
using System.Collections;

public class Platform : Entity {
	public FPNodeLink bodyLink;
	public BoxCollider bodyCollider;
	
	public static Platform Create(InGamePage world, Vector2 pos, float angulo, Vector2 size, FSprite sprite, string name){
		GameObject GObj = new GameObject(name);
		Platform body = GObj.AddComponent<Platform>();
		body.GamePage = world;
		body.Position=pos;
		body.Size=size;
		body.Sprite = sprite;
		Color cl =new Color(255,255,255,0.3f);
		body.Sprite.color=cl;
		body.Sprite.width = size.x;
		body.Sprite.height = size.y;
		body.Rotation=angulo;
		body.transform.Rotate(new Vector3(0,0,angulo));
		return body;
	}
	
	public override void Init ()
	{
		GamePage._gameObjects.AddChild(Holder = new FContainer());
		gameObject.transform.position = new Vector3(Position.x * FPhysics.POINTS_TO_METERS,Position.y * FPhysics.POINTS_TO_METERS,64);
		gameObject.transform.parent = GamePage.root.transform;
		bodyLink = gameObject.AddComponent<FPNodeLink>();
		bodyLink.Init(Holder, true);
		Holder.AddChild(Sprite);
		AngularDrag=5f;
		Mass=1f;
		Bounciness=0.5f;
		FrictionDyn=0.3f;
		FrictionSta=0.2f;
		InitPhysics();
	}
	
	public override void InitPhysics()
	{
		bodyCollider = gameObject.AddComponent<BoxCollider>();
		bodyCollider.size=new Vector3(Size.x * FPhysics.POINTS_TO_METERS,Size.y * FPhysics.POINTS_TO_METERS,64);
		PhysicMaterial mat = new PhysicMaterial();
		mat.bounciness = Bounciness;
		mat.dynamicFriction = FrictionDyn;
		mat.staticFriction = FrictionSta;
		mat.frictionCombine = PhysicMaterialCombine.Maximum;
		
	}
	
	public override void Update(){
		
	}
	
}

	
