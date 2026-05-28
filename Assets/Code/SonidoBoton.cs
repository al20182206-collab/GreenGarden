using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SonidoBoton : MonoBehaviour
{
    [Header("Ajustes de Sonido")]
    public AudioClip sonidoClick; 

    private Button boton;
    private static AudioSource bocinaUIGlobal; 

    void Start()
    {
        boton = GetComponent<Button>();

        // 1. Si no hay bocina global, creamos una que sea INMORTAL
        if (bocinaUIGlobal == null)
        {
            // Creamos un objeto vacío solo para el sonido
            GameObject objetoBocina = new GameObject("BocinaUI_Global");
            bocinaUIGlobal = objetoBocina.AddComponent<AudioSource>();
            
            // LA MAGIA: Le decimos a Unity que NO borre esta bocina al cambiar de escena
            DontDestroyOnLoad(objetoBocina); 
        }

        // 2. Conectamos el clic del botón
        boton.onClick.AddListener(ReproducirSonidoUI);
    }

    void ReproducirSonidoUI()
    {
        if (sonidoClick != null && bocinaUIGlobal != null)
        {
            bocinaUIGlobal.PlayOneShot(sonidoClick);
        }
    }
}