using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControlPala : MonoBehaviour
{
    private bool moviendo = false;
    private Camera camaraPrincipal;
    public static ControlPala instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        ActualizarCamara();
        if (camaraPrincipal != null)
        {
            Vector3 centroCamara = camaraPrincipal.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
            centroCamara.z = 0;
            transform.position = centroCamara;
        }
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        ActualizarCamara();
    }

    void ActualizarCamara()
    {
        camaraPrincipal = Camera.main;
    }

    void Update()
    {
        var pointer = Pointer.current;
        if (pointer == null) return;
        if (camaraPrincipal == null) { ActualizarCamara(); return; }

        if (pointer.press.wasPressedThisFrame)
        {
            Vector2 posPantalla = pointer.position.ReadValue();
            Vector2 worldPos = camaraPrincipal.ScreenToWorldPoint(posPantalla);

            Collider2D objetoTocado = Physics2D.OverlapPoint(worldPos);

            if (objetoTocado != null && objetoTocado.gameObject == gameObject)
            {
                moviendo = true;
            }
        }

        if (moviendo)
        {
            Vector2 posPantalla = pointer.position.ReadValue();
            Vector3 worldPos = camaraPrincipal.ScreenToWorldPoint(posPantalla);
            transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
        }

        if (pointer.press.wasReleasedThisFrame && moviendo)
        {
            moviendo = false;
            // Siempre se desactiva al soltar
            // El anuncio se dispara desde LogicaPlantaUniversal via OnTriggerEnter2D
            gameObject.SetActive(false);
        }
    }
}