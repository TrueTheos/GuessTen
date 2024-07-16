using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDestroy : MonoBehaviour
{
    [SerializeField]
    private float timeUntilDestroy = 3f;

    private float timer;

    private void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeUntilDestroy)
        {
            Destroy(this.gameObject);
        }
    }
}
