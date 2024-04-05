using UnityEngine;

namespace OpenAIGPT
{
    public class GPTImageObserver:MonoBehaviour
    {
        [SerializeField] private CameraImageCapture imageCapture;
        [SerializeField] private GPTConverser converser;

        public void ObserveCameraImage()
        {
            converser.PromptImage(imageCapture.Capture());
        }
        
    }
}