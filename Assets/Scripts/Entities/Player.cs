using UnityEngine;
using System.Collections;

public class Player : Entity {
	
	public FPNodeLink bodyLink;
	public BoxCollider bodyCollider;
	public FParticleDefinition pd;
	bool canJump=false;
	bool facingLeft;
	public static Player Create(InGamePage world, Vector2 pos){
		GameObject GObj = new GameObject("JumpMan");
		Player body = GObj.AddComponent<Player>();
		body.GamePage = world;
		body.Position=pos;
		body.Size=new Vector2(40,40);
		body.Sprite = new FSprite("Futile_White");
		Color cl =new Color(0,90,0,1);
		body.Sprite.color=cl;
		body.Sprite.width = body.Size.x;
		body.Sprite.height = body.Size.y;

		return body;
	}
	
	public override void Init ()
	{
		GamePage._gameObjects.AddChild(Holder = new FContainer());
		gameObject.transform.position = new Vector3(Position.x * FPhysics.POINTS_TO_METERS,Position.y * FPhysics.POINTS_TO_METERS,32);
		gameObject.transform.parent = GamePage.root.transform;
		bodyLink = gameObject.AddComponent<FPNodeLink>();
		bodyLink.Init(Holder, false);
		Holder.AddChild(Sprite);
		AngularDrag=5f;
		Mass=1f;
		Bounciness=0.5f;
		FrictionDyn=0.3f;
		FrictionSta=0.2f;
		InitPhysics();
		canJump=true;
		
		pd = new FParticleDefinition("Futile_White");
		pd.endScale = 0.2f;
		pd.startColor = new Color(80,90,0,1);
		pd.endColor = new Color (250, 250, 250,1f);
	}
	
	public override void InitPhysics()
	{
		Rigidbody rb = gameObject.AddComponent<Rigidbody>();
		rb.constraints =  RigidbodyConstraints.FreezeRotation;
		rb.angularDrag = AngularDrag;
		rb.mass = Mass;
		bodyCollider = gameObject.AddComponent<BoxCollider>();
		bodyCollider.size=new Vector3(Size.x * FPhysics.POINTS_TO_METERS,Size.y* FPhysics.POINTS_TO_METERS,32);
		PhysicMaterial mat = new PhysicMaterial();
		mat.bounciness = Bounciness;
		mat.dynamicFriction = FrictionDyn;
		mat.staticFriction = FrictionSta;
		mat.frictionCombine = PhysicMaterialCombine.Maximum;
		collider.material = mat;
	}
	
	void OnCollisionEnter(Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {
			if(collision.collider.name.Equals("Platform")&&(contact.normal.y>0)){
				canJump=true;
			}
			if(collision.collider.name.Equals("Enemy")){
				//kill the player
				
				
			}
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
		if(collision.collider.name.Equals("Platform"))
					generateParticles();
    }
	
	public override void Update(){
		if(this.GamePage.leftArrow){
			facingLeft=true;
			gameObject.transform.position = new Vector3(gameObject.transform.position.x-0.1f, gameObject.transform.position.y, gameObject.transform.position.z);		
		}
		if(this.GamePage.rightArrow){
			facingLeft=false;
			gameObject.transform.position = new Vector3(gameObject.transform.position.x+0.1f, gameObject.transform.position.y, gameObject.transform.position.z);		
		}
	}
	
	public void setJump(){
		if(canJump){
			canJump=false;
			gameObject.rigidbody.AddRelativeForce(transform.up * 8, ForceMode.Impulse);
			generateParticles();
		}
	}
	
	public void setAttack(){
		Projectile pro;
		Vector2 spawnpos;
		if(facingLeft){
			spawnpos=gameObject.transform.position*FPhysics.METERS_TO_POINTS;
			spawnpos.x-=20f;
		 pro = Projectile.Create(GamePage,spawnpos , new Vector2(-1,0), new FSprite("Futile_White"));
		}else{
			spawnpos=gameObject.transform.position*FPhysics.METERS_TO_POINTS;
			spawnpos.x+=20f;
		 pro = Projectile.Create(GamePage,spawnpos , new Vector2(1,0), new FSprite("Futile_White"));
		}pro.Init();
	}
	
	void generateParticles(){
		int part=RXRandom.Range(2, 4);
			for(int x=0;x<part;x++){
				pd.x = gameObject.rigidbody.position.x*FPhysics.METERS_TO_POINTS + RXRandom.Range(-10.0f, 10.0f);;
				pd.y = (gameObject.rigidbody.position.y)*FPhysics.METERS_TO_POINTS-10;
				Vector2 speed = RXRandom.Vector2Normalized() * RXRandom.Range(20.0f,80.0f);
				pd.speedX = speed.x;
				pd.speedY = speed.y;
				pd.lifetime = RXRandom.Range(0.2f, 0.5f);
				this.GamePage.impactParticles.AddParticle(pd);
			}
	}
}