using UnityEngine;

public class Collide_Door : Collide
{
    [SerializeField] private Interaction_Door m_door;

    public override void Trigger_Event()
    {
        m_door.Move_Door(false); // Ãâ±¸ ¹®ÀÌ Äç ´ÝÈû
    }
}
