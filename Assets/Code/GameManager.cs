using UnityEngine;

public class GestorJuego : MonoBehaviour 
{
    public GameObject regadera; // Arrastra la regadera aquí en el Inspector

    public void AlternarRegadera() 
    {
        if (regadera != null) 
        {
            // Si está apagada la prende, si está prendida la apaga
            bool estadoActual = regadera.activeSelf;
            regadera.SetActive(!estadoActual);

            // Opcional: Que aparezca siempre en el centro o cerca del botón
            if (!estadoActual) {
                regadera.transform.position = new Vector3(0, 0, 0); 
            }
        }
    }
    
     public GameObject Pala;
     
     public void AlternarPala() 
        {
            if (Pala != null) 
            {
                // Si está apagada la prende, si está prendida la apaga
                bool estadoActual = Pala.activeSelf;
                Pala.SetActive(!estadoActual);
    
                // Opcional: Que aparezca siempre en el centro o cerca del botón
                if (!estadoActual) {
                    Pala.transform.position = new Vector3(0, 0, 0); 
                }
            }
        }
}

