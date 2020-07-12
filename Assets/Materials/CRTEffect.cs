using UnityEngine;

[ExecuteInEditMode]
public class CRTEffect : MonoBehaviour
{
    public Material mat;
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (PlayerPrefs.GetInt("CRTEffect") == 1)
        {
            Graphics.Blit(src, dest, mat);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
