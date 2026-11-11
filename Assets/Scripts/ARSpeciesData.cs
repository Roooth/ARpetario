using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevaEspecie", menuName = "ARpetario/Especie")]
public class ARSpeciesData : ScriptableObject
{
    [Header("Identificación")]
    public string id;

    [Header("Información")]
    public string nombre;
    public string nombreCientifico;
    [TextArea(3, 6)]
    public string descripcion;

    [Header("Visual")]
    public Sprite imagen;

    [Header("Modelo 3D")]
    public GameObject prefab;

}
