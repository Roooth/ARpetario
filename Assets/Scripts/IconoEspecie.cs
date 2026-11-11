
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconoEspecie : MonoBehaviour
{
    [SerializeField] private Image imagen;
    [SerializeField] private TMP_Text nombre;
    [SerializeField] private GameObject bloqueo;
    [SerializeField] private Button boton;


    private ARSpeciesData especie;
    private ControladorReptidex controlador;
    private bool desbloqueada;

    public void Configurar(
        ARSpeciesData nuevaEspecie,
        bool estaDesbloqueada,
        ControladorReptidex nuevoControlador)
    {
        especie = nuevaEspecie;
        desbloqueada = estaDesbloqueada;
        controlador = nuevoControlador;

        imagen.sprite = especie.imagen;
        nombre.text = desbloqueada ? especie.nombre : "???";
        bloqueo.SetActive(!desbloqueada);

        boton.onClick.RemoveAllListeners();
        boton.onClick.AddListener(SeleccionarEspecie);
    }

    private void SeleccionarEspecie()
    {
        if (!desbloqueada)
        {
            return;
        }

        controlador.MostrarInformacion(especie);
    }

}
