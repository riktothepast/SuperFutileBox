using UnityEngine;
using System.Collections.Generic;

	public enum PageType
	{
		None,
		MenuPage,
		InGamePage
	}

public class SuperFutileBox : MonoBehaviour {
	
	public static SuperFutileBox instance;
	public FFont myfont;
	
	private PageType _currentPageType = PageType.None;
	private Page _currentPage = null;
	
	private FStage _stage;
	
	void Start () {
		RXDebug.Log("Starting the demo");
		instance = this;
		Go.defaultEaseType = EaseType.Linear;
		Go.duplicatePropertyRule = DuplicatePropertyRuleType.RemoveRunningProperty;
		FutileParams fparams = new FutileParams(true,true, false,false);

		fparams.AddResolutionLevel(480.0f,	1.0f,	1.0f, ""); //iPhone
//		fparams.AddResolutionLevel(960.0f,	2.0f,	2.0f,	"_Scale2"); //iPhone retina
		fparams.AddResolutionLevel(1024.0f,	1.0f,	1.0f,	""); //iPad
//		fparams.AddResolutionLevel(1280.0f,	2.0f,	2.0f,	"_Scale2"); //Nexus 7
//		fparams.AddResolutionLevel(2048.0f,	4.0f,	4.0f,	"_Scale4"); //iPad Retina

		fparams.origin = new Vector2(0.5f,0.5f);
		
		Futile.instance.Init (fparams);
		Futile.atlasManager.LoadAtlas("Atlases/SFB"); 
				Futile.atlasManager.LoadFont("font", "font", "Atlases/font", 0, 0);

		
		_stage = Futile.stage;
		
		 GoToPage(PageType.InGamePage);
		
		_stage.ListenForUpdate (HandleUpdate);
		
	}
	
	void HandleUpdate ()
	{
		
	}
	
	public void GoToPage (PageType pageType)
	{
		RXDebug.Log("Here i am changing the page");
		if(_currentPageType == pageType) return;

		Page pageToCreate = null;

		if(pageType == PageType.MenuPage)
		{
			pageToCreate = new MenuPage();
		}
		if(pageType == PageType.InGamePage)
		{
			pageToCreate = new InGamePage(2);
		}

		if(pageToCreate != null)
		{
			_currentPageType = pageType;	

			if(_currentPage != null)
			{
				_stage.RemoveChild(_currentPage);
			}

			_currentPage = pageToCreate;
			_stage.AddChild(_currentPage);
			_currentPage.Start();
			
		}

	}
	
}