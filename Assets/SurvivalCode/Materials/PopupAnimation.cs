using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;

    private TextMeshProUGUI tmp;
    private float time = 0;
    private Vector3 origin;

    private void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
    }
    private void Update()
    {
        tmp.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(5, 1 + heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }
}
