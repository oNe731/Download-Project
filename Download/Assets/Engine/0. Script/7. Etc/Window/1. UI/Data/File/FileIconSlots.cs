using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileIconSlots : WindowData
{
    private int m_slotCount = 72;
    private FileIconSlot[][] m_files;

    public FileIconSlots()
    {
        m_files = new FileIconSlot[6][]; // 6¡Ÿ
        for (int i = 0; i < m_files.Length; ++i)
            m_files[i] = new FileIconSlot[12]; // 12ƒ≠

        int RowIndex = 0;
        int ColumnIndex = 0;
        for (int i = 0; i < m_slotCount; ++i)
        {
            m_files[RowIndex][ColumnIndex] = new FileIconSlot();
            ColumnIndex++;

            if (ColumnIndex >= 12)
            {
                RowIndex++;
                ColumnIndex = 0;
            }
        }
    }

    public override void Load_Scene()
    {
        int RowIndex = 0;
        int ColumnIndex = 0;
        for (int i = 0; i < m_slotCount; ++i)
        {
            m_files[RowIndex][ColumnIndex].Load_Scene();
            ColumnIndex++;

            if (ColumnIndex >= 12)
            {
                RowIndex++;
                ColumnIndex = 0;
            }
        }
    }

    public override void Update_Data()
    {
    }

    public override void Unload_Scene()
    {
    }

    public void Add_FileIcon(int rowIndex, int columnIndex, WindowManager.FILETYPE fileType, string fileName, Action action = null)
    {
        if (m_files[rowIndex][columnIndex].Empty == false)
            return;

        m_files[rowIndex][columnIndex].Add_FileIcon(fileType, fileName, action);
    }
}
