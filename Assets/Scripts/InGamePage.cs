using UnityEngine;
using System.Collections.Generic;
using System;

public class InGamePage : Page {
	// Use this for initialization
	public FContainer _playerSpheres;
	public FContainer _gameObjects;
	public FContainer _enemyContainer;
	public FContainer _playerBullets;
	float t,ellapsed;
	public List<Platform> platforms = new List<Platform>();
	public List<Projectile> projectiles = new List<Projectile>();
	public List<Enemy> enemies = new List<Enemy>();
	Player jugador;
	public Vector2 SpawnPoint=new Vector2(0,350);
	public bool leftArrow, rightArrow;
	RXCircle midBounds;
	public FParticleSystem impactParticles;
	public FParticleSystem projectilesParticles;
	FLabel scoreLabel, gameOver;
	int _frameCount = 0;
	int Score;
	public FPWorld root;
	
	public InGamePage(){
		
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
		gameOver = new FLabel("font", "Game Over, press 'R' to try again!");
		scoreLabel.SetAnchor(new Vector2(0,-10));
		AddChild(scoreLabel);
		
		FSoundManager.PlayMusic("Barymag");
		FSoundManager.isMuted=false;
	}
	
	/*
	 * La actualizacion se llama cada cuadro de actualizacion
	 */
	public void Update () {
		
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
		if (Input.GetKeyDown (KeyCode.DownArrow)){
			jugador.getDown();
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow)){
			leftArrow=false;
		}
		if (Input.GetKeyUp (KeyCode.RightArrow)){
			rightArrow=false;
		}	
			//para evitar llenar de enmigos, solo crearemos uno si se cumple esta condicion
		if((_frameCount%120)==0){
				GenerateMonster();
		}
		}else{
			if (Input.GetKeyDown (KeyCode.R)){
			RemoveChild(_enemyContainer);
			_enemyContainer = new FContainer(); 
				for(int x = 0; x<enemies.Count; x++)
					enemies[x].Destroy();
			AddChild(_enemyContainer);
			Score=0;
			GameOver=false;
			_playerBullets.RemoveAllChildren();
			FSoundManager.isMuted=false;		
				FSoundManager.PlayMusic("Barymag");
			InitPlayer();
						scoreLabel.text="Score = "+Score;
			RemoveChild(gameOver);
				rightArrow=false;
				leftArrow=false;
			}
		}
				_frameCount++;
	}
	
	/*
	 * Creamos plataformas para el juego y "paredes izquierda y derecha"
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
		var arr1 = new[]{1,-1};
		var rndMember = arr1[new System.Random().Next(arr1.Length)];
		Enemy enemigo;
		enemigo = Enemy.Create(this,SpawnPoint,rndMember);
		enemies.Add (enemigo);
		enemigo.Init();
	}
	
	public void setScore(int x){
		Score+=x;	
		scoreLabel.text="Score = "+ Score;
	}
	public void callGameOver(){
		GameOver=true;
		AddChild(gameOver);
		_enemyContainer.RemoveAllChildren();
		FSoundManager.StopMusic();
	}
	
	//set to get
	public Vector2 screenCenter{set;get;}
	public bool GameOver{set;get;}
}