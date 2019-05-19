using UnityEngine;
using System.Collections;

public class BattleCell : MonoBehaviour
{
    /// <summary>
    /// state: 0normal, 1walkable, 2selecting, 3targeting
    /// </summary>
    private int state;

    private void Start()
    {
        ResetState();
    }

    public void SetState(){
        state = 1;
    }

    public void ResetState(){
        state = 0;
    }
}
