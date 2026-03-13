using UnityEngine.SceneManagement;

namespace Game.Core
{
    public static class SceneLoader
    {
        public const string MenuScene = "Menu";
        public const string GameplayScene = "Actual Game";
        public const string ResultsScene = "Results";

        public static void LoadMenu()
        {
            SceneManager.LoadScene(MenuScene);
        }

        public static void LoadGameplay()
        {
            SceneManager.LoadScene(GameplayScene);
        }

        public static void LoadResults()
        {
            SceneManager.LoadScene(ResultsScene);
        }
    }
}