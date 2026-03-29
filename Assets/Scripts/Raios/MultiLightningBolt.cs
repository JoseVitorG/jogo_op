
using UnityEngine;
using System.Collections.Generic;

public class MultiLightningBolt : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public int boltCount = 5;
    public int segments = 12;

    public float boltLifetime = 0.15f;
    public float chaos = 0.5f;
    public float widthMin = 0.03f;
    public float widthMax = 0.06f;

    public Material lightningMaterial;

    private List<LineRenderer> bolts = new List<LineRenderer>();
    private float timer;

    void Start()
    {
        for (int i = 0; i < boltCount; i++)
        {
            GameObject bolt = new GameObject("LightningBolt_" + i);
            bolt.transform.parent = transform;

            LineRenderer lr = bolt.AddComponent<LineRenderer>();
            lr.material = lightningMaterial;
            lr.positionCount = segments;
            lr.useWorldSpace = true;
            lr.numCapVertices = 2;
            lr.numCornerVertices = 2;
            lr.widthMultiplier = Random.Range(widthMin, widthMax);

            bolts.Add(lr);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= boltLifetime)
        {
            timer = 0f;
            RegenerateBolts();
        }
    }

    void RegenerateBolts()
    {
        for (int i = 0; i < bolts.Count; i++)
        {
            LineRenderer lr = bolts[i];
            lr.widthMultiplier = Random.Range(widthMin, widthMax);

            for (int j = 0; j < segments; j++)
            {
                float t = (float)j / (segments - 1);
                Vector3 basePos = Vector3.Lerp(pointA.position, pointB.position, t);

                Vector3 offset = Random.insideUnitSphere * chaos;
                lr.SetPosition(j, basePos + offset);
            }
        }
    }
}
