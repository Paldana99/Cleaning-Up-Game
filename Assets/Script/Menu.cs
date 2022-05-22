using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
  public string principalFunction, functionCalled;
  public GameObject panel, confirmationBox;

  public void executeFunction(string functionName) {
    SendMessage(functionName);
  }
}
