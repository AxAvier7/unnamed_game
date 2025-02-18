using System;
using UnityEngine;

[Serializable]
public class Dialogue //una clase para poder añadir lineas de dialogo
{
    public string Name;
    [TextArea(3,10)]
    public string[] sentences;
}