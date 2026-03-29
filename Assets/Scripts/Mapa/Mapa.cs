using UnityEngine;

public class GridLines : MonoBehaviour
{
    public int largura = 10;
    public int altura = 10;
    public float tamanho = 1f;
    public int intervalo = 5;

    public Material lineMaterial;
    public float espessura = 0.05f;

    void Start()
    {
        for (int x = 0; x <= largura; x++)
        {
            if (x % intervalo == 0)
                CriarLinha(
                    new Vector3(x * tamanho, 0.01f, 0),
                    new Vector3(x * tamanho, 0.01f, altura * tamanho)
                );
        }

        for (int z = 0; z <= altura; z++)
        {
            if (z % intervalo == 0)
                CriarLinha(
                    new Vector3(0, 0.01f, z * tamanho),
                    new Vector3(largura * tamanho, 0.01f, z * tamanho)
                );
        }
    }

    void CriarLinha(Vector3 inicio, Vector3 fim)
    {
        GameObject linha = new GameObject("Linha");
        linha.transform.parent = transform;

        LineRenderer lr = linha.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, inicio);
        lr.SetPosition(1, fim);
        lr.startWidth = espessura;
        lr.endWidth = espessura;
        lr.material = lineMaterial;
        lr.useWorldSpace = true;
    }
}
