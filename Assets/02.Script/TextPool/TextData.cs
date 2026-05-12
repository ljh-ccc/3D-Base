using TMPro;
using UnityEngine;

public class TextData : MonoBehaviour
{
    TextPool textPool;
    TMP_Text text;

    float timer = 0f;
    [SerializeField] float lifeTime;
    [SerializeField] float moveSpeed;
    bool isRelease;

    private void Awake()
    {
        if(text == null) { text = GetComponent<TMP_Text>(); }
    }

    private void OnEnable()
    {
        isRelease = false;
    }

    void OnDisable()
    {
        ReturnToPool();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > lifeTime)
        {
            Relaese();
        }

        Vector3 nextPos = transform.position + Vector3.up * moveSpeed * Time.deltaTime;
        transform.position = nextPos;
    }

    private void LateUpdate()
    {
        if(Camera.main == null) { return; }
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void Initialize(TextPool pool, Vector3 pos, string msg, Color color)
    {
        textPool = pool;
        transform.position = pos;
        text.text = msg;
        text.color = color;
    }

    void Relaese()
    {
        if(isRelease) { Debug.Log("[TextData] Already returned"); return;}

        isRelease = true;

        textPool.ReturnQueue(gameObject);
    }

    void ReturnToPool()
    {
        timer = 0f;
        text.text = "";
        text.color = Color.white;
    }
}
