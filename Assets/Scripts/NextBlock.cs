using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class NextBlock : MonoBehaviour
{
    [Header("Blocks")]
    [SerializeField] private GameObject[] m_Blocks;
    [SerializeField] private GameObject[] m_BlockList = new GameObject[5];

    [Header("Randomize")]
    private int m_RandomInt;

    [Header("UI")]
    [SerializeField] Image[] m_BlockSlots;
    [SerializeField] Sprite m_GridImage;

    public Vector3 m_SpawnPoint = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AddNextBlock());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            SpawnBlock();
            PushList();
            BlockDisplay();
        }
    }

    private IEnumerator AddNextBlock()
    {
        while (true)
        {
            m_RandomInt = Random.Range(0, m_Blocks.Length);
            Debug.Log(m_RandomInt);
            for (int i = 0; i < m_BlockList.Length; i++)
            {
                if (m_BlockList[i] == null)
                {
                    m_BlockList[i] = m_Blocks[m_RandomInt];
                    BlockDisplay();
                    break;
                }
            }
            yield return new WaitForSeconds(10);
        }
    }

    void BlockDisplay()
    {
        for (int i = 0; i < m_BlockList.Length; i++)
        {
            if (m_BlockList[i] != null)
            {
                SpriteRenderer UsedSprite = m_BlockList[i].GetComponent<SpriteRenderer>();
                m_BlockSlots[i].sprite = UsedSprite.sprite;
            }
            else
            {
                m_BlockSlots[i].sprite = m_GridImage;
            }
        }
    }

    void SpawnBlock()
    {
        if (m_BlockList.Length > 0)
        {
            Instantiate(m_BlockList[0], m_SpawnPoint, Quaternion.identity);
        }
    }

    void PushList()
    {
        for (int i = 0; i < m_BlockList.Length-1; i++)
        {
            m_BlockList[i] = m_BlockList[i + 1];
            m_BlockList[i + 1] = null;
        }
    }
}
