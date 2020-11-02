using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class HalftoneController : MonoBehaviour
{
    const int visibilityDispatch = 32;

    [StructLayout(LayoutKind.Sequential)]
    struct halftonePoint
    {
        public const int stride =
        sizeof(float) * 3;

        public Vector2 screenPos;
        public float density;
    }

    #region ComputeShaders
    public ComputeShader pointsVisibilityCalculator;
    #endregion

    #region ComputeShadersKernels
    int kernelCyanVisCalCs;
    int kernelMagentaVisCalCs;
    int kernelYellowVisCalCs;
    int kernelBlackVisCalCs;
    #endregion

    #region ComputeBuffers
    ComputeBuffer visibleCyanPoints, visibleMagentaPoints, visibleYellowPoints, visibleBlackPoints;
    ComputeBuffer drawIndirectCyan, drawIndirectMagenta, drawIndirectYellow, drawIndirectBlack;
    #endregion

    #region UserProperties
    public bool postEnabled;
    public Shader particlesShader;
    public Texture2D particleTex;

    [Range(0.0f, 1.0f)]
    public float cyanEfficency, magentaEfficency, yellowEfficency, blackEfficency;

    [Range(3, 31)]
    public int rasterRadius = 3;

    [Range(-90.0f, 90.0f)]
    public float cyanAngle, magentaAngle, yellowAngle, blackAngle;

    #endregion

    #region localVariables
    private RenderTexture tempSource = null;
    private Material cyanMat, magentaMat, yellowMat, blackMat;
    Camera thisCamera;

    int resolutionX = 0;
    int resolutionY = 0;
    int diameter;
    int previousDiamater = 0;

    #endregion

    Matrix4x4 generate2dMatrix(float angle, Vector2 screenSize)
    {
        Matrix4x4 rotationMatrix = Matrix4x4.identity;
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        rotationMatrix.m00 = cos;
        rotationMatrix.m01 = -sin;
        rotationMatrix.m10 = sin;
        rotationMatrix.m11 = cos;

        Matrix4x4 scaleMatrix = Matrix4x4.identity;
        scaleMatrix.m00 = screenSize.x;
        scaleMatrix.m11 = screenSize.y;

        return rotationMatrix * scaleMatrix;
    }

    private void OnEnable()
    {
        thisCamera = GetComponent<Camera>();

        if (particlesShader == null || pointsVisibilityCalculator == null)
            return;

        cyanMat = new Material(particlesShader);
        magentaMat = new Material(particlesShader);
        yellowMat = new Material(particlesShader);
        blackMat = new Material(particlesShader);

        FindKernels();
        OnResolutionOrInputChanged();

    }
    void FindKernels()
    {
        kernelCyanVisCalCs = pointsVisibilityCalculator.FindKernel("kernelCyanVisCalCs");
        kernelMagentaVisCalCs = pointsVisibilityCalculator.FindKernel("kernelMagentaVisCalCs");
        kernelYellowVisCalCs = pointsVisibilityCalculator.FindKernel("kernelYellowVisCalCs");
        kernelBlackVisCalCs = pointsVisibilityCalculator.FindKernel("kernelBlackVisCalCs");
    }
    void OnResolutionOrInputChanged()
    {
        resolutionX = thisCamera.pixelWidth;
        resolutionY = thisCamera.pixelHeight;

        tempSource = new RenderTexture(resolutionX, resolutionY, 0, RenderTextureFormat.ARGBHalf);
        pointsVisibilityCalculator.SetTexture(kernelCyanVisCalCs, "source", tempSource);
        pointsVisibilityCalculator.SetTexture(kernelMagentaVisCalCs, "source", tempSource);
        pointsVisibilityCalculator.SetTexture(kernelYellowVisCalCs, "source", tempSource);
        pointsVisibilityCalculator.SetTexture(kernelBlackVisCalCs, "source", tempSource);

        int distanceBeetwenPoints = rasterRadius * 2;
        int gridResolutionX = Mathf.CeilToInt(((float)thisCamera.pixelWidth) / ((float)distanceBeetwenPoints));
        int gridResolutionY = Mathf.CeilToInt(((float)thisCamera.pixelHeight) / ((float)distanceBeetwenPoints));

        diameter = Mathf.CeilToInt(Mathf.Sqrt(gridResolutionX * gridResolutionX + gridResolutionY * gridResolutionY));
        float cyanAngleRadians = Mathf.Deg2Rad * cyanAngle;
        float magentaAngleRadians = Mathf.Deg2Rad * magentaAngle;
        float yellowAngleRadians = Mathf.Deg2Rad * yellowAngle;
        float blackAngleRadians = Mathf.Deg2Rad * blackAngle;

        pointsVisibilityCalculator.SetInt("screenWidth", resolutionX);
        pointsVisibilityCalculator.SetInt("screenHeight", resolutionY);
        pointsVisibilityCalculator.SetInt("diameter", diameter);
        pointsVisibilityCalculator.SetInt("distanceBeetwenPoints", distanceBeetwenPoints);
        pointsVisibilityCalculator.SetVector("CMYKAngle", new Vector4(cyanAngleRadians, magentaAngleRadians, yellowAngleRadians, blackAngleRadians));

        Vector2 particleSize = new Vector2((float)distanceBeetwenPoints / (float)thisCamera.pixelWidth, (float)distanceBeetwenPoints / (float)thisCamera.pixelHeight);

        Color cyanHalftoneColor = Color.Lerp(Color.white, Color.cyan, cyanEfficency);
        Color magentaHalftoneColor = Color.Lerp(Color.white, Color.magenta, magentaEfficency);
        Color yellowHalftoneColor = Color.Lerp(Color.white, Color.yellow, yellowEfficency);
        Color blackHalftoneColor = Color.Lerp(Color.white, Color.black, blackEfficency);

        cyanMat.SetColor("_Color", cyanHalftoneColor);
        cyanMat.mainTexture = particleTex;
        cyanMat.SetMatrix("_RotationScaleMatrix", generate2dMatrix(cyanAngleRadians, particleSize));

        magentaMat.SetColor("_Color", magentaHalftoneColor);
        magentaMat.mainTexture = particleTex;
        magentaMat.SetMatrix("_RotationScaleMatrix", generate2dMatrix(magentaAngleRadians, particleSize));

        yellowMat.SetColor("_Color", yellowHalftoneColor);
        yellowMat.mainTexture = particleTex;
        yellowMat.SetMatrix("_RotationScaleMatrix", generate2dMatrix(yellowAngleRadians, particleSize));

        blackMat.SetColor("_Color", blackHalftoneColor);
        blackMat.mainTexture = particleTex;
        blackMat.SetMatrix("_RotationScaleMatrix", generate2dMatrix(blackAngleRadians, particleSize));

        if (previousDiamater != diameter)
        {
            ResetBuffers();
            CreateBuffersAndSetData();
            BindBuffers();
            previousDiamater = diameter;
        }

    }
    private void Awake()
    {
        previousDiamater = 0;
    }
    private void OnValidate()
    {
        if (cyanMat && magentaMat && yellowMat && blackMat)
            OnResolutionOrInputChanged();
    }


    void CreateBuffersAndSetData()
    {
        int maxVisibleMemoryCount = diameter * diameter;
        visibleCyanPoints = new ComputeBuffer(maxVisibleMemoryCount, halftonePoint.stride, ComputeBufferType.Append);
        visibleMagentaPoints = new ComputeBuffer(maxVisibleMemoryCount, halftonePoint.stride, ComputeBufferType.Append);
        visibleYellowPoints = new ComputeBuffer(maxVisibleMemoryCount, halftonePoint.stride, ComputeBufferType.Append);
        visibleBlackPoints = new ComputeBuffer(maxVisibleMemoryCount, halftonePoint.stride, ComputeBufferType.Append);

        drawIndirectCyan = new ComputeBuffer(1, sizeof(uint) * 4, ComputeBufferType.IndirectArguments);
        drawIndirectMagenta = new ComputeBuffer(1, sizeof(uint) * 4, ComputeBufferType.IndirectArguments);
        drawIndirectYellow = new ComputeBuffer(1, sizeof(uint) * 4, ComputeBufferType.IndirectArguments);
        drawIndirectBlack = new ComputeBuffer(1, sizeof(uint) * 4, ComputeBufferType.IndirectArguments);

        uint[] startDrawParams = new uint[] { 1, 0, 0, 0 }; // vertexPerInstance, instancesCount, startVertex location, start istance location

        drawIndirectCyan.SetData(startDrawParams);
        drawIndirectMagenta.SetData(startDrawParams);
        drawIndirectYellow.SetData(startDrawParams);
        drawIndirectBlack.SetData(startDrawParams);

    }

    void BindBuffers()
    {
        pointsVisibilityCalculator.SetBuffer(kernelCyanVisCalCs, "visibleCyanPoints", visibleCyanPoints);
        pointsVisibilityCalculator.SetBuffer(kernelMagentaVisCalCs, "visibleMagentaPoints", visibleMagentaPoints);
        pointsVisibilityCalculator.SetBuffer(kernelYellowVisCalCs, "visibleYellowPoints", visibleYellowPoints);
        pointsVisibilityCalculator.SetBuffer(kernelBlackVisCalCs, "visibleBlackPoints", visibleBlackPoints);
        cyanMat.SetBuffer("_visiblePoints", visibleCyanPoints);
        magentaMat.SetBuffer("_visiblePoints", visibleMagentaPoints);
        yellowMat.SetBuffer("_visiblePoints", visibleYellowPoints);
        blackMat.SetBuffer("_visiblePoints", visibleBlackPoints);
    }

    void CaculatePointsVisibility()
    {
        if (!pointsVisibilityCalculator)
        {
            Debug.LogError("No compute shaders deteced");
            return;
        }
        visibleCyanPoints.SetCounterValue(0);
        visibleMagentaPoints.SetCounterValue(0);
        visibleYellowPoints.SetCounterValue(0);
        visibleBlackPoints.SetCounterValue(0);

        int dipstachNumbers = Mathf.CeilToInt(((float)(diameter)) / ((float)(visibilityDispatch)));

        pointsVisibilityCalculator.Dispatch(kernelCyanVisCalCs, dipstachNumbers, dipstachNumbers, 1);
        pointsVisibilityCalculator.Dispatch(kernelMagentaVisCalCs, dipstachNumbers, dipstachNumbers, 1);
        pointsVisibilityCalculator.Dispatch(kernelYellowVisCalCs, dipstachNumbers, dipstachNumbers, 1);
        pointsVisibilityCalculator.Dispatch(kernelBlackVisCalCs, dipstachNumbers, dipstachNumbers, 1);

        ComputeBuffer.CopyCount(visibleCyanPoints, drawIndirectCyan, sizeof(uint));
        ComputeBuffer.CopyCount(visibleMagentaPoints, drawIndirectMagenta, sizeof(uint));
        ComputeBuffer.CopyCount(visibleYellowPoints, drawIndirectYellow, sizeof(uint));
        ComputeBuffer.CopyCount(visibleBlackPoints, drawIndirectBlack, sizeof(uint));
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!postEnabled)
        {
            Graphics.Blit(source, destination);
            return;
        }
        if (resolutionX != thisCamera.pixelWidth || resolutionY != thisCamera.pixelHeight)
            OnResolutionOrInputChanged();

        Graphics.Blit(source, tempSource);
        CaculatePointsVisibility();
        Graphics.Blit(Texture2D.whiteTexture, destination);

        cyanMat.SetPass(0);
        Graphics.DrawProceduralIndirectNow(MeshTopology.Points, drawIndirectCyan);
        magentaMat.SetPass(0);
        Graphics.DrawProceduralIndirectNow(MeshTopology.Points, drawIndirectMagenta);
        yellowMat.SetPass(0);
        Graphics.DrawProceduralIndirectNow(MeshTopology.Points, drawIndirectYellow);
        blackMat.SetPass(0);
        Graphics.DrawProceduralIndirectNow(MeshTopology.Points, drawIndirectBlack);

    }
    void ResetBuffer(ref ComputeBuffer bufferToClear)
    {
        if (bufferToClear != null)
        {
            bufferToClear.Dispose();
        }
    }
    void ResetBuffers()
    {
        ResetBuffer(ref visibleCyanPoints);
        ResetBuffer(ref visibleMagentaPoints);
        ResetBuffer(ref visibleYellowPoints);
        ResetBuffer(ref visibleBlackPoints);
        ResetBuffer(ref drawIndirectCyan);
        ResetBuffer(ref drawIndirectMagenta);
        ResetBuffer(ref drawIndirectYellow);
        ResetBuffer(ref drawIndirectBlack);


    }

    private void FreeMemory()
    {
        ResetBuffers();
        DestroyImmediate(cyanMat);
        DestroyImmediate(magentaMat);
        DestroyImmediate(yellowMat);
        DestroyImmediate(blackMat);
        tempSource.Release();
        previousDiamater = 0;
    }

    private void OnDisable()
    {
        FreeMemory();
    }
    private void OnDestroy()
    {
        FreeMemory();
    }
}