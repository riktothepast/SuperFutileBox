using UnityEngine;
using System.Collections;

public class Projectile : Entity {
	
	public FPNodeLink bodyLink;
	public BoxCollider bodyCollider;
	Vector2 Acceleration;
	
	public static Projectile Create (InGamePage world, Vector2 pos, Vector2 Aceleration, FSprite sprite){
		GameObject GObj = new GameObject("Bullet");
		Projectile body = GObj.AddComponent<Projectile>();
		body.GamePage = world;
		body.Position=pos;
		body.Aceleration=Aceleration;
		body.Size=new Vector2(10,10);
		body.Sprite = new FSprite("Futile_White");
		Color cl =new Color(60,60,60,1);
		body.Sprite.color=cl;
		body.Sprite.width = body.Size.x;
		body.Sprite.height = body.Size.y;
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
	
		void OnCollisionEnter(Collision collision) {
			if(collision.collider.name.Equals("Platform")||collision.collider.name.Equals("Left")||collision.collider.name.Equals("Right")){
				Debug.Log("plataform");
				Destroy();
			}
			if(collision.collider.name.Equals("Enemy")){
				collision.gameObject.SendMessage("RestLife",1);
				GamePage.setScore(10);
				Destroy();
			}
    }
	
	public override void Update()
	{
		gameObject.transform.position = new Vector3(gameObject.transform.position.x+(Aceleration.x*0.5f), gameObject.transform.position.y, gameObject.transform.position.z);		
	}
	
	void Destroy(){
		UnityEngine.Object.Destroy(gameObject);
		Holder.RemoveFromContainer();
	}

}
