using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{   
   public void LoadMainMenu()
   {
      SceneManager.LoadScene(0);
   }

   public void LoadSettings()
   {
      SceneManager.LoadScene(1);
   }

   public void LoadAudio()
   {
      SceneManager.LoadScene(2);
   }

   public void LoadGame()
   {
      SceneManager.LoadScene(3);
   }


   public void LoadCredits()
   {
      SceneManager.LoadScene(4);
   }

   public void QuitGame ()
   {
      Application.Quit();
   }

   void Update()
   {
      if(Input.GetKeyDown(KeyCode.Backspace))
      {
         LoadMainMenu();
      }
   }
}