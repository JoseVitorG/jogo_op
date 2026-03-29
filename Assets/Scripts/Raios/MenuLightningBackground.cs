
using UnityEngine;
using System.Collections.Generic;

public class MenuLightningBackground : MonoBehaviour
{
    [Header("Points")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Lightning Settings")]
    public int idleBoltCount = 3;
    public int activeBoltCount = 10;
    public int segments = 14;

    public float idleChaos = 0.1f;
    public float activeChaos = 0.4f;

    public float idleLifetime = 0.2f;
    public float activeLifetime = 0.06f;

    public float widthMin = 0.02f;
    public float widthMax = 0.05f;

    public Material lightningMaterial;

    private List<LineRenderer> bolts = new List<LineRenderer>();
    private float timer;
    private float currentLifetime;
    private float currentChaos;

    void Start()
    {
        SetSelected(false);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= currentLifetime)
        {
            timer = 0f;
            RegenerateBolts();
        }
    }

    public void SetSelected(bool selected)
    {
        ClearBolts();

        int count = selected ? activeBoltCount : idleBoltCount;
        currentChaos = selected ? activeChaos : idleChaos;
        currentLifetime = selected ? activeLifetime : idleLifetime;

        for (int i = 0; i < count; i++)
        {
            GameObject bolt = new GameObject("LightningBolt_" + i);
            bolt.transform.SetParent(transform);

            LineRenderer lr = bolt.AddComponent<LineRenderer>();
            lr.material = lightningMaterial;
            lr.positionCount = segments;
            lr.useWorldSpace = true;
            lr.numCapVertices = 2;
            lr.numCornerVertices = 2;
            lr.widthMultiplier = Random.Range(widthMin, widthMax);

            bolts.Add(lr);
        }

        RegenerateBolts();
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
                Vector3 basePos = Vector3.Lerp(startPoint.position, endPoint.position, t);
                Vector3 offset = Random.insideUnitSphere * currentChaos;
                lr.SetPosition(j, basePos + offset);
            }
        }
    }

    void ClearBolts()
    {
        for (int i = 0; i < bolts.Count; i++)
        {
            if (bolts[i] != null)
                Destroy(bolts[i].gameObject);
        }
        bolts.Clear();
    }
}
