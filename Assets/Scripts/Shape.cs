using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
   private GameManager gameManager;
   private Vector3 startPos;

   public Color shapeColor;
   public Vector2[] shapeCells;

   private void Start()
   {
      startPos = GetComponent<RectTransform>().position;
      gameManager = FindObjectOfType<GameManager>();
   }

   void ChangeSize(Vector3 targetSize)
   {
      transform.localScale = targetSize;
         
   }
   public void OnBeginDrag(PointerEventData eventData)
   {
      ChangeSize(new Vector3(0.95f, 0.95f, 1));
   }
   
   public void OnEndDrag(PointerEventData eventData)
   {
      ChangeSize(new Vector3(0.6f, 0.6f, 1));
      GetComponent<RectTransform>().position = startPos;
   }

   public void OnDrag(PointerEventData eventData)
   {
      GetComponent<RectTransform>().position = Input.mousePosition + new Vector3(0, 75f, 0 );
   }

   
}
