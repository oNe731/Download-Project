using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1F_Weakness : Boss1F_Base // 내려가기, 맞기, 올라가기
{
    public Boss1F_Weakness(StateMachine<Monster> stateMachine) : base(stateMachine)
    {

    }

    public override void Enter_State()
    {
        m_owner.CumulativeDamage = 0;

        m_animator.speed = 1f;
        m_animator.SetBool("IsWeakDown", true);
    }

    public override void Update_State()
    {
        /*
         *  몸체에 일정 데미지를 가하면 *약점노출상태가 된다
     *약점노출상태: 하고 있던 동작을 모두 멈춘 채 입을 벌리고 축 늘어지며 약점부위인 입속의 눈을 노출시킨 상태.
	         ㄴ이때 눈을 제외한 부분을 공격하면 기본 데미지가 들어가고, 눈을 공격하면 데미지가 2.5배로 들어간다.
	         ㄴ약점 노출 상태는 5초간 유지한다.(끝나면 다시 원래대로 일어나서 입닫고 공격패턴 시작)

         */


        /*
         * . 약점 노출상태가 되어 하던 패턴이 끊겼다면, 랜덤한 순서에서부터 시작한다
    (ex. 패턴 순서가 ABCDE가 있다면..... 이 중 랜덤한 부분 선택후 거기서부터 다시 시작: C로 시작해서DE...다시 ABCDE순서로 가는 느낌)

         */
    }

    public override void Exit_State()
    {

    }
}
