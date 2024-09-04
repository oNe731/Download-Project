using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Gun_Effect : MonoBehaviour
{
    private float m_activeTime = 0.1f;
    private float m_time = 0f;

    private MeshRenderer m_meshRenderer;
    private Texture2D[] m_textures;

    public void Initialize_Effect()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();

        m_textures = new Texture2D[3];
        m_textures[0] = GameManager.Ins.Resource.Load<Texture2D>("1. Graphic/2D/3. Horror/Effect/GunLight/Gun_Light_1");
        m_textures[1] = GameManager.Ins.Resource.Load<Texture2D>("1. Graphic/2D/3. Horror/Effect/GunLight/Gun_Light_2");
        m_textures[2] = GameManager.Ins.Resource.Load<Texture2D>("1. Graphic/2D/3. Horror/Effect/GunLight/Gun_Light_3");

        gameObject.SetActive(false);
    }

    public void Reset_Effect()
    {
        m_meshRenderer.materials[0].SetTexture("_BaseMap", m_textures[Random.Range(0, 3)]);

        m_time = 0;
        m_activeTime = Random.Range(0.05f, 0.015f);
        gameObject.SetActive(true);
    }

    private void Update()
    {
        m_time += Time.deltaTime;
        if(m_time >= m_activeTime)
            gameObject.SetActive(false);
    }
}
