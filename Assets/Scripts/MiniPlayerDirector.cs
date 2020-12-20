using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniPlayerDirector : MonoBehaviour
{
    public static MiniPlayerDirector Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Camera cameraPrefab;
    public GameObject miniPlayerPrefab;
    public Shader baseShader;
    public Vector3 externalOffset = new Vector3(0f,1.2f,-14f);

    public Animator Create(float positionOffset, Card card)
    {
        Vector3 position = new Vector3(positionOffset, 0f, 0f);

        var newMiniPlayer = Instantiate(miniPlayerPrefab, transform, false);
        newMiniPlayer.transform.localPosition = position;

        var cameraPosition = new Vector3(position.x + externalOffset.x, position.y + externalOffset.y, externalOffset.z);

        var newCamera = Instantiate(cameraPrefab, transform, false);
        newCamera.transform.localPosition = cameraPosition;
        newCamera.targetTexture = RenderTexture.GetTemporary(256, 256, 0, RenderTextureFormat.ARGB32,
                                                                RenderTextureReadWrite.Default, 8,
                                                                RenderTextureMemoryless.Color);        

        card.SetTexture(newCamera.targetTexture);

        return newMiniPlayer.GetComponent<Animator>();
    }


}
