using UnityEngine;
using UnityEngine.UI;

public class ProgressBarScript : MonoBehaviour
{
    private Image bar;
    private void Start()
    {
        bar = GetComponent<Image>();
    }
    public void SetValue(float value)
    {
        bar.fillAmount = value / 100;
    }
}
