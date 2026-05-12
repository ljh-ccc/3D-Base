using UnityEngine;
using System.Collections.Generic;

public class TextPool : MonoBehaviour
{
    [SerializeField] GameObject text;

    [SerializeField] int defaultCount;
    [SerializeField] int maxCount;

    public int DefaultCount => defaultCount;
    public int MaxCount => maxCount;

    public Queue<GameObject> textPool = new Queue<GameObject>();

    private void Awake()
    {
        if(text == null) { Debug.LogWarning("[TextPool] text is null"); return; }

        InitializePool();
    }

    void InitializePool()
    {
        for(int i = 0; i < defaultCount; i++)
        {
            GameObject obj = Instantiate(text, transform);

            obj.SetActive(false);

            textPool.Enqueue(obj);
        }
    }

    public void ReturnQueue(GameObject obj)
    {
        if (textPool.Count < maxCount)
        {
            obj.SetActive(false);

            textPool.Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("[TextPool] textPool is full, Destroy Object");

            Destroy(obj);
        }
    }

    public void AddInstantiate()
    {
        int addCount = Mathf.Max(1, maxCount / 10);

        for(int i = 0; i < addCount; i++)
        {
            if(textPool.Count < maxCount)
            {
                GameObject obj = Instantiate(text, transform);

                obj.SetActive(false);

                textPool.Enqueue(obj);
            }
            else
            {
                Debug.Log("[TextPool] textPool is full, failed to add new object");
                break;
            }
        }
    }


}
