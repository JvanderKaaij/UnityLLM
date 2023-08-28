using System.Collections.Generic;
using Blip;
using Kosmos2;
using LLMObservation;
using OpenAIGPT;
using UnityEngine;
using UnityEngine.Events;

public class LLMOObserver : MonoBehaviour
{
    [SerializeField] CameraImageCapture cameraImageCapture;
    [SerializeField] BlipConnector blipConnector;
    [SerializeField] Kosmos2Connector kosmosConnector;
    [SerializeField] string kosmosPrompt;
    [SerializeField] GPTConverser gptConverser;
    [SerializeField] LLMOHierarchyExplorer hierarchyExplorer;

    private string sceneDescription;

    public List<Kosmos2GroundedRelation> groundedRelations = new();
    public UnityEvent<string> OnResponse;
    public UnityEvent<List<Kosmos2GroundedRelation>> OnGroundedResponse;


    [ContextMenu("Observe")]
    public void Observe()
    {
        Observe(kosmosPrompt);
    }
    
    public void Observe(string prompt)
    {
        StartCoroutine(kosmosConnector.InterpretImage(cameraImageCapture.Capture(), prompt, KosmosResponse));
        // sceneDescription = hierarchyExplorer.HierarchyDescription();
    }

    public void ObserveGrounded()
    {
        StartCoroutine(kosmosConnector.InterpretImage(cameraImageCapture.Capture(), "<grounding>An image of", KosmosGroundedResponse));
    }

    private void KosmosGroundedResponse(KosmosResponseData data)
    {
        Debug.Log($"Grounded Kosmos: {data.message}");
        foreach (var entity in data.entities)
        {
            var gObj = FindObjectsIn2DBoundingBox(entity.boundingBox);
            if (gObj)
            {
                groundedRelations.Add(new Kosmos2GroundedRelation(){label = entity.label, subject = gObj, components = hierarchyExplorer.GetComponentDescription(gObj)});
            }
        }
        OnGroundedResponse.Invoke(groundedRelations);
    }

    private void KosmosResponse(KosmosResponseData data)
    {
        Debug.Log($"Kosmos: {data.message}");
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
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.blue, 20f);
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
