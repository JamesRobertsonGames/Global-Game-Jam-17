using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

    public MeshCollider r;
    public SkinnedMeshRenderer s;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        StartCoroutine(BakeMesh());
	}

    private bool baking = false;
    IEnumerator BakeMesh()
    {
        print(1);
        if (!baking)
        {
            print(11);
            baking = true;

            Mesh temp = new Mesh();
            s.BakeMesh(temp);
            r.sharedMesh = temp;

            baking = false;
        }

        yield return null;
    }
}
