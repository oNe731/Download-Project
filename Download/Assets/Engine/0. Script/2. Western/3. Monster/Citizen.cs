using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : Person
{
    public Citizen() : base()
    {
    }

    public new void Initialize()
    {
        base.Initialize();
        m_meshRenderer.materials[0].SetTexture("_BaseMap", Resources.Load<Texture2D>("1. Graphic/3D/2. Western/Character/Texture/Person_02"));
        m_personType = PERSONTYPE.PT_CITIZEN;
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}
