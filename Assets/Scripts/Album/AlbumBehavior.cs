using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Album
{
    public class AlbumBehavior : MonoBehaviour
    {
        public static bool running = true;

        [Header("Album Config")]
        [SerializeField] float changePageTime = .5f;
        List<Transform> pages;
        int currentPage = 0;
        public Transform backRotation;
        public Transform frontRotation;

        int cardsByPage = 12;

        bool changingPage;

        public GameObject pagePrefab;
        public GameObject cardPrefab;
        public Transform pageContainer;

        public PlayerCards playerCards;

        Camera cam;
        bool open;
        [Header("Card Description Config")]
        public Image cardImage;
        public TextMeshProUGUI cardName;
        public TextMeshProUGUI cardDescription;
        public TextMeshProUGUI cardRarity;
        Card cardhover;
        Database.Card lastCard;

        public GraphicRaycaster raycaster;
        PointerEventData pointerEventData;
        public EventSystem eventSystem;

        Animator m_animator;

        private void Start()
        {
            m_animator = GetComponent<Animator>();
            cam = Camera.main;
            pages = new List<Transform>();
        }

        private void Update() {
            if(!running)
                return;
            
            if(open){

                if(Input.GetMouseButtonDown(1)){
                    CloseAlbum();
                }

                pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results);
                if(results.Count > 0){
                    cardhover = results[0].gameObject.transform.GetComponent<Card>();
                    if(cardhover != null){
                        if(cardhover != lastCard){
                            lastCard = cardhover.cardData;
                            ShowCardDescription();
                        }
                    }
                    else{
                        m_animator.SetBool("CardInfo", false);
                    }
                }
            }
            else{
                if(Input.GetMouseButtonDown(1)){
                    ShowAlbum();
                }
            }
        }

        private void ShowCardDescription(){
            m_animator.SetBool("CardInfo", false);
            m_animator.SetBool("CardInfo", true);
            cardImage.sprite = lastCard.front;
            cardName.SetText(lastCard.name);
            cardDescription.SetText(lastCard.description);
            cardRarity.SetText(lastCard.rarity.ToString());
        }

        public void ShowAlbum(){
            open = true;
            SystemsController.RunningAlbum(true);
            m_animator.SetTrigger("In");

            bool front = true;
            PageBehavior page = Instantiate(pagePrefab,pageContainer).GetComponent<PageBehavior>();
            pages.Add(page.transform);
            
            for (int cardListIndex = 0; cardListIndex < playerCards.List.Count; cardListIndex++)
            {
                // troca de página
                if(cardListIndex % cardsByPage == 0 && cardListIndex != 0){
                    if(front){
                        front  = !front;
                    }
                    else
                    {
                        page = Instantiate(pagePrefab,pageContainer).GetComponent<PageBehavior>();
                        pages.Add(page.transform);
                        page.transform.SetAsFirstSibling();
                        front  = !front;
                    }
                }

                Card card;

                if(front){
                    card = Instantiate(cardPrefab, page.front).GetComponent<Card>();
                }
                else{
                    card = Instantiate(cardPrefab, page.back).GetComponent<Card>();
                }
                card.Setup(playerCards.List[cardListIndex]);
            }
        }


        public void CloseAlbum(){
            m_animator.SetTrigger("Out");
            Transform t;
            while (pages.Count > 0)
            {
                t = pages[0];
                pages.RemoveAt(0);
                Destroy(t.gameObject);
            }
            open = false;
            SystemsController.RunningAlbum(open);
        }

        public void NextPage()
        {
            if (currentPage + 1 <= pages.Count && !changingPage)
            {
                StartCoroutine(ChangePage(currentPage++, false));
            }
        }

        public void PrevPage()
        {
            if (currentPage - 1 >= 0 && !changingPage)
            {
                StartCoroutine(ChangePage(--currentPage, true));
            }
        }

        IEnumerator ChangePage(int page, bool showFront)
        {
            changingPage = true;
            float timer = 0;

            float progress = 0;

            Quaternion desired = showFront ? frontRotation.localRotation : backRotation.localRotation;
            Quaternion start = pages[page].rotation;

            pages[page].SetAsLastSibling();

            PageBehavior pb = pages[page].GetComponent<PageBehavior>();

            do
            {
                progress = Mathf.Min(timer / changePageTime, 1);

                if (progress > 0.55)
                {
                    if (showFront)
                    {
                        pb.ShowFront(frontRotation.rotation);
                    }
                    else
                    {
                        pb.ShowBack(backRotation.rotation);
                    }
                }

                pages[page].localRotation = Quaternion.Slerp(start, desired, progress);

                timer += Time.deltaTime;

                yield return null;

            } while (timer < changePageTime);
            changingPage = false;
        }

    }
}

