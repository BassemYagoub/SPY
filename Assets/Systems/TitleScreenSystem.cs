using UnityEngine;
using FYFY;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Xml;
using DIG.GBLXAPI;
using System.Collections;


/// <summary>
/// Manage main menu to launch a specific mission
/// </summary>
public class TitleScreenSystem : FSystem {
	private GameData gameData;
	private GameObject campagneMenu;
	private GameObject playButton;
	private GameObject quitButton;
	private GameObject backButton;
	private GameObject cList;
	private Dictionary<GameObject, List<GameObject>> levelButtons; //key = directory button,  value = list of level buttons
	private GameObject mainLoop;
        private SendStatements instance = new  SendStatements() ;
        

        void TaskWithParameters(string message)
        {
		//Output this to console when the Button2 is clicked
		Debug.Log(message);
        }
        /*public void LevelSendStatement(){
   	
	   	Debug.Log("player  { "+GBL_Interface.playerName + " } click on Level .....Level Stared ");
		GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
		{
		    verb = "interacted",
		    objectType = "menu",
		    objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  click on Level ....Level Started  at date [ "
	 
		});
   	
       }*/
	public TitleScreenSystem(){
		if (Application.isPlaying)
		{
			gameData = GameObject.Find("GameData").GetComponent<GameData>();
			gameData.levelList = new Dictionary<string, List<string>>();
			campagneMenu = GameObject.Find("CampagneMenu");
			playButton = GameObject.Find("Jouer");
			quitButton = GameObject.Find("Quitter");
			backButton = GameObject.Find("Retour");
			GameObjectManager.dontDestroyOnLoadAndRebind(GameObject.Find("GameData"));

			cList = GameObject.Find("CampagneList");
			levelButtons = new Dictionary<GameObject, List<GameObject>>();

			GameObjectManager.setGameObjectState(campagneMenu, false);
			GameObjectManager.setGameObjectState(backButton, false);
			string levelsPath = Application.streamingAssetsPath + Path.DirectorySeparatorChar + "Levels";
			List<string> levels;
			foreach (string directory in Directory.GetDirectories(levelsPath))
			{
				levels = readScenario(directory);
				if (levels != null)
				{
					gameData.levelList[Path.GetFileName(directory)] = levels; //key = directory name
				}
			}

			//create level directory buttons
			foreach (string key in gameData.levelList.Keys)
			{
				GameObject directoryButton = Object.Instantiate<GameObject>(Resources.Load("Prefabs/Button") as GameObject, cList.transform);
				directoryButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = key;
				levelButtons[directoryButton] = new List<GameObject>();
				GameObjectManager.bind(directoryButton);
				// add on click
				directoryButton.GetComponent<Button>().onClick.AddListener(delegate {showLevels(directoryButton); });
				
				// create level buttons
				for (int i = 0; i < gameData.levelList[key].Count; i++)
				{
					GameObject button = Object.Instantiate<GameObject>(Resources.Load("Prefabs/LevelButton") as GameObject, cList.transform);
					button.transform.Find("Button").GetChild(0).GetComponent<TextMeshProUGUI>().text = Path.GetFileNameWithoutExtension(gameData.levelList[key][i]);
					int indice = i;
					
					button.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {  instance.LevelSendStatement();MainLoop.instance.StartCoroutine( launchLevel_delay(key, indice));  });

					levelButtons[directoryButton].Add(button);
					GameObjectManager.bind(button);
					GameObjectManager.setGameObjectState(button, false);
				}
			}
		}
	}

	private List<string> readScenario(string repositoryPath){
		if(File.Exists(repositoryPath+Path.DirectorySeparatorChar+"Scenario.xml")){
			List<string> levelList = new List<string>();
			XmlDocument doc = new XmlDocument();
			doc.Load(repositoryPath+Path.DirectorySeparatorChar+"Scenario.xml");
			XmlNode root = doc.ChildNodes[1]; //root = <scenario/>
			foreach(XmlNode child in root.ChildNodes){
				if (child.Name.Equals("level")){
					levelList.Add(repositoryPath + Path.DirectorySeparatorChar + (child.Attributes.GetNamedItem("name").Value));
				}
			}
			return levelList;			
		}
		return null;
	}

	protected override void onProcess(int familiesUpdateCount) {
		if(Input.GetButtonDown("Cancel")){
			Application.Quit();
		}
	}

	// See Jouer button in editor
	public void showCampagneMenu(){
		GameObjectManager.setGameObjectState(campagneMenu, true);
		int i = 0;
		foreach(GameObject directory in levelButtons.Keys) {
			//hide directory buttons
			GameObjectManager.setGameObjectState(directory, false);

			//only show levels from "Campagne" folder
			if (i <= 0) {
				showLevels(directory);
            }
            else { break; }
			i++;
		}
		GameObjectManager.setGameObjectState(playButton, false);
		GameObjectManager.setGameObjectState(quitButton, false);
		GameObjectManager.setGameObjectState(backButton, true);
	}

	private void showLevels(GameObject levelDirectory) {
		//show/hide levels
		foreach (GameObject directory in levelButtons.Keys){
			//hide level directories
			GameObjectManager.setGameObjectState(directory, false);
			//show levels
			if(directory.Equals(levelDirectory)){
				//foreach(GameObject go in levelButtons[directory]){
				for(int i = 0 ; i < levelButtons[directory].Count ; i ++){
					GameObjectManager.setGameObjectState(levelButtons[directory][i], true);

					string directoryName = levelDirectory.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
					//locked levels
					if (i > PlayerPrefs.GetInt(directoryName, 0)) { //by default first level of directory is the only unlocked level of directory
						levelButtons[directory][i].transform.Find("Button").GetComponent<Button>().interactable = true; //old version
						//GameObjectManager.setGameObjectState(levelButtons[directory][i].transform.Find("Button").gameObject, false);
					}
					else { //unlocked levels
						levelButtons[directory][i].transform.Find("Button").GetComponent<Button>().interactable = true;

						//scores
						int scoredStars = PlayerPrefs.GetInt(directoryName + Path.DirectorySeparatorChar + i + gameData.scoreKey, 0); //0 star by default
						Transform scoreCanvas = levelButtons[directory][i].transform.Find("ScoreCanvas");
						for (int nbStar = 0; nbStar < 4; nbStar++) {
							if (nbStar == scoredStars)
								GameObjectManager.setGameObjectState(scoreCanvas.GetChild(nbStar).gameObject, true);
							else
								GameObjectManager.setGameObjectState(scoreCanvas.GetChild(nbStar).gameObject, false);
						}
					}
				}
			}
			//hide other levels
			else{
				foreach(GameObject go in levelButtons[directory]){
					GameObjectManager.setGameObjectState(go, false);
				}
			}
		}
	}

	public void launchLevel(string levelDirectory, int level){
		
		gameData.levelToLoad = (levelDirectory,level);
		GameObjectManager.loadScene("MainScene");
	}
	public IEnumerator launchLevel_delay(string levelDirectory, int level){
		yield return null;
		yield return null;
		launchLevel(levelDirectory,level);
	}

	// See Retour button in editor
	public void backFromCampagneMenu(){
		foreach(GameObject directory in levelButtons.Keys){
			//if(directory.activeSelf){
				//main menu
				GameObjectManager.setGameObjectState(campagneMenu, false);
				GameObjectManager.setGameObjectState(playButton, true);
				GameObjectManager.setGameObjectState(quitButton, true);
				GameObjectManager.setGameObjectState(backButton, false);
				break;
			/*}
			else{
				//show directory buttons
				GameObjectManager.setGameObjectState(directory, true);
				//hide level buttons
				foreach(GameObject go in levelButtons[directory]){
					GameObjectManager.setGameObjectState(go, false);
				}
			}*/
		}
	}

	// See Quitter button in editor
	public void quitGame(){
		Application.Quit();
	}
}
