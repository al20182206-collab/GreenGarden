using UnityEngine;
using UnityEngine.UI;

public class LogicaPlantaUniversal : MonoBehaviour 
{
    [Header("Configuración de esta Planta")]
    public int indicePlanta; // 0 para la primera, 1 para la segunda, 2 para la tercera

    public Sprite[] etapas; 
    public Slider barra;
    public Sprite spriteMuerta;

    private SpriteRenderer render;
    private int etapaActual = 0;
    
    [Header("Ajustes de Sonido")]
    public AudioClip sonidoAgua; // Arrastra aquí tu audio de agua/lluvia (.mp3 o .wav)
    private AudioSource bocinaPlanta;

    void Start() {
        render = GetComponent<SpriteRenderer>();
        
        // Creamos automáticamente el componente AudioSource para no tener que agregarlo a mano en el inspector
        bocinaPlanta = gameObject.AddComponent<AudioSource>();
        bocinaPlanta.clip = sonidoAgua;
        bocinaPlanta.loop = true; // Queremos que cicle mientras estés regando
        bocinaPlanta.playOnAwake = false; // Que no suene al iniciar el nivel


        // Aplicar castigo por tiempo ausente
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
            
        } else {
            render.color = Color.white;
            EstadoPlanta.vivas[indicePlanta] = true;
            gameObject.tag = "Untagged"; // planta viva = pala no hace nada
            SincronizarVisual();
        }
    }
void SincronizarVisual() {
        etapaActual = Mathf.FloorToInt(EstadoPlanta.progresos[indicePlanta] / 5f);
        etapaActual = Mathf.Clamp(etapaActual, 0, etapas.Length - 1);
        render.sprite = etapas[etapaActual];
    }

    void OnTriggerStay2D(Collider2D otro) {
        if (otro.CompareTag("Agua") && EstadoPlanta.vivas[indicePlanta]) {
            // --- ¡NUEVO: LÓGICA DE AUDIO! ---
            // Si el agua está tocando la planta y la bocina NO está sonando, le damos Play
            if (!bocinaPlanta.isPlaying) {
                bocinaPlanta.Play();
            }
            EstadoPlanta.armonias[indicePlanta] += 20f * Time.deltaTime;
            EstadoPlanta.armonias[indicePlanta] = Mathf.Clamp(EstadoPlanta.armonias[indicePlanta], 0, 100);

            

            if (EstadoPlanta.armonias[indicePlanta] > 60) {
                EstadoPlanta.progresos[indicePlanta] += Time.deltaTime;
            }
        }
    }

    // Lógica de la Pala
    void OnTriggerEnter2D(Collider2D otro) {
         // Solo actúa si la pala toca una planta MUERTA
         if (otro.CompareTag("Pala") && !EstadoPlanta.vivas[indicePlanta]) {
             EstadoPlanta.armonias[indicePlanta] = 100f;
             EstadoPlanta.progresos[indicePlanta] = 0f;
             EstadoPlanta.vivas[indicePlanta] = true;
             gameObject.tag = "Untagged";
 
             // Dispara el anuncio
             if (ControlAnuncio.instancia != null)
                 ControlAnuncio.instancia.MostrarAnuncio();
         }
     }
    
    // --- ¡NUEVO: APAGAR EL SONIDO! ---
    // Cuando el agua deja de tocar la planta, apagamos la bocina
    void OnTriggerExit2D(Collider2D otro) {
        if (otro.CompareTag("Agua")) {
            if (bocinaPlanta.isPlaying) {
                bocinaPlanta.Stop();
            }
        }
    }
    
 }