using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SOManager : MonoBehaviour
{
    public static SOManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SOManager>();
            }

            return _instance;
        }
    }

    private static SOManager _instance;

    public SpritesSO PlantSprites;
}
