﻿using UnityEngine;
using System.Collections;

public class Player : Entity {
	
	public FPNodeLink bodyLink;
	public BoxCollider bodyCollider;
	public FParticleDefinition pd;
	bool canJump=false;
	bool facingLeft;
	private FAnimatedSprite sprite;
	
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
		
		body.sprite = new FAnimatedSprite(new FAnimation("standing","standing",40,40,0,0,2,120,true));
		body.sprite.addAnimation(new FAnimation("walking","standing",40,40,0,1,2,120,true));
		
		return body;
	}
	
	public override void Init ()
	{
		GamePage._gameObjects.AddChild(Holder = new FContainer());
		gameObject.transform.position = new Vector3(Position.x * FPhysics.POINTS_TO_METERS,Position.y * FPhysics.POINTS_TO_METERS,32);
		gameObject.transform.parent = GamePage.root.transform;
		bodyLink = gameObject.AddComponent<FPNodeLink>();
		bodyLink.Init(Holder, false);
		Holder.AddChild(sprite);
		AngularDrag=5f;
		Mass=1f;
		Bounciness=0.5f;
		FrictionDyn=0.3f;
		FrictionSta=0.2f;
		InitPhysics();
		canJump=true;
		
		pd = new FParticleDefinition("Futile_White");
		pd.endScale = 0.2f;
		pd.startColor = new Color(80,90,0,0.8f);
		pd.endColor = new Color (250, 250, 250,0.1f);
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
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
		if(collision.collider.name.Equals("Bottom"))
				kill ();
			
		if(collision.collider.name.Equals("Enemy"))
			kill ();
		
		if(collision.collider.name.Equals("Platform"))
					generateParticles();
    }
	
	public override void Update(){
		if(this.GamePage.leftArrow){
			facingLeft=true;
			sprite.scaleX = -1.0f;
			gameObject.transform.position = new Vector3(gameObject.transform.position.x-0.1f, gameObject.transform.position.y, gameObject.transform.position.z);		
			sprite.play("walking");
		}else
		if(this.GamePage.rightArrow){
			facingLeft=false;
			sprite.scaleX = 1.0f;
			gameObject.transform.position = new Vector3(gameObject.transform.position.x+0.1f, gameObject.transform.position.y, gameObject.transform.position.z);		
			sprite.play("walking");
		}else{
			sprite.play("standing");
		}
	}
	
	public void setJump(){
		if(canJump){
			canJump=false;
			gameObject.rigidbody.AddRelativeForce(transform.up * 8, ForceMode.Impulse);
			generateParticles();
			FSoundManager.PlaySound("jump");

		}
	}
	
	public void getDown(){
			gameObject.rigidbody.AddRelativeForce(new Vector2(0,-1) * 8, ForceMode.Impulse);
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
				FSoundManager.PlaySound("laser");

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
	
	void kill (){
		pd = new FParticleDefinition("Futile_White");
		pd.endScale = 0.2f;
		pd.startColor = new Color(250,0,0,0.8f);
		pd.endColor = new Color (250, 0, 0,0.1f);
		int part=RXRandom.Range(16, 32);
			for(int x=0;x<part;x++){
				pd.x = gameObject.rigidbody.position.x*FPhysics.METERS_TO_POINTS + RXRandom.Range(-10.0f, 10.0f);
				pd.y = (gameObject.rigidbody.position.y)*FPhysics.METERS_TO_POINTS-+ RXRandom.Range(-10.0f, 10.0f);
				Vector2 speed = RXRandom.Vector2Normalized() * RXRandom.Range(20.0f,80.0f);
				pd.speedX = speed.x;
				pd.speedY = speed.y;
				pd.lifetime = RXRandom.Range(1f, 5f);
				this.GamePage.impactParticles.AddParticle(pd);
			}
		UnityEngine.Object.Destroy(gameObject);
		Holder.RemoveFromContainer();
		GamePage.callGameOver();
						FSoundManager.PlaySound("atari_boom5");

	}
	
	void setAtSpawnPoint(){
		gameObject.transform.position = new Vector3(GamePage.SpawnPoint.x*FPhysics.POINTS_TO_METERS, GamePage.SpawnPoint.y*FPhysics.POINTS_TO_METERS, gameObject.transform.position.z);		
		Debug.Log(gameObject.transform.position);
	}
}