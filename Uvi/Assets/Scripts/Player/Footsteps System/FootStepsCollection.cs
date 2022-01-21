using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Footsteps collection", menuName = "Create New Footsteps Collection")]
public class FootStepsCollection : ScriptableObject
{
    public AudioClip LeftFootStep;
    public AudioClip RightFootStep;
    public AudioClip JumpSound;
    public AudioClip LandSound;
}
