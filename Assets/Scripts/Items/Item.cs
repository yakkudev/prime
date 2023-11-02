using UnityEngine;
using UnityEngine.EventSystems;

    public abstract class Item : InteractionData
    {
        [Header("Item Info")]
        public Sprite icon;
        public string itemName;
        public string description;
        public bool isEquipped;
        public int maxStackSize = 1;
        public int stackSize = 1;
        public IUsable Usable;

        [SerializeField] bool isBuilding;
        public int index;
        public Item[] listWhereItemIs;
        protected IRefreshInteractionTypes RefreshInteractionTypes;


        public InteractionType[] interactionTypes;
        private void Update()
        {
            if (!isEquipped) return;
            if (Input.GetKeyDown(KeyCode.Mouse0)) Interact();
        }

        private void Interact()
        {
        Debug.Log("budynek2");
        if(RefreshInteractionTypes != null) RefreshInteractionTypes.RefreshInteractions();


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;

        //Debug.Log("hit" + hit.transform.name + " " + EventSystem.current.IsPointerOverGameObject());
        Debug.Log("budynek3");
        if (!hit.transform.TryGetComponent(out Item hitItem) || EventSystem.current.IsPointerOverGameObject()) return;
            if (hitItem.isBuilding)
                Interaction(this, this, hitItem, hitItem);
                Debug.Log("budynek4");
    }
        public void Interaction(Item handItem, InteractionData handItemData, Item structure, InteractionData structureData)
        {
        if (structure.gameObject.CompareTag("building")) Debug.Log("budynek");
            structure.RefreshInteractionTypes.RefreshInteractions();
            for (int i = 0; i < handItem.interactionTypes.Length; i++)
            {
                for (int j = 0; j < structure.interactionTypes.Length; j++)
                {

                    if (structure.interactionTypes[i] == InteractionType.OpeningUI)
                    {
                        structure.Usable.Use(InteractionType.OpeningUI, handItemData);
                        return;
                    }

                    if (structure.interactionTypes[i] == InteractionType.BuildingWantToDoSomething)
                    {
                        structure.Usable.Use(InteractionType.BuildingWantToDoSomething, handItemData);
                    }

                    if (handItem.interactionTypes[i] != structure.interactionTypes[j]) continue;

                    handItem.Usable.Use(handItem.interactionTypes[i], structureData);
                }
            }
        }

        protected void ChangeInteraction(InteractionType typeOfInteraction, bool toogleOn)
        {
            for (int i = 0; i < interactionTypes.Length; i++)
            {
                if (interactionTypes[i] == typeOfInteraction)
                {
                    // interakcja jest juz na liscie

                    if (toogleOn) return;

                    interactionTypes[i] = InteractionType.None;
                }
            }

            if (toogleOn)
            {
                for (int i = 0; i < interactionTypes.Length; i++)
                {
                    if (interactionTypes[i] == InteractionType.None)
                    {

                        interactionTypes[i] = typeOfInteraction;
                        return;
                    }
                }

                InteractionType[] newInteractionTypes = new InteractionType[interactionTypes.Length + 1];
                for (int i = 0; i < interactionTypes.Length; i++)
                {
                    newInteractionTypes[i] = interactionTypes[i];
                }
                newInteractionTypes[interactionTypes.Length - 1] = typeOfInteraction;
                interactionTypes = newInteractionTypes;
            }
        }

        public GameObject CopyItemToOtherItem(Transform itemTo, GameObject gameObjectToCopy = null)
        {
            if (gameObjectToCopy == null) gameObjectToCopy = gameObject;
            return Instantiate(gameObjectToCopy, itemTo);
        }
        public void DeleteStack(int amountOfStacks = 1)
        {
            if (stackSize - amountOfStacks < 1)
            {
                InventoryManager.playerInventory.RemoveItemFromInventory(index, listWhereItemIs);
                Destroy(gameObject);
            }
            else
            {
                stackSize -= amountOfStacks;
            }
        }

    }
