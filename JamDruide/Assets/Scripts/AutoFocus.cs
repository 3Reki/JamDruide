using UnityEngine;
using UnityEngine.EventSystems;

public class AutoFocus : MonoBehaviour
{
    [SerializeField] private GameObject target;
    
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(target);
    }
}