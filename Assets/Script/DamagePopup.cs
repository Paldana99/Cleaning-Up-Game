using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private Color textColor;
    public float speed, disappearTimer, disappearSpeed;

    private void Awake() {
      textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount) {
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;
    }

    public DamagePopup Create(DamagePopup prefab, Vector3 position, int damageAmount) {
      DamagePopup damagePopup = Instantiate(prefab, position, Quaternion.identity);
      damagePopup.Setup(damageAmount);
      return damagePopup;
    }

    private void Update() {
      transform.position += new Vector3(0, speed, 0)*Time.deltaTime;
      disappearTimer -= Time.deltaTime;
      if (disappearTimer < 0) {
        textColor.a -= disappearSpeed * Time.deltaTime;
        textMesh.color = textColor;
        if (textColor.a < 0)
          Destroy(gameObject);
      }
    }

}
