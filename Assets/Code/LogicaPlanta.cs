using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogicaPlantaUniversal : MonoBehaviour 
{
    [Header("Configuración de esta Planta")]
    public int indicePlanta; 
    public Sprite[] etapas; 
    public Slider barra;
    public Sprite spriteMuerta;

    private SpriteRenderer render;
    private int etapaActual = 0;
    private int etapaAnterior = -1;
    
    [Header("Ajustes de Sonido")]
    public AudioClip sonidoAgua; 
    public AudioClip sonidoPala; 
    public AudioClip sonidoEvolucion; 
    private AudioSource bocinaPlanta; // Solo para agua (loop)

    private bool esperandoAnuncio = false;

    void Start() {
        render = GetComponent<SpriteRenderer>();
        bocinaPlanta = gameObject.AddComponent<AudioSource>();
        bocinaPlanta.playOnAwake = false; 
        
        float segundos = EstadoPlanta.ObtenerTiempoTranscurrido();
        EstadoPlanta.armonias[indicePlanta] -= segundos * 1.5f;
        SincronizarVisual();
    }

    void Update() {
        float armonia = EstadoPlanta.armonias[indicePlanta];
        if (barra != null) barra.value = armonia;

        if (armonia <= 0) {
            if (spriteMuerta != null) render.sprite = spriteMuerta;
            render.color = Color.gray;
            EstadoPlanta.vivas[indicePlanta] = false;
            etapaAnterior = -1;
        } else {
            render.color = Color.white;
            EstadoPlanta.vivas[indicePlanta] = true;
            gameObject.tag = "Untagged"; 
            SincronizarVisual();
        }
    }
    
    void SincronizarVisual() 
    {
        if (EstadoPlanta.vivas[indicePlanta]) {
            etapaActual = Mathf.FloorToInt(EstadoPlanta.progresos[indicePlanta] / 5f);
            etapaActual = Mathf.Clamp(etapaActual, 0, etapas.Length - 1);
            
            // 💥 LLAMADA GLOBAL: No depende de bocinaPlanta, así que no se cortará
            if (etapaActual > etapaAnterior && etapaAnterior != -1)
            {
                if (GestorSonidos.instancia != null) 
                    GestorSonidos.instancia.ReproducirEfecto(sonidoEvolucion);
            }
            etapaAnterior = etapaActual;

            if (etapas.Length > 0) render.sprite = etapas[etapaActual];
        }
    }

    void OnTriggerStay2D(Collider2D otro) {
        if (Time.timeScale == 0f) return;

        if (otro.CompareTag("Agua") && EstadoPlanta.vivas[indicePlanta]) {
            if (!bocinaPlanta.isPlaying) {
                bocinaPlanta.clip = sonidoAgua;
                bocinaPlanta.loop = true;
                bocinaPlanta.Play();
            }
            EstadoPlanta.armonias[indicePlanta] += 20f * Time.deltaTime;
            EstadoPlanta.armonias[indicePlanta] = Mathf.Clamp(EstadoPlanta.armonias[indicePlanta], 0, 100);

            if (EstadoPlanta.armonias[indicePlanta] > 60) {
                EstadoPlanta.progresos[indicePlanta] += Time.deltaTime;
            }
        }
    }

    void OnTriggerExit2D(Collider2D otro) {
        if (otro.CompareTag("Agua")) {
            if (bocinaPlanta.isPlaying) bocinaPlanta.Stop();
        }
    }

    void OnTriggerEnter2D(Collider2D otro) {
        if (Time.timeScale == 0f) return;

        if (otro.CompareTag("Pala") && !EstadoPlanta.vivas[indicePlanta] && !esperandoAnuncio) {
            esperandoAnuncio = true; 
            if (GestorMusica.instancia != null) GestorMusica.instancia.PausarMusica();

            if (ControlAnuncio.instancia != null) {
                ControlAnuncio.instancia.MostrarAnuncio();
                StartCoroutine(EsperarAnuncioYRevivir());
            } else {
                RevivirPlantaMecanica();
            }
        }
    }

    IEnumerator EsperarAnuncioYRevivir() {
        yield return null; 
        yield return new WaitUntil(() => Time.timeScale > 0f); 
        if (GestorMusica.instancia != null) GestorMusica.instancia.ReanudarMusica();
        RevivirPlantaMecanica();
    }

    void RevivirPlantaMecanica() {
        EstadoPlanta.armonias[indicePlanta] = 100f;
        EstadoPlanta.progresos[indicePlanta] = 0f;
        EstadoPlanta.vivas[indicePlanta] = true;
        gameObject.tag = "Untagged";
        etapaAnterior = -1;
        
        if (etapas.Length > 0) render.sprite = etapas[0];
        render.color = Color.white;

        // 💥 LLAMADA GLOBAL: El palazo ahora es independiente
        if (GestorSonidos.instancia != null) 
            GestorSonidos.instancia.ReproducirEfecto(sonidoPala);
            
        esperandoAnuncio = false; 
    }
}