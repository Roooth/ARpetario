using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReptidexInformacion : MonoBehaviour
{
    [Header("Elementos de información")]
    [SerializeField] private TMP_Text nombre;
    [SerializeField] private TMP_Text nombreCientifico;
    [SerializeField] private TMP_Text descripcion;
    [SerializeField] private Image imagen;

    [SerializeField] private BotonVerEnAR botonVerEnAR;
    public void MostrarEspecie(ARSpeciesData especie)
    {
        nombre.text = especie.nombre;
        nombreCientifico.text = especie.nombreCientifico;
        descripcion.text = especie.descripcion;
        imagen.sprite = especie.imagen;

        botonVerEnAR.Configurar(especie);
    }
}