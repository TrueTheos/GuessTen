using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private float oneImageWidth = 50f;

    private const float offset = 10f;
    private float speed;

    private void Start()
    {
        speed = moveSpeed;

        if (!this.gameObject.name.Contains("(Clone)"))
        {
            GameObject clone = Instantiate(this.gameObject);
            clone.transform.parent = this.transform.parent;
            clone.transform.position = new Vector3(oneImageWidth, 0, 0);

            clone = Instantiate(this.gameObject);
            clone.transform.parent = this.transform.parent;
            clone.transform.position = new Vector3(-oneImageWidth, 0, 0);
        }
    }

    private void Update()
    {
        if (this.transform.position.x <= -oneImageWidth - offset)
        {
            this.transform.position = new Vector3(oneImageWidth + (oneImageWidth - offset) - 
            (-this.transform.position.x - (oneImageWidth + offset)), 0, 0);
        }

        Vector3 v = this.transform.position;
        v.x -= speed * Time.deltaTime;
        this.transform.position = v;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (speed > 0)
                speed = 0;
            else
                speed = moveSpeed;
        }
    }
}
