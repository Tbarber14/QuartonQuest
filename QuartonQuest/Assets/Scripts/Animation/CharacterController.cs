using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Animations;

public class CharacterController : MonoBehaviour
{
    public Animator charAnim;
    // Start is called before the first frame update
    void Start()
    {
        charAnim = GetComponent<Animator>();
        GameCoreController.Instance.GameOver += GameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameCoreController.Instance.IsGameOver==false)
        {
            if (GameCoreController.Instance.CurrentTurn != GameCoreController.GameTurnState.OPPONENT)
            {
                if (name == "PlayerCharacter")
                {
                    charAnim.Play("Armature_ThinkingAction");
                }
            }
            else if (GameCoreController.Instance.CurrentTurn == GameCoreController.GameTurnState.OPPONENT)
            {
                if (name == "PlayerCharacter")
                {
                    charAnim.Play("Armature_ArmatureAction");
                }
            }
        }
    }

    public void GameOver()
    {

        if (GameCoreController.Instance.CurrentTurn == GameCoreController.GameTurnState.PLAYERWON)
        {
            if (name == "PlayerCharacter")
            {
                charAnim.Play("Armature_WinAction");
            }
            else
            {
                charAnim.Play("Armature_LoseAction");
            }
        }
        else if (GameCoreController.Instance.CurrentTurn == GameCoreController.GameTurnState.OPPONENTWON)
        {
            if (name != "PlayerCharacter")
            {
                charAnim.Play("Armature_WinAction");
            }
            else
            {
                charAnim.Play("Armature_LoseAction");
            }
        }
        else if (GameCoreController.Instance.CurrentTurn == GameCoreController.GameTurnState.GAMETIED)
        {
            //At tie, both play lose action
            charAnim.Play("Armature_LoseAction");
        }
    }
}
