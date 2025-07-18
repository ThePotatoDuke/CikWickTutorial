using UnityEngine;

public class CatStateController : MonoBehaviour
{
    [SerializeField] private CatState _currentCatstate = CatState.Idle;
    private void Start() {
        ChangeState(CatState.Walking);
    }
    public void ChangeState(CatState newState)
    {
        if (_currentCatstate == newState)
        {
            return;
        }
        _currentCatstate = newState;
    }
    public CatState GetCurrentState()
    {
        return _currentCatstate;
    }
}
