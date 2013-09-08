using UnityEngine;
using System.Collections.Generic;
using System;

public class InGamePage : Page {
	// Use this for initialization
	int mode=0;
	public FContainer _playerSpheres;
	public FContainer _gameObjects;
	public FContainer _enemyContainer;
	public FContainer _playerBullets;
	float t,ellapsed;
	public List<Platform> platforms = new List<Platform>();
	public List<Projectile> projectiles = new List<Projectile>();
	public List<Enemy> enemies = new List<Enemy>();
	Player jugador;
	public Vector2 SpawnPoint=new Vector2(0,300);
	public bool leftArrow, rightArrow;
	RXCircle midBounds;
	public FParticleSystem impactParticles;
	public FParticleSystem projectilesParticles;
	FLabel scoreLabel, gameOver;
	int _frameCount = 0;
	int Score;
	public FPWorld root;
	
	public InGamePage(int difficulty){
		mode = difficulty;
	}

	override public void Start () {
		root = FPWorld.Create(64.0f);
		screenCenter=new Vector2(Futile.screen.halfWidth,Futile.screen.halfHeight);
		ListenForUpdate(Update);

		_gameObjects = new FContainer(); 
		AddChild(_gameObjects);
		_enemyContainer = new FContainer(); 
		AddChild(_enemyContainer);
		AddChild(impactParticles = new FParticleSystem(40));
		AddChild(projectilesParticles = new FParticleSystem(300));
		_playerBullets = new FContainer(); 
		AddChild(_playerBullets);
		
		t = Time.time;
		
		CreateWorld();
		InitPlayer();
		scoreLabel = new FLabel("font", "Score = "+ Score);
		gameOver = new FLabel("font", "Game Over, try again!");
		scoreLabel.SetAnchor(new Vector2(0,-10));
		AddChild(scoreLabel);
	}
	
	// Update is called once per frame
	public void Update () {
		
		foreach(Platform platform in platforms){
			platform.Update();
		}
		if(!GameOver){
		//Key Controllers
		if (Input.GetKeyDown (KeyCode.LeftArrow)){
			leftArrow=true;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)){
			rightArrow=true;
		}
		if (Input.GetKeyDown (KeyCode.Space)){
			jugador.setJump();
		}
		if (Input.GetKeyDown (KeyCode.X)){
			jugador.setAttack();
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow)){
			leftArrow=false;
		}
		if (Input.GetKeyUp (KeyCode.RightArrow)){
			rightArrow=false;
		}	
		}
		if((_frameCount%120)==0){
				GenerateMonster();
		}
				_frameCount++;
		
		
	}
	
	/*
	 * Creates the boundaries for the game, platforms and wall "limits"
	 * 
	 */
	
	void CreateWorld(){
		//top
		Platform plat= Platform.Create(this,new Vector2(-340,50),0f,new Vector2(340,40),new FSprite("platform"), "Platform");
		plat.Init();
		plat= Platform.Create(this,new Vector2(340,50),0f,new Vector2(340,40),new FSprite("platform"),"Platform");
		plat.Init();
		plat= Platform.Create(this,new Vector2(0,250),0f,new Vector2(340,40),new FSprite("platform"),"Platform");
		plat.Init();
		//middle
		plat= Platform.Create(this,new Vector2(0,-150),0f,new Vector2(340,40),new FSprite("platform"),"Platform");
		plat.Init();		
		//bottoms
		plat= Platform.Create(this,new Vector2(-340,-340),0f,new Vector2(340,40),new FSprite("platform"),"Platform");
		plat.Init();
		plat= Platform.Create(this,new Vector2(340,-340),0f,new Vector2(340,40),new FSprite("platform"),"Platform");
		plat.Init();
		//left to right
		plat= Platform.Create(this,new Vector2(520,0),0f,new Vector2(40,1000),new FSprite("bounds"),"Left");
		plat.Init();
		plat= Platform.Create(this,new Vector2(-520,0),0f,new Vector2(40,1000),new FSprite("bounds"),"Right");
		plat.Init();
		//enemy goal
		plat= Platform.Create(this,new Vector2(0,-380),0f,new Vector2(340,40),new FSprite("Futile_White"),"Bottom");
		plat.Init();
	}
	
	void InitPlayer(){
		jugador= Player.Create(this, new Vector2(0,0));
		jugador.Init();
	}
	
	void GenerateMonster(){
		Enemy enemigo = Enemy.Create(this,SpawnPoint,1);
		enemigo.Init();
	}
	
	
	/*
	 * TouchKit gesture listeners.	
	 */
	
	void setGestureSignals(){
		var TapRecognizer = new TKTapRecognizer();
		var LongTapRecognizer = new TKLongPressRecognizer();
		var LeftSwipeRecognizer = new TKSwipeRecognizer( TKSwipeDirection.Left);
		var RightSwipeRecognizer = new TKSwipeRecognizer( TKSwipeDirection.Left);
		var PinchRecognizer = new TKPinchRecognizer();
		var RotationRecognizer = new TKRotationRecognizer();
		
		TapRecognizer.gestureRecognizedEvent += ( r ) =>
			{
				Debug.Log( "tap recognizer fired: " + r );
				
			};
		TouchKit.addGestureRecognizer( TapRecognizer );
		
		LongTapRecognizer.gestureRecognizedEvent += ( r ) =>
			{
				Debug.Log( "long press recognizer fired: " + r );
			};
		LongTapRecognizer.gestureCompleteEvent += ( r ) =>
			{
				Debug.Log( "long press recognizer finished: " + r );
			};
			TouchKit.addGestureRecognizer( LongTapRecognizer );
		
		LeftSwipeRecognizer.gestureRecognizedEvent += ( r ) =>
			{
				Debug.Log( "left swipe recognizer fired: " + r );
			};
			TouchKit.addGestureRecognizer( LeftSwipeRecognizer );
		
		RightSwipeRecognizer.gestureRecognizedEvent += ( r ) =>
			{
				Debug.Log( "right swipe recognizer fired: " + r );
			};
			TouchKit.addGestureRecognizer( RightSwipeRecognizer );
		
		PinchRecognizer.gestureRecognizedEvent += ( r ) =>
			{
				Debug.Log( "pinch recognizer fired: " + r );
			};
			TouchKit.addGestureRecognizer( PinchRecognizer );
		
		RotationRecognizer.gestureRecognizedEvent += ( r ) =>
			{
				Debug.Log( "rotation recognizer fired: " + r );
			};
			TouchKit.addGestureRecognizer( RotationRecognizer );
	}
	public void setScore(int x){
		Score+=x;	
		scoreLabel.text="Score = "+ Score;
	}
	public void callGameOver(){
		GameOver=true;
		AddChild(gameOver);
	}
	
	//set to get
	public Vector2 screenCenter{set;get;}
	public bool GameOver{set;get;}
}