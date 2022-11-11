using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftsList : MonoBehaviour
{
    public List<Recipe> recipes;
    public enum Resources
    {
        None, Mistletoe, Hydromel, Mushroom, Salt
    }

    [Serializable]
    public class Recipe
    {
        public Resources[] ingredients = new Resources[2];
        public ScriptableObject output;
        
    }
}
