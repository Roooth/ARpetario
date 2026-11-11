using UnityEngine;

public class BotonVerEnAR : MonoBehaviour
{
    [SerializeField] private Trans_Shader transicion;
    [SerializeField] private string nombreEscena = "ARplanos";

    private ARSpeciesData especieActual;

    public void Configurar(ARSpeciesData especie)
    {
        especieActual = especie;
    }

    public void IrAAR()
    {
        DatosAR.Instancia.SeleccionarEspecie(especieActual);
        transicion.TransicionarA(nombreEscena);
    }
}