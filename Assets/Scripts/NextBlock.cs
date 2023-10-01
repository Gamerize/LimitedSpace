using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class NextBlock : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_Blocks;
    [SerializeField]
    GameObject[] m_BlockList = new GameObject[5];
    private int m_RandomInt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddNextBlock()
    {
        m_RandomInt = Random.Range(0,m_Blocks.Length+1);
        for (int i = 0; i < m_BlockList.Length; i++)
        {
            if (m_BlockList[i] == null)
            {
                m_BlockList[i] = m_Blocks[m_RandomInt];
            }
        }
    }

}
