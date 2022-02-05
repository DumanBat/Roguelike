using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public SpriteRenderer hand;

    private int _handDirectionIndex = -1;

    public void SetActiveHand(int directionIndex)
    {
        if (_handDirectionIndex == directionIndex) return;
        _handDirectionIndex = directionIndex;

        DisableHand();

        switch (directionIndex)
        {
            case 1:
                hand.gameObject.SetActive(true);
                hand.flipX = true;
                break;
            case 3:
                hand.gameObject.SetActive(true);
                hand.flipX = false;
                break;
            default:
                break;
        }
    }

    public void DisableHand()
    {
        hand.gameObject.SetActive(false);
    }
}
