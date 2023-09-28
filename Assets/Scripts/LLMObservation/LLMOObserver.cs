using System.Collections.Generic;
using Blip;
using DefaultNamespace;
using Kosmos2;
using LLMObservation;
using OpenAIGPT;
using UnityEngine;
using UnityEngine.Events;

public class LLMOObserver : MonoBehaviour
{
    [SerializeField] CameraImageCapture cameraImageCapture;
    [SerializeField] Kosmos2Connector kosmosConnector;
    [SerializeField] string kosmosPrompt;
    [SerializeField] string kosmosPostPrompt;
    [SerializeField] LLMOHierarchyExplorer hierarchyExplorer;
    [SerializeField] private float rayLength = 50f;
    private string sceneDescription;

    public List<Kosmos2GroundedRelation> groundedRelations = new();
    public UnityEvent<string> OnResponse;
    public UnityEvent<List<Kosmos2GroundedRelation>> OnGroundedResponse;

    public string conversationLog;

    [ContextMenu("Observe")]
    public void Observe()
    {
        Observe(kosmosPrompt);
    }
    
    public void Observe(string prompt)
    {
        string newPrompt = $"{prompt} {kosmosPostPrompt}";
        StartCoroutine(kosmosConnector.InterpretImage(cameraImageCapture.Capture(), newPrompt, KosmosResponse));
        conversationLog += $"<question>{prompt}</question> ";
        // sceneDescription = hierarchyExplorer.HierarchyDescription();
    }

    public void ObserveGrounded()
    {
        StartCoroutine(kosmosConnector.InterpretImage(cameraImageCapture.Capture(), "<grounding>As detailed as possible: An image of", KosmosGroundedResponse));
    }

    private void KosmosGroundedResponse(KosmosResponseData data)
    {
        Debug.Log($"Grounded Kosmos: {data.message}");
        LoggingController.Log($"[Grounded Kosmos]: {data.message}");
        foreach (var entity in data.entities)
        {
            var gObj = FindObjectsIn2DBoundingBox(entity.boundingBox);
            Debug.Log($"BoundingBox Look for: {entity.label}");
            if (gObj)
            {
                Debug.Log($"Object Found: {entity.label}");
                groundedRelations.Add(new Kosmos2GroundedRelation(){label = entity.label, subject = gObj, components = hierarchyExplorer.GetComponentDescription(gObj)});
            }
        }
        OnGroundedResponse.Invoke(groundedRelations);
    }

    private void KosmosResponse(KosmosResponseData data)
    {
        OnResponse.Invoke(data.message);
    }
    
    public GameObject FindObjectsIn2DBoundingBox(BoundingBox boundingBox)
    {
        var minPoint = new Vector2(boundingBox.x_min * cameraImageCapture.cameraToCapture.pixelWidth,
            (1.0f - boundingBox.y_min) * cameraImageCapture.cameraToCapture.pixelHeight);
        var maxPoint = new Vector2(boundingBox.x_max * cameraImageCapture.cameraToCapture.pixelWidth,
            (1.0f - boundingBox.y_max) * cameraImageCapture.cameraToCapture.pixelHeight);

        Ray ray = cameraImageCapture.cameraToCapture.ScreenPointToRay((minPoint + maxPoint) / 2f);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.blue, 20f);
        if (Physics.Raycast(ray, out hit))
        {
            GameObject foundObject = hit.collider.gameObject;
            return foundObject;
        }
        else
        {
            Debug.Log("No object found in the given bounding box.");
            return null;
        }
    }
    
}
