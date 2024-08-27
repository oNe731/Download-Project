public class Jumpscare_Girl : Jumpscare
{
    public override void Active_Jumpscare()
    {
        m_isTrigger = true;

        /*
         [A]범위에 주인공이 들어가면[B]에 서있던 어린 여자아이가 화살표 방향으로 걸어간다.
         주인공이 [A]에 들어간 순간 바로 웃음소리가 나고
         동시에 여자아이는 0.3초 정도 서 있다가 움직이기 시작
         화살표가 끝나는 곳 쯤에서 여자아이 오브젝트 삭제시키면 될 것 같습니다.
         여자아이 웃음소리 사운드 O
        */
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}
