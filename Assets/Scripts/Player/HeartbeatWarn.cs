using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WarningStruct
{
    public AudioClip HeartbeatSound;
    public float WarningRange;
}

public class HeartbeatWarn : MonoBehaviour
{
    public WarningStruct[] warnings;

    int curIndex;
    AudioSource audioSource;
	public GameObject creature;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
    {
        curIndex = -1;

		float dist = Vector3.Distance(transform.position, creature.transform.position);

		foreach (var warn in warnings)
        {
            if (warn.WarningRange > dist)
                curIndex++;
		}

		if (audioSource.isPlaying || curIndex == -1)
			return;

		audioSource.clip = warnings[curIndex].HeartbeatSound;
		audioSource.Play(0);
	}
}
