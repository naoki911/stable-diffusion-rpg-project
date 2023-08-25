using UnityEngine;


[System.Serializable]
public class FootstepData{
    public LayerMask layerMask;
    public AudioClip audioClip;
}
public class FootstepPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public FootstepData[] footstepDatas;

    public float rayOriginOffset = 0.1f; // Rayの発射元となる高さのオフセット

    public void PlayFootstep()
    {
        // キャラクターの足元より少し高い位置からRaycastを行い、地面の種類を判別する
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * rayOriginOffset;
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 1.0f + rayOriginOffset))
        {
            AudioClip clipToPlay = null;
            foreach(FootstepData footstepData in footstepDatas){
                if (IsObjectInLayerMask(hit.collider.gameObject, footstepData.layerMask))
                {
                    clipToPlay = footstepData.audioClip;
                    break;
                }
            }

            // 選んだ足音を再生する
            if (clipToPlay != null)
            {
                audioSource.PlayOneShot(clipToPlay);
            }
        }
    }

    private bool IsObjectInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) > 0;
    }
}

