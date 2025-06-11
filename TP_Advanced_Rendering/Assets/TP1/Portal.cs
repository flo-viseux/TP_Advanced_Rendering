using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    public MeshRenderer screen;
    Camera playerCam;
    Camera portalCam;
    RenderTexture viewTexture;

    private void Awake()
    {
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        portalCam = GetComponentInChildren<Camera>();
        portalCam.enabled = false;
    }

    void CreateViewTexture()
    {
        Debug.Log("CreateViewTexture");
        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if (viewTexture != null)
                viewTexture.Release();
            viewTexture = new RenderTexture(Screen.width, Screen.height, 24);

            portalCam.targetTexture = viewTexture;
            linkedPortal.screen.material.SetTexture("_MainTex", viewTexture);
        }
    }

    private void LateUpdate()
    {
        Render();
    }

    static bool VisibleFromCamera(Renderer renderer, Camera camera)
    {
        Plane[] frustrumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(frustrumPlanes, renderer.bounds);
    }

    public void Render()
    {
        if(!VisibleFromCamera(linkedPortal.screen, playerCam))
        {
            var testTexture = new Texture2D(1, 1);
            testTexture.SetPixel(0, 0, Color.red);
            testTexture.Apply();
            linkedPortal.screen.material.SetTexture("_MainTex", testTexture);
            return;
        }

        linkedPortal.screen.material.SetTexture("_MainTex", viewTexture);
        screen.enabled = false;
        CreateViewTexture();

        var m = this.transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        portalCam.Render();

        screen.enabled = true;
    }
}
