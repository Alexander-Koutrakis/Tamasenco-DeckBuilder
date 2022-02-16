
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartScreen : MonoBehaviour
{

   public void LoadScene()
    {
        SceneManager.LoadScene("DeckBuilding");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
