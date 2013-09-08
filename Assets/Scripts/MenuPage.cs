using UnityEngine;
using System.Collections;

public class MenuPage : Page {
	

	
	public MenuPage()
	{
		ListenForUpdate(HandleUpdate);
		ListenForResize(HandleResize);
	}
	
	override public void Start () {

		
		
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
