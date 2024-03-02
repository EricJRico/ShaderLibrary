using UnityEngine;

public class AssignTexture : MonoBehaviour
{
    public ComputeShader shader;
    public int texResolution = 256;

    Renderer renderer;
    RenderTexture outputRenderTexture;
    int kernelHandle;

    // Start is called before the first frame update
    void Start()
    {
        outputRenderTexture = new RenderTexture(texResolution, texResolution, 0);
        outputRenderTexture.enableRandomWrite = true;
        outputRenderTexture.Create();

        renderer = GetComponent<Renderer>();
        renderer.enabled = true;

        InitShader();
    }

    private void InitShader()
    {
        kernelHandle = shader.FindKernel("CSMain");
        shader.SetTexture(kernelHandle, "Result", outputRenderTexture);
        renderer.sharedMaterial.SetTexture("_BaseMap", outputRenderTexture);

        DispatchShader(texResolution/16, texResolution/16);
    }

    private void DispatchShader(int x, int y)
    {
        // [numthreads(8,8,1)]
        shader.Dispatch(kernelHandle, x, y, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.U))
        {
            // Since the number of threads is [numthreads(8,8,1)] divide by 8 to fill
            // the full screen when dispatching the shader
            DispatchShader(texResolution/8, texResolution/8);
        }
    }
}
