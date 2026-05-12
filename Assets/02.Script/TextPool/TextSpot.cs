using TMPro;
using UnityEngine;

public class TextSpot : MonoBehaviour
{
    [SerializeField] TextPool pool;

    [SerializeField] Color textColor;

    private void Start()
    {
        if(pool == null) { Debug.LogWarning("[TextSpot] pool is null"); return; }
    }

    public void GetPoolObject(string msg)
    {
        if (pool == null) { return; }

        if(pool.textPool.Count > 0)
        {
            GameObject text = pool.textPool.Dequeue();

            text.GetComponent<TextData>().Initialize(pool, transform.position + new Vector3(1f, 2f, 0f), msg, textColor);

            text.SetActive(true);
        }
    }
}
