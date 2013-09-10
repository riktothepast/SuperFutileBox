using UnityEngine;
using System.Collections;

public class MenuPage : Page {
	private FButton _startButton;
	private FLabel _titleLabel, _instLabel;
	
	public MenuPage()
	{
		ListenForUpdate(HandleUpdate);
		ListenForResize(HandleResize);
	}
	
	override public void Start () {
		_startButton = new FButton("boton");

		_startButton.AddLabel("font","Play!",new Color(0,0,20,1f));
		_startButton.scale=2f;
		_titleLabel = new FLabel("font","Super Futile Box");
		_instLabel = new FLabel("font","- Move with Arrow keys \n - Press 'space' to jump \n - Down key to fast fall \n - Avoid enemies!!! \n \n Tune:  Barymag #2 - 8 by Lukasz Sychowicz (X-Ray)");
		AddChild(_startButton);
		AddChild(_titleLabel);
		AddChild(_instLabel);
		_titleLabel.x = 0f;
		_titleLabel.y = 100f;	
		
		_instLabel.x = 0f;
		_instLabel.y = -150f;			
		
		
		_startButton.SignalRelease += HandleStartButtonRelease;
		
		_startButton.x = 0f;
		_startButton.y = 0f;	
		
			Go.to(_startButton, 0.5f, new TweenConfig().
			setDelay(0.3f).
			floatProp("scale",1.0f).
			setEaseType(EaseType.BackOut));
		
	}
	
	protected void HandleUpdate () {

	}
	
	private void HandleStartButtonRelease (FButton button)
	{
		SuperFutileBox.instance.GoToPage(PageType.InGamePage);
	}
	
	protected void HandleResize(bool wasOrientationChange){
		
	}
}
