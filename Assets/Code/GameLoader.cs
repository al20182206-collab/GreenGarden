using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private string nombreDeLaEscenaUI = "Escena_UI";

    void Start()
    {
        // Esto busca si la UI ya está cargada para no duplicarla
        Scene uiScene = SceneManager.GetSceneByName(nombreDeLaEscenaUI);

        if (!uiScene.isLoaded)
        {
            // La carga de forma "Aditiva" (encima de la actual)
            SceneManager.LoadScene(nombreDeLaEscenaUI, LoadSceneMode.Additive);
        }
    }
}
