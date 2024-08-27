public class Jumpscare_Ball : Jumpscare
{
    public override void Active_Jumpscare()
    {
        m_isTrigger = true;

        /*
          [A]범위에 주인공이 들어가면[B]에서 조그만 공이 화살표 방향으로 굴러오기 시작한다.
          몇번 힘없이 튀기다가 마지막에 굴러와서 멈추는 느낌으로   
          움직일것 같습니다.(통......통....통...데구르르르...)*
          화살표는 마지막에 공이 멈추는 거리?움직임 까지 고려한 범위입니다.  
          멈춘 공은 그 자리에 그대로 있습니다.* 공 튀기는 소리 사운드 O
        */
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}
