using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropManager : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    [Header("BESOIN DU CANVAS POUR LA TAILLE DE REFERENCE DE L'ECRAN")]
    [SerializeField]
    private Canvas canvas;
    [Header("BESOIN D'UN GO POUR DEFINIR L'APPROCHE DE LA ZONE DE DROP")]
    [SerializeField]
    private GameObject zone;
    [Header("BESOIN D'UN GO POUR ZONE DE DROP")]
    [SerializeField]
    private GameObject drop;
    [Header("DISTANCE MINIMUM POUR LA ZONE DE DROP")]
    [SerializeField]
    private float minDistance;

    private GameObject item; // ITEM DRAG AND DROPPER
    private float distanceSolo; // DISTANCE DE REFERENCE ENTRE L'ITEM ET SA BONNE ZONE DE DROP
    private Vector3 startPosition; // SAUVEGARDE DE LA POSITION INITIALE DE L'ITEM POUR SA REINITILISATION
    private List<GameObject> zoneList = new List<GameObject>(); // LIST DES ZONES POUR COMPARER LES APPROCHES
    private List<float> floatList = new List<float>(); // VALEUR DE DISTANCE DES ZONES POUR DEFINIR LES APPROCHES

    private void Awake()
    {
        item = gameObject;

        // stockage des différentes zones de drop
        foreach(GameObject zoneItem in GameObject.FindGameObjectsWithTag("Zone"))
        {
            zoneList.Add(zoneItem);
            floatList.Add(0);
        }
    }

    // debut du drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = item.transform.position;
    } 

    // fin du drag
    public void OnEndDrag(PointerEventData eventData)
    {
        // desactivation du composant image des zones de drop
        for (int i = 0; i < zoneList.Count; i++)
        {
            zoneList[i].GetComponent<Image>().enabled = false;
        }

        // calcule de la bonne distance entre l'item et la zone de drop
        if (distanceSolo < minDistance && drop.CompareTag(item.tag))
        {
            item.transform.position = drop.transform.position;
        }
        else
        {
            item.transform.position = startPosition;
        }
    }

    // pendant que l'objet est dragger
    public void OnDrag(PointerEventData eventData)
    {
        item.GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor;

        distanceSolo = Vector3.Distance(item.transform.position, zone.transform.position);

        // boucle de comparaison des différentes zones de drop
        for (int i = 0; i < zoneList.Count; i++)
        {
            floatList[i] = Vector3.Distance(item.transform.position, zoneList[i].transform.position);

            // si proche d'une zone, activver le composant image
            if (floatList[i] < minDistance)
            {
                zoneList[i].GetComponent<Image>().enabled = true;
            }
            else
            {
                zoneList[i].GetComponent<Image>().enabled = false;
            }
        }

       
    }
}

