using UnityEngine;
using System.Collections;

public class Enemy : Entity {
	public BoxCollider bodyCollider;
	public FParticleDefinition pd;
	public FPNodeLink bodyLink;
	int direction;
	int life;
	float speed=0.05f;
	public static Enemy Create(InGamePage world, Vector2 pos, int direction){
		GameObject GObj = new GameObject("Enemy");
		Enemy body = GObj.AddComponent<Enemy>();
		body.GamePage = world;
		body.Position=pos;
		body.Size=new Vector2(40,40);
		body.Sprite = new FSprite("Futile_White");
		Color cl =new Color(0,0,90,1);
		body.Sprite.color=cl;
		body.Sprite.width = body.Size.x;
		body.Sprite.height = body.Size.y;
		body.direction=direction;
		body.life=3;
		return body;
	}
	
	public override void Init(){
		GamePage._gameObjects.AddChild(Holder = new FContainer());
		gameObject.transform.position = new Vector3(Position.x * FPhysics.POINTS_TO_METERS,Position.y * FPhysics.POINTS_TO_METERS,32);
		Debug.Log(Position);
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
		
		pd = new FParticleDefinition("Futile_White");
		pd.endScale = 0.2f;
		pd.startColor = new Color(250,0,0,1f);
		pd.endColor = new Color (250, 0, 0,0.01f);
	}
	
	public override void InitPhysics(){
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
	
	public override void Update(){
		if(direction>0){
			gameObject.transform.position = new Vector3(gameObject.transform.position.x+speed, gameObject.transform.position.y, gameObject.transform.position.z);		
		}else{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x-speed, gameObject.transform.position.y, gameObject.transform.position.z);		
		}
		if(life<=0){
			generateParticles();
			Destroy();
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		if(collision.collider.name.Equals("Left")||collision.collider.name.Equals("Right")||collision.collider.name.Equals("Enemy"))
					direction*=-1;
		if(collision.collider.name.Equals("Bottom")){
					gameObject.transform.position = new Vector3(GamePage.SpawnPoint.x * FPhysics.POINTS_TO_METERS,GamePage.SpawnPoint.y * FPhysics.POINTS_TO_METERS,32);
					speed+=0.01f;
					//change sprite color ?
		}

    }
	
	public void RestLife(int x){
		life-=x;
	}
	
	void Destroy(){
		UnityEngine.Object.Destroy(gameObject);
		Holder.RemoveFromContainer();
	}
	
		void generateParticles(){
		int part=RXRandom.Range(16, 32);
			for(int x=0;x<part;x++){
				pd.x = gameObject.rigidbody.position.x*FPhysics.METERS_TO_POINTS + RXRandom.Range(-10.0f, 10.0f);
				pd.y = (gameObject.rigidbody.position.y)*FPhysics.METERS_TO_POINTS+ RXRandom.Range(-10.0f, 10.0f);
				Vector2 speed = RXRandom.Vector2Normalized() * RXRandom.Range(20.0f,120.0f);
				pd.speedX = speed.x;
				pd.speedY = speed.y;
				pd.lifetime = RXRandom.Range(0.5f, 2f);
				this.GamePage.impactParticles.AddParticle(pd);
			}
	}
}
