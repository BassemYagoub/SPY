using UnityEngine;
using FYFY;
using DIG.GBLXAPI;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;


public class SendStatements : FSystem {
 
    private Family f_actionForLRS = FamilyManager.getFamily(new AllOfComponents(typeof(ActionPerformedForLRS)));

    public static SendStatements instance ;
    public static int LevelIndice;
    public static int nbAction=0;
    public static int nbActionExec=0;
    public SendStatements()
    {
        if (Application.isPlaying)
        {
            initGBLXAPI();
        }
        instance = this;
    }

    public void initGBLXAPI()
    {
        if (!GBLXAPI.IsInit)
            GBLXAPI.Init(GBL_Interface.lrsAddresses);

        GBLXAPI.debugMode = false;

        string sessionID = Environment.MachineName + "-" + DateTime.Now.ToString("yyyy.MM.dd.hh.mm.ss");
        //Generate player name unique to each playing session (computer name + date + hour)
        GBL_Interface.playerName = String.Format("{0:X}", sessionID.GetHashCode());
        //Generate a UUID from the player name
        GBL_Interface.userUUID = GBLUtils.GenerateActorUUID(GBL_Interface.playerName);
    }

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        // Do not use callbacks because in case in the same frame actions are removed on a GO and another component is added in another system, family will not trigger again callback because component will not be processed
        foreach (GameObject go in f_actionForLRS)
        {
            ActionPerformedForLRS[] listAP = go.GetComponents<ActionPerformedForLRS>();
            int nb = listAP.Length;
            ActionPerformedForLRS ap;
            if (!this.Pause)
            {
                for (int i = 0; i < nb; i++)
                {
                    ap = listAP[i];
                    //If no result info filled
                    if (!ap.result)
                    {
                        GBL_Interface.SendStatement(ap.verb, ap.objectType, ap.objectName, ap.activityExtensions);
                    }
                    else
                    {
                        bool? completed = null, success = null;

                        if (ap.completed > 0)
                            completed = true;
                        else if (ap.completed < 0)
                            completed = false;

                        if (ap.success > 0)
                            success = true;
                        else if (ap.success < 0)
                            success = false;

                        GBL_Interface.SendStatementWithResult(ap.verb, ap.objectType, ap.objectName, ap.activityExtensions, ap.resultExtensions, completed, success, ap.response, ap.score, ap.duration);
                    }
                }
            }
           for (int i = nb - 1; i > -1; i--)
            {
                GameObjectManager.removeComponent(listAP[i]);
            }
        }
	}
   	
    public void testSendStatement()
    {
        Debug.Log(GBL_Interface.playerName + " asks to send statement...");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "MyButton",
           /* activityExtensions = new Dictionary<string, string>() {
		  { "availableExtension1", "myContent1" },
		  { "availableExtension2", "myContent2" }
	     }*/
        });
     }   
    DateTime TimeClickJouer;
    public void JouerButtonSendStatement()
    {
   	TimeClickJouer = DateTime.Now;
   	Debug.Log(" time of click on Jouer : "+TimeClickJouer);
      
        
        Debug.Log("Button Jouer cliked by player { "+GBL_Interface.playerName + " } asks to send statement... ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "started",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" } click on Button 'Jouer 'at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ] Total Global Click on button ' Jouer ' for this player in this session of  Game is  "+1,
 
        });

   }
   DateTime TimeClickQuitter;
   public void QuitterButtonSendStatement()
    {
    	TimeClickQuitter = DateTime.Now;
   	Debug.Log(" time of click on Quitter : "+TimeClickQuitter);
   	
        Debug.Log("Button Quiter cliked by player { "+GBL_Interface.playerName + " } asks to send statement...");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "completed",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" } click on Button 'Quitter 'at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });

   }
   
   public void GamePlayDuration(){
   	System.TimeSpan diff = TimeClickQuitter.Subtract(TimeClickJouer);
   	Debug.Log(" PLayTimeDUration : [ "+diff+"  ]");
   	
   	 Debug.Log("Player { "+GBL_Interface.playerName + " } has quit the game ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "completed",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" } has quit the game at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ] the duration game [ "+diff+" ]",
 
        });
   	
   }
   
   public void TurnLeftSendStatement(){
   	nbAction +=1;
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } Drag&drop  TurnLeft instruction  ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  Drag&drop  TurnLeft instruction  at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
   
   public void TurnRightSendStatement(){
   	nbAction +=1;
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } Drag&drop  TurnRight instruction  ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  Drag&drop  TurnRight instruction  at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
    public void ForwardSendStatement(){
   	nbAction +=1;
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } Drag&drop  Forward instruction  ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  Drag&drop  Forward instruction  at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
    public void WaitSendStatement(){
   	nbAction +=1;
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } Drag&drop  Wait instruction  ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  Drag&drop  Wait instruction  at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
   public void ActivateSendStatement(){
   	nbAction +=1;
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } Drag&drop  Activate instruction  ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  Drag&drop  Activate instruction  at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
   public void TurnBackSendStatement(){
   	nbAction +=1;
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } Drag&drop  TurnBack instruction  ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  Drag&drop  TurnBack instruction  at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
   public void IFSendStatement(){
   	nbAction +=1;
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } Drag&drop  IF instruction  ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  Drag&drop  IF instruction  at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
   
   public void ForSendStatement(){
   	nbAction +=1;
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } Drag&drop  For instruction  ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  Drag&drop  For instruction  at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
   
    public void WhileSendStatement(){
   	 nbAction +=1;
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } Drag&drop  While instruction  ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  Drag&drop  While instruction  at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
   
   public void BackToMenuSendStatement(){
   	TotalActionInLevel();
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } click on button Back To menu");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  click on button Back To menu  at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
  
  public void ExecuteSendStatement(){
   	 nbActionExec +=nbAction ;
   	 nbAction=0;
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } click on button Execute ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  click on button Execute at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
   public void SpeedSendStatement(){
   	
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } click on button Speed execution ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  click on button Speed execution at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
   
    public void RestartLevelSendStatement(){
         TotalActionInLevel();
   	
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } click on button Restart Level ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  click on button Restart Level at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
    public void ResetlSendStatement(){
   	
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } click on button Reset ");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  click on button reset at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
   public static DateTime TimestartLevel;
   public void LevelSendStatement(int i){
   	
   	        LevelIndice = i;
   	        TimestartLevel = DateTime.Now;
	   	Debug.Log("player  { "+GBL_Interface.playerName + " } click on Level [ "+i+" ].....Level Stared at  [ "+TimestartLevel+" ]");
		GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
		{
		    verb = "interacted",
		    objectType = "menu",
		    objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  click on Level [ "+i+" ]   ....Level Started  at date [  "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
	 
		});
   	
       } 
  public void NextLevelSendStatement(){
  		LevelIndice +=1;
  		TimestartLevel = DateTime.Now;
	   	Debug.Log("player  { "+GBL_Interface.playerName + " } click on buttonNextLevel [ "+LevelIndice+" ].....Level Stared at  [ "+TimestartLevel+" ]");
		GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
		{
		    verb = "interacted",
		    objectType = "menu",
		    objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  click on buttonNextLevel [ "+LevelIndice+" ]   ....Level Started  at date [  "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
	 
		});
   	
       } 
       
   public static DateTime TimeEndLevel;
   public void EndLevelSendStatement(){
        	TimeEndLevel = DateTime.Now;
   	        Debug.Log(" time end of level : "+TimeEndLevel);
	   	Debug.Log("player  { "+GBL_Interface.playerName + " } End Level [ "+(LevelIndice)+" ]");
		GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
		{
		    verb = "interacted",
		    objectType = "menu",
		    objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  completed Level [ "+LevelIndice+" ]   ....  at date [  "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ] Congratulations ! ",
	 
		});
   	
       } 
       
   public void LevelDuration(){
   	System.TimeSpan diff = TimeEndLevel.Subtract(TimestartLevel);
   	Debug.Log("Player { "+GBL_Interface.playerName + " } time duration in level "+LevelIndice+" [ "+diff+"  ]");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "completed",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" } time duration in level "+LevelIndice+" is :  [ "+diff+"  ]",
 
        });
   	
   }
    public void TotalActionInLevel(){
   	
   	Debug.Log("Player { "+GBL_Interface.playerName + " } Total Action  in level "+LevelIndice+" [ "+nbActionExec+"  ]");
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "completed",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" } Total Action  in level "+nbActionExec+" is :  [ "+nbAction+"  ]",
 
        });
        nbActionExec	 = 0;
        nbAction  =0;
   	
   }
   public void BackToMenuAfterLevelSendStatement(){
   	
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } click on button Back To menu After end of Level "+LevelIndice);
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  click on button Back To menu After end of Level "+LevelIndice+" at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
   public void ReloadLevelSendStatement(){
   	
   	 Debug.Log("player  { "+GBL_Interface.playerName + " } click on button Reload Level "+LevelIndice);
        GameObjectManager.addComponent<ActionPerformedForLRS>(MainLoop.instance.gameObject, new
        {
            verb = "interacted",
            objectType = "menu",
            objectName = "Grp4_B.R.S :  Player {"+GBL_Interface.playerName+" }  click on button Reload Level "+LevelIndice+" at date [ "+ DateTime.Now.ToString("dd/MM/yyyy")+" ] Time : [ "+DateTime.Now.ToString("hh : mm :ss")+" ]",
 
        });
   	
   }
}
