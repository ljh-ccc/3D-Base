using UnityEngine;
using UnityEngine.UI;

public class AimColor : MonoBehaviour
{
    [SerializeField] TwoPointScan scan;

    [SerializeField] Color defaultColor;
    [SerializeField] Color lockOnColor;
    [SerializeField] Color blockedColor;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        if(scan != null) { scan.OnColor += ChangeColor; }    
    }

    private void OnDisable()
    {
        if(scan != null) { scan.OnColor -= ChangeColor; }
    }

    void ChangeColor(int num)
    {
        switch (num)
        {
            case 0:
                image.color = lockOnColor;
                break;
            case 1:
                image.color = defaultColor;
                break;
            case 2:
                image.color = blockedColor;
                break;
        }
    }

}

