using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerLevel : MonoBehaviour
{
  [SerializeField]
  private Sprite[] liveSprites;

  [SerializeField]
  private Image livesImage;

  [SerializeField]
  private Button pauseButton;


  public void updateLives(int playerLives) {
    livesImage.sprite = liveSprites[playerLives];
  }

}
