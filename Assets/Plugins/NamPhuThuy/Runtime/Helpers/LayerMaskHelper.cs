using UnityEngine;

namespace NamPhuThuy
{
    public class LayerMaskHelper
    {
        
        
        public static int OnlyIncluding(params int[] layers)
        {
            return MakeMask(layers);
        }

        public static int Everything()
        {
            return -1;
        }

        public static int Default()
        {
            return 1;
        }

        public static int Nothing()
        {
            return 0;
        }

        public static int EverythingBut(params int[] layers)
        {
            return ~MakeMask(layers);
        }

        public static bool ContainsLayer(LayerMask layerMask, int layer)
        {
            return (layerMask.value & 1 << layer) != 0;
        }

        static int MakeMask(params int[] layers)
        {
            int mask = 0;
            foreach (int item in layers)
            {
                mask |= 1 << item;
            }
            return mask;
        }
    }
}

//EXAMPLE
/*//Set a camera to only look at layer 7:
camera.cullingMask = LayerMaskHelper.OnlyIncluding( 7 );
camera.cullingMask = LayerMaskHelper.OnlyIncluding( LayerMask.NameToLayer("UserInterface") );

//Set a light to affect layers 4, 8 and 11:
light.cullingMask = LayerMaskHelper.OnlyIncluding( 4, 11, 8 ); 
light.cullingMask = LayerMaskHelper.OnlyIncluding( LayerMask.NameToLayer("Landscape"), LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies") );
 
light.cullingMask = LayerMaskHelper.EverythingBut( 5, 6 );
light.cullingMask = LayerMaskHelper.EverythingBut( LayerMask.NameToLayer("Landscape"), LayerMask.NameToLayer("Player") );
 

//Does this camera's culling mask allow it to look at the target object?:
public GameObject target;
	
void Update(){
    bool isTargetLayerInCullingMask = LayerMaskHelper.ContainsLayer( camera.cullingMask, target.layer );
    string textReplacement = isTargetLayerInCullingMask ? "allows" : "does not allow";
    Debug.Log( "This camera's culling mask " + textReplacement + " it to see the target object." );
}*/