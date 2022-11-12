using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicInstance : MonoBehaviour
{

    public static MusicInstance instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
