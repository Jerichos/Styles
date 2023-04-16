using UnityEngine;

namespace Styles.Game
{
public class WriteMessage : MonoBehaviour, IInteractable
{
    [SerializeField] private string _message;
    
    public void Interact(MonoBehaviour actor)
    {
        Debug.Log(_message);    
    }
}
}