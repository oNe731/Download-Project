public class Collide_Ball : Collide
{
    public override void Trigger_Event()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
