using UnityEngine;
using FYFY;

[ExecuteInEditMode]
public class SendStatements_wrapper : MonoBehaviour
{
	private void Start()
	{
		this.hideFlags = HideFlags.HideInInspector; // Hide this component in Inspector
	}

	public void initGBLXAPI()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "initGBLXAPI", null);
	}

	public void testSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "testSendStatement", null);
	}

	public void JouerButtonSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "JouerButtonSendStatement", null);
	}

	public void QuitterButtonSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "QuitterButtonSendStatement", null);
	}

	public void GamePlayDuration()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "GamePlayDuration", null);
	}

	public void TurnLeftSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "TurnLeftSendStatement", null);
	}

	public void TurnRightSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "TurnRightSendStatement", null);
	}

	public void ForwardSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "ForwardSendStatement", null);
	}

	public void WaitSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "WaitSendStatement", null);
	}

	public void ActivateSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "ActivateSendStatement", null);
	}

	public void TurnBackSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "TurnBackSendStatement", null);
	}

	public void IFSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "IFSendStatement", null);
	}

	public void ForSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "ForSendStatement", null);
	}

	public void WhileSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "WhileSendStatement", null);
	}

	public void BackToMenuSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "BackToMenuSendStatement", null);
	}

	public void ExecuteSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "ExecuteSendStatement", null);
	}

	public void SpeedSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "SpeedSendStatement", null);
	}

	public void RestartLevelSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "RestartLevelSendStatement", null);
	}

	public void ResetlSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "ResetlSendStatement", null);
	}

	public void LevelSendStatement(System.Int32 i)
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "LevelSendStatement", i);
	}

	public void NextLevelSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "NextLevelSendStatement", null);
	}

	public void EndLevelSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "EndLevelSendStatement", null);
	}

	public void LevelDuration()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "LevelDuration", null);
	}

	public void BackToMenuAfterLevelSendStatement()
	{
		MainLoop.callAppropriateSystemMethod ("SendStatements", "BackToMenuAfterLevelSendStatement", null);
	}

}
