using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Criminal : Person
{
    public Criminal() : base()
    {
    }

    public new void Initialize()
    {
        base.Initialize();
        m_meshRenderer.materials[0].SetTexture("_BaseMap", Resources.Load<Texture2D>("1. Graphic/3D/2. Western/Character/Texture/Person_01"));
        m_personType = PERSONTYPE.PT_CRIMINAL;
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}
