using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;


public class Target : MonoBehaviour
{

    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ConstantString.player)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
            UIManager.Instance.ChangeScreen(SceneName.GamePlay);
            GameStateManager.Instance.ChangeGameState(GameState.GamePlay);
            /*UIManager.Instance.ChangeScreen(SceneName.GameOver);
            GameStateManager.Instance.ChangeGameState(GameState.GameOver);*/

        }
    }
}
