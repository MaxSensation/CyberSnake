using System.Collections.Generic;
using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;

public class ColorManager : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<List<Color>> colors = new NetworkVariable<List<Color>>(new NetworkVariableSettings{WritePermission = NetworkVariablePermission.ServerOnly}, new List<Color>());
    private PlaneController _planeController;

    public Color GetColor()
    {
        Color color;
        if (colors.Value.Count > 0)
        {
            color = colors.Value[0];
            colors.Value.RemoveAt(0);   
        }
        else
        {
            color = Color.blue;
        }
        return color;
    }
}