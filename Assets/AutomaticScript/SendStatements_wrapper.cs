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

}
