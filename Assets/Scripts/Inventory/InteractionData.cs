using UnityEngine;


    public class InteractionData : MonoBehaviour
    {
        // watering
        public float maxWaterAmount;
        public float waterAmount;
        
        // planting
        public Seed seed;
        //public Field field;
        
        // harvesting
        public Crop crop;
        
        // fertilizing
        public float maxFertilizerAmount;
        public float fertilizerAmount;
        
        // plowing
        public bool isPlowed;
        
        // cos do cutting wood
        
        // cos do mining with pickaxe
        
        // cos do building
        
        // cos do destruction by hand
        
        // cos do destruction with tool
        
        // opening gui
        public string storageName;
        public Item[] itemsInStorage;
        public GameObject additionalGUI;

    
}
