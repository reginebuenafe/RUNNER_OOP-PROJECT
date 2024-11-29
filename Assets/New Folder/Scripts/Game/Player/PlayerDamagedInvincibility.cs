using UnityEngine;

public class PlayerDamagedInvincibility : MonoBehaviour
{
    [SerializeField]
    private float _invincibilityDuration;

    [SerializeField]
    private Color _flashColor;

    [SerializeField]
    private int _numberOfFlashes;
    private InvincibiltyController _invincibillityController;
    

    private void Awake(){
        _invincibillityController = GetComponent<InvincibiltyController>();
    }
    public void StartInvincibility(){
    _invincibillityController.StartInvincibility(_invincibilityDuration, _flashColor, _numberOfFlashes);    
    }
}
