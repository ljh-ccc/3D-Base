using UnityEngine;

public class Box : MonoBehaviour, IInteractable
{
    [SerializeField] Color defaultColor;
    [SerializeField] Color openedColor;

    Material material;
    TextSpot spot;

    [SerializeField] bool isOpened;

    public string InteractMsg
        => isOpened ? "Already Opened" : "Open Box";
    

    private void Awake()
    {
        if(material == null) { material = GetComponent<MeshRenderer>().material; }
        material.color = defaultColor;
        spot = GetComponent<TextSpot>();
        isOpened = false;
    }

    public void Interact()
    {
        if (isOpened) 
        { 
            Debug.Log($"[Box] {name} is already opened");
            spot.GetPoolObject(InteractMsg);    
            return; 
        }

        spot.GetPoolObject(InteractMsg);

        isOpened = true;
        Debug.Log($"[Box] Open {name}");
        

        material.color = openedColor;
    }




}
