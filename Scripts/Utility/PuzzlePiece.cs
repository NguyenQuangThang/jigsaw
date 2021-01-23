//----------------------------------------------------------------------------------------------------------------------------------------------------------
// Script contatins all necessary info about particular puzzle-piece
//----------------------------------------------------------------------------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PuzzlePiece 
{	
	public Transform transform;			// Link to transform
	public Vector3 startPosition;		// Initial position	
	public Quaternion startRotation;	// Initial rotation
	public Vector3 targetPosition;		// Target position for movement
	public Renderer renderer; 
	public SpriteRenderer spriteRenderer;           // Link to renderer
                                        // Link to renderer
    public Vector3 size;				// Size of piece
    public Vector3 oldSize;

    public Vector3 oldScale;

    public GameObject thisOb;

	// Important internal variables - please don't change them blindly
	bool useLocalSpace;
	float movementTime;
	Vector3 velocity = Vector3.zero;
    Material materialAssembled;  // Material when piece assembled in puzzle
    bool setOldSize = false;

	//===================================================================================================== 
	// Constructor
	public PuzzlePiece (Transform _transform, Material _materialAssembled)
	{
        thisOb = _transform.gameObject;
		transform = _transform;
		startPosition = _transform.localPosition;
		startRotation = _transform.localRotation;
	
		renderer = _transform.GetComponent<Renderer> ();
        spriteRenderer = _transform.GetComponent<SpriteRenderer>();

        materialAssembled = _materialAssembled;

		size = renderer.bounds.size;
        if (!setOldSize)
        {
            oldSize = renderer.bounds.size;
            setOldSize = true;
            oldScale = transform.localScale;
        }
        //Debug.Log("old size " + oldSize);
        //Debug.Log("setOldSize " + setOldSize);

        //spriteRenderer.drawMode = SpriteDrawMode.Tiled;
        //spriteRenderer.size = new Vector2(1, 1);
        //spriteRenderer.drawMode = SpriteDrawMode.Simple;
        //spriteRenderer.size = new Vector3(1, 1, 1);

    }

    //public void Active()
    //{
    //    Debug.Log("thisOb.name : " + thisOb.name);
    //    thisOb.SetActive(false);
    //}
    public PuzzlePiece(Vector3 _size)
    {
        oldSize = _size;
    }
	//----------------------------------------------------------------------------------------------------
	// Calculate  piece rendedrer center offset from the piece pivot
	public Vector2 GetPieceCenterOffset () 
	{
		Vector2 pieceCenterOffset = new Vector2 (renderer.bounds.center.x - transform.position.x, renderer.bounds.center.y - transform.position.y);
		return pieceCenterOffset;
	}

    //---------------------------------------------------------------------------------------------------- 
    // Process piece movement  
    public IEnumerator Move (Vector3 _targetPosition, bool _inLocalSpace, float _movementTime) 
	{
        // Initialize
        targetPosition = _targetPosition;
        movementTime = _movementTime;
        useLocalSpace = _inLocalSpace;


        // Use proper positions data according to used movement space (Local or World)  and  Smoothly move piece until it reaches targetPosition
        while (Vector3.Distance (useLocalSpace ? transform.localPosition : transform.position,  targetPosition) > 0.1f)
        {            
            if (useLocalSpace)
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref velocity, movementTime);
            else
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, movementTime);

            yield return null;
        }

        // Set final position and Assemble if needed
        if (useLocalSpace)
            transform.localPosition = targetPosition;
        else
            transform.position = targetPosition;

        if (targetPosition == startPosition)
            Assemble();
    }
    
    //----------------------------------------------------------------------------------------------------   
    // Set to assembled state
    public void Assemble ()
	{
		if (transform.childCount > 0) 
			transform.GetChild(0).gameObject.SetActive(false);

		renderer.material = materialAssembled;
	}

	//----------------------------------------------------------------------------------------------------	
}