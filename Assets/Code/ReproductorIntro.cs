using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ReproductorIntro : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nombreEscenaSiguiente = "Mapa";

    void Start()
    {
        videoPlayer.loopPointReached += AlTerminarVideo;
    }

    void AlTerminarVideo(VideoPlayer vp)
    {
        SceneManager.LoadScene(nombreEscenaSiguiente);
    }
}