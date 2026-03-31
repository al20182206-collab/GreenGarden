using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cambiarescena : MonoBehaviour
{

    public void ChangeScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void TestButton(int sceneID)
    {
        SceneManager.LoadScene(0);
    }

}