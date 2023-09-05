using UnityEngine;

public class CameraImageCapture : MonoBehaviour
{
    public Camera cameraToCapture;
    public byte[] Capture()
    {
        // Create a new texture with the specified dimensions
        Texture2D texture = new Texture2D(cameraToCapture.pixelWidth, cameraToCapture.pixelHeight, TextureFormat.RGB24, false);

        // Set the target texture of the camera to the created texture
        cameraToCapture.targetTexture = RenderTexture.GetTemporary(cameraToCapture.pixelWidth, cameraToCapture.pixelHeight, 16);

        // Render the camera's view to the target texture
        cameraToCapture.Render();

        // Read the pixels from the target texture into the created texture
        RenderTexture.active = cameraToCapture.targetTexture;
        texture.ReadPixels(new Rect(0, 0, cameraToCapture.pixelWidth, cameraToCapture.pixelHeight), 0, 0);
        texture.Apply();

        // Get the byte array of the texture's pixels
        byte[] imageBytes = texture.EncodeToPNG(); // You can also use EncodeToJPG for JPEG format

        // Clean up and reset
        RenderTexture.active = null;
        cameraToCapture.targetTexture = null;
        RenderTexture.ReleaseTemporary(cameraToCapture.targetTexture);

        return imageBytes;
    }
}