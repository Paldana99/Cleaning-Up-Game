using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationBox : MonoBehaviour
{
  public string functionName;
  public GameObject menu;
  public GameObject panel;
  public string principalFunction;


  public void Confirmation(string functionCalledName) {
    gameObject.SetActive(true);
    panel.SetActive(false);
    functionName = functionCalledName;
  }


  public void yesButton() {
    menu.GetComponent<Menu>().executeFunction(functionName);
  }


  public void noButton() {
    menu.GetComponent<Menu>().executeFunction(principalFunction);
  }


}
