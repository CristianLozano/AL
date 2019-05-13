using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LSystemsD : MonoBehaviour {
    
    private List<int> numberSegments = new List<int>();
    
    public float segBottomRadius = .55f;
	public float segTopRadius = .15f;
	
	public float segLength = 1.0f;
    
    private List<float> curBotRadius = new List<float>();
    private List<float> curTopRadius = new List<float>();
    
    private List<Vector3> gvertices = new List<Vector3>();
    private List<Vector3> gnormales = new List<Vector3>();
    private List<Vector2> guvs = new List<Vector2>();
    private List<int> gtriangles = new List<int>();
    
    private List<int> topVertices = new List<int>();
    
    private List<int> minVertex = new List<int>();
    private List<int> maxVertex = new List<int>();
    
    private List<int> minTriangle = new List<int>();
    private List<int> maxTriangle = new List<int>();
    
    private int currentSegmentId = 0;
    private int currentSegmentOffset = 0;
    
    private int currentBranchId = 0;
    
    private int verticesOffset = 0;
    private int trianglesOffset = 0;
    
    private List<float> angle = new List<float>();
    private float iniAngle = 25.0f;
    
    
    private List<Vector3> segmentPos = new List<Vector3>();
    private List<Quaternion> segmentRot = new List<Quaternion>();
    private List<Vector3> segmentRotV = new List<Vector3>();
    
    private List<Vector3> segmentLocRotVect = new List<Vector3>();
    
    private int nBranchesToAdd = 0;
    
    private int nbSides = 18;
    
    private List<int> inhSegId = new List<int>();
    private List<int> inhBranchId = new List<int>();
    
    private List<int> branchingOrder = new List<int>();
    
    private List<Vector3> iniPos3 = new List<Vector3>();
    
    
    private Vector3 iniPos;
    private Vector3 iniPos2;
    
    
	// Use this for initialization
	void Start () {
		SetupCone();
		CreateMesh();
		
		Vector3 aa = RotVector(20f,
		          new Vector3(0f,1f,0f),
		          new Vector3(0f,0f,1f));
	    print(aa);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	
	
	
	
	
	void SetupCone(){
	    
	    numberSegments.Add(35);
	    

		
		
		iniAngle = 0.01f/segLength;
		
		angle.Add(0.0f);
		
		curBotRadius.Add(0f);
		curTopRadius.Add(0f);
		
		inhSegId.Add(currentSegmentId);
		inhBranchId.Add(0);
		
		branchingOrder.Add(0);
		
		iniPos = new Vector3(0f,segLength,0f);
		iniPos2 = new Vector3(0f,segLength,0f);
		
		iniPos3.Add(iniPos);

		Vector3 rotVect = new Vector3(0f,0f,1f);

		
		segmentPos.Add(new Vector3(0f,0f,0f));
		segmentLocRotVect.Add(new Vector3(0f,1f,0f));
		
		segmentRot.Add(Quaternion.AngleAxis( iniAngle, rotVect));
		segmentRotV.Add(new Vector3(0f,0f,0f));
    
		
		curBotRadius[currentBranchId] = segBottomRadius;
		curTopRadius[currentBranchId] = segBottomRadius-(segBottomRadius-segTopRadius)/numberSegments[currentBranchId];
		
		


		
		DrawCone();
		float branchPosibility = 1.0f;
		
		rotVect = Vector3.Cross
	    	(
	    		(segmentLocRotVect[currentBranchId]), 
	    		new Vector3(Random.Range(-1,1f),Random.Range(-1,1f),Random.Range(-1,1f))
	    													
	    	).normalized;
	    
		int branchIsLocked = 0;
		for(int j=1;j<numberSegments[currentBranchId];j++){
		    if(branchIsLocked==0){
		    
				angle[currentBranchId] = angle[currentBranchId]+iniAngle;
				segmentRot[currentBranchId] = Quaternion.AngleAxis( angle[currentBranchId], rotVect);
				
				segmentRotV[currentBranchId] = RotVector(iniAngle, segmentRotV[currentBranchId], rotVect);
				
			
				segmentPos[currentBranchId] = segmentPos[currentBranchId]+(segmentRot[currentBranchId]*iniPos);
				currentSegmentOffset = 0;
			
				curTopRadius[currentBranchId] = segBottomRadius-(segBottomRadius-segTopRadius)*(j+1)/numberSegments[currentBranchId];
				DrawCone();
		
		
				for(int i=minVertex[currentSegmentId-1];i<maxVertex[currentSegmentId-1];i++){
					if(topVertices[i]==1){
						gvertices[i]=gvertices[i-currentSegmentOffset-1];
					}
				}
			
				branchPosibility = branchPosibility-0.1f;
			//	if((j>3)&&(Random.Range(0f,1f)>branchPosibility)){
					AddBranch();
					numberSegments.Add(numberSegments[currentBranchId]-j);
					branchPosibility = 1.0f;
					branchIsLocked = 1;
				
			//	}
			}
			
            
		}
		branchIsLocked = 0;
		
		while(nBranchesToAdd>0){
			currentBranchId++;
			
	/////////////////////////////////////////////////////////////////////////		
	//////////rotVect should update rotation vector that new branch /////////
	///////// would start rotating perpendicularly to old branch     ////////		
	/////////////////////////////////////////////////////////////////////////		
	/////////////////////////////////////////////////////////////////////////		

	    
	        Vector3 randVect = new Vector3(Random.Range(-1,1f),Random.Range(-1,1f),Random.Range(-1,1f));
			rotVect = Vector3.Cross
	    	(
	    		segmentLocRotVect[currentBranchId], 
	    		randVect
	    		
	    													
	    	).normalized;
	    	
	    	
	/////////////////////////////////////////////////////////////		
	/////////////////////////////////////////////////////////////		
	/////////////////////////////////////////////////////////////	
	        angle[currentBranchId] = 5f;
			iniAngle = 5.0f/segLength*((iniAngle+0.001f)/Mathf.Abs((iniAngle+0.001f)));
			
	        if(inhBranchId[currentBranchId]>0){
				segmentRot[currentBranchId] = (Quaternion.AngleAxis( 0.5f*iniAngle, rotVect));
				segmentRotV[currentBranchId] = RotVector(iniAngle, segmentRotV[currentBranchId], rotVect);
			}
			else{

			}
			
			segBottomRadius = curTopRadius[currentBranchId];

		
			DrawCone();
			for(int i=minVertex[currentSegmentId-1];i<maxVertex[currentSegmentId-1];i++){
					if(topVertices[i]==1){
						gvertices[i]=gvertices[i-(minVertex[currentSegmentId-1]-minVertex[inhSegId[currentBranchId]])];
					}
			}
		//      iniPos2 = segLength*(segmentPos[currentBranchId]).normalized;
			iniPos2 = iniPos3[currentBranchId];
			
		
		    branchPosibility = 1.0f;
			for(int j=1;j<numberSegments[currentBranchId];j++){
		        if(branchIsLocked==0){
		   //         iniPos2 = segLength*(segmentPos[currentBranchId]).normalized;


		/*						rotVect = Vector3.Cross
	    	(
	    		segmentLocRotVect[currentBranchId], 
	    		randVect
	    		
	    													
	    	).normalized;*/
	    	        angle[currentBranchId] = angle[currentBranchId]+iniAngle;
					segmentRot[currentBranchId] = segmentRot[currentBranchId]*Quaternion.AngleAxis( angle[currentBranchId], rotVect );
					
			        segmentRotV[currentBranchId] = RotVector(iniAngle, segmentRotV[currentBranchId], rotVect);
			        
			        iniPos2=RotVector(iniAngle, iniPos2, rotVect);
				//	segmentPos[currentBranchId] = segmentPos[currentBranchId]+(segmentRot[currentBranchId]*iniPos);
					segmentPos[currentBranchId] = segmentPos[currentBranchId]+RotVector(iniAngle, iniPos2, rotVect);
	
					
					
					currentSegmentOffset = 0;
			
					curTopRadius[currentBranchId] = segBottomRadius-(segBottomRadius-segTopRadius)*(j+1)/numberSegments[currentBranchId];
		
									branchPosibility = branchPosibility-0.1f;
					if((j>2)&&(Random.Range(0f,1f)>branchPosibility)){
						AddBranch();
						numberSegments.Add(numberSegments[currentBranchId]-j);
						branchPosibility = 1.0f;
					
						AddBranch();
						numberSegments.Add(numberSegments[currentBranchId]-j);
						branchPosibility = 1.0f;
					
						branchIsLocked = 0;
					}
			
					DrawCone();
		
		
					for(int i=minVertex[currentSegmentId-1];i<maxVertex[currentSegmentId-1];i++){
						if(topVertices[i]==1){
							gvertices[i]=gvertices[i-currentSegmentOffset-1];
						}
					}
				}

			
			}
			branchIsLocked = 0;
			nBranchesToAdd--;
		}
		

		

		
		
		
		
	}
	
	Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c) {
        Vector3 side1 = b - a;
        Vector3 side2 = c - a;
        return Vector3.Cross(side1, side2).normalized;
    }
	
	
	
	Vector3 RotVector(float rotAngle, Vector3 original, Vector3 direction){
	    
	    
	  //  Vector3 rotatedVector = Quaternion.AngleAxis( rotAngle, direction)*original;
	    
	    Vector3 cross1 = Vector3.Cross(original,direction);
	    Vector3 cross2 = Vector3.Cross(original,cross1);
	    
	    Vector3 rotatedVector = Quaternion.AngleAxis( rotAngle, cross2)*original;
	    
	
		return rotatedVector;
	
	}
	
	
	
	
	
	void AddBranch(){
	    inhSegId.Add(currentSegmentId);
	    inhBranchId.Add(currentBranchId);
	    
		curBotRadius.Add(curBotRadius[currentBranchId]);
		curTopRadius.Add(curTopRadius[currentBranchId]);
		
		segmentPos.Add(segmentPos[currentBranchId]);
		segmentRot.Add(segmentRot[currentBranchId]);
		
		segmentRotV.Add(segmentRotV[currentBranchId]);
		
		segmentLocRotVect.Add(segmentLocRotVect[currentBranchId]);
		
		if(currentBranchId==2){
//			Debug.Log("i"+currentBranchId+""+segmentLocRotVect[currentBranchId]);
		}

		angle.Add(0f);
		nBranchesToAdd++;
		
		branchingOrder.Add(branchingOrder[currentBranchId]+1);
		
		iniPos3.Add(iniPos2);
		
		
		
	}
	
	void AddBranch2(){
	    inhSegId.Add(currentSegmentId);
	    inhBranchId.Add(currentBranchId);
	    
		curBotRadius.Add(curBotRadius[currentBranchId]);
		curTopRadius.Add(curTopRadius[currentBranchId]);
		
		segmentPos.Add(segmentPos[currentBranchId]);
		segmentRot.Add(segmentRot[currentBranchId]);
		
	
		
		segmentLocRotVect.Add(segmentLocRotVect[currentBranchId]);
		

		angle.Add(0f);
		nBranchesToAdd++;
		
		branchingOrder.Add(branchingOrder[currentBranchId]+1);
		
		
		
	}
	
	
	
	
	
	void DrawCone(){
	

 
	int useBottomCap = 0;
	int useTopCap = 0; 
 
	float height = segLength;
	float bottomRadius = curBotRadius[currentBranchId];
	float topRadius = curTopRadius[currentBranchId];


 
	int nbVerticesCap = nbSides + 1;
	
	
/////////////////////////////////////////////////////////////////////////////////////////////////////////////	
/////////////////////////////////////////////     Vertices     //////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
//	#region Vertices
 


	int NN = nbVerticesCap + nbVerticesCap + nbSides * 2 + 2;
	
	
	int[] vUsed = new int[NN];
	int[] isTopVertice = new int[NN];
	
	int numUsed = 0;
	
	for(int ii=0;ii<NN;ii++){
		vUsed[ii] = 0;
		isTopVertice[ii] = 0;
	}

 
	// bottom + top + sides
	Vector3[] vertices = new Vector3[nbVerticesCap + nbVerticesCap + nbSides * 2 + 2];
	int vert = 0;
	float _2pi = Mathf.PI * 2f;
 
 
	// Bottom cap
	if(useBottomCap == 1){
		vert = 0;
		vUsed[vert] = 1;
		vertices[vert++] = new Vector3(0f, 0f, 0f);
		numUsed++;
		while( vert <= nbSides )
		{
			float rad = (float)vert / nbSides * _2pi;
			vertices[vert] = new Vector3(Mathf.Cos(rad) * bottomRadius, 0f, Mathf.Sin(rad) * bottomRadius);
			vUsed[vert] = 1;
			
			numUsed++;
			vert++;
		}
	}
 
	// Top cap
	if(useTopCap == 1){
		vert = nbSides+1;
		vUsed[vert] = 1;
		vertices[vert++] = new Vector3(0f, height, 0f);
		numUsed++;
		while (vert <= nbSides * 2 + 1)
		{
			float rad = (float)(vert - nbSides - 1)  / nbSides * _2pi;
			vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
			vUsed[vert] = 1;
			numUsed++;
			vert++;
		}
	}


 
	vert = nbSides * 2 + 2; 
	// Sides
	int v = 0;
	while (vert <= vertices.Length - 4 )
	{
		float rad = (float)v / nbSides * _2pi;
		vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
		vertices[vert + 1] = new Vector3(Mathf.Cos(rad) * bottomRadius, 0, Mathf.Sin(rad) * bottomRadius);
		
		isTopVertice[vert + 1] = 1;
		
		vUsed[vert] = 1;
		vUsed[vert+1] = 1;
		
		vert+=2;
		numUsed+=2;
		v++;
	}
	vertices[vert] = vertices[ nbSides * 2 + 2 ];
	vertices[vert + 1] = vertices[nbSides * 2 + 3 ];
	
	isTopVertice[vert + 1] = 1;
	
	vUsed[vert] = 1;
	vUsed[vert+1] = 1;
	
    Vector3[] verticesA = new Vector3[numUsed+2];
    
    int jj = 0;
	for(int ii=0;ii<NN;ii++){
	    if(vUsed[ii] == 1){
			verticesA[jj] = vertices[ii];
		//	vertices[ii] = segmentRot[currentBranchId]*vertices[ii];
			vertices[ii] = segmentRotV[currentBranchId]+vertices[ii];
			gvertices.Add(vertices[ii]+segmentPos[currentBranchId]);
			
			jj++;
		}
	}
//	segmentLocRotVect[currentBranchId] = (segmentRot[currentBranchId]*(new Vector3(0f, 1f, 0f) - new Vector3(0f, 0f, 0f))).normalized;
	segmentLocRotVect[currentBranchId] = (segmentRotV[currentBranchId]+(new Vector3(0f, 1f, 0f) - new Vector3(0f, 0f, 0f))).normalized;
//	print(segmentRot[currentBranchId]*(new Vector3(0f, 1f, 0f) - new Vector3(0f, 0f, 0f)));
	jj = 0;
	for(int ii=0;ii<NN;ii++){
	    if(vUsed[ii] == 1){
			
			if(isTopVertice[ii]==1){
				topVertices.Add(1);
			}
			else{
				topVertices.Add(0);
			}
			
			jj++;
		}
	}


//	#endregion
 
 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////    Normales    /////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////// 
 
 
 
//	#region Normales
 
	// bottom + top + sides

	Vector3[] normalesA = new Vector3[verticesA.Length];
	Vector3[] normales = new Vector3[vertices.Length];
	
	
    numUsed=0;
	vert = 0;
    
    for(int ii=0;ii<NN;ii++){
		vUsed[ii] = 0;
	}
 
	// Bottom cap
	if(useBottomCap == 1){
		vert = 0;
		while( vert  <= nbSides )
		{   
			normales[vert] = Vector3.down;
			vUsed[vert] = 1;
			numUsed++;
			vert++;
		}
	}
 
	// Top cap
	if(useTopCap == 1){
		vert = nbSides+1;
		while( vert <= nbSides * 2 + 1 )
		{
		    
			normales[vert] = Vector3.up;
			vUsed[vert] = 1;
			numUsed++;
			vert++;
		}
	}

 
  
	// Sides

	vert = nbSides * 2 + 2;
	v = 0;
	while (vert <= vertices.Length - 4 )
	{			
		float rad = (float)v / nbSides * _2pi;
		float cos = Mathf.Cos(rad);
		float sin = Mathf.Sin(rad);
 
		normales[vert] = new Vector3(cos, 0f, sin);
		normales[vert+1] = normales[vert];
		
		vUsed[vert] = 1;
		vUsed[vert+1] = 1;
 
        numUsed+=2;
		vert+=2;
		v++;
	}
	normales[vert] = normales[ nbSides * 2 + 2 ];
	normales[vert + 1] = normales[nbSides * 2 + 3 ];
	vUsed[vert] = 1;
	vUsed[vert+1] = 1;
    

    
    jj=0;
	for(int ii=0;ii<NN;ii++){
	    if(vUsed[ii] == 1){
			normalesA[jj] = normales[ii];
			gnormales.Add(normales[ii]);
			jj++;
		}
	}

//	#endregion
 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////     UVs      //////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////// 
 
 
//	#region UVs
	Vector2[] uvsA = new Vector2[verticesA.Length];
	Vector2[] uvs = new Vector2[vertices.Length];
 
    int[] uUsed = new int[vertices.Length];
    
    for(int ii=0;ii<NN;ii++){
		uUsed[ii] = 0;
	}
 
	// Bottom cap
	numUsed=0;
	
	int u = 0;
	if(useBottomCap == 1){
		u = 0;
        
        uUsed[u] = 1;
		uvs[u++] = new Vector2(0.5f, 0.5f);
		numUsed++;
	
		while (u <= nbSides)
		{
			float rad = (float)u / nbSides * _2pi;
			uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
			uUsed[u] = 1;
			numUsed++;
			u++;
		}
	} 
	// Top cap
	if(useTopCap == 1){
		u = nbSides+1;
	    
	    uUsed[u] = 1;
		uvs[u++] = new Vector2(0.5f, 0.5f);
		numUsed++;
		while (u <= nbSides * 2 + 1)
		{
			float rad = (float)u / nbSides * _2pi;
			uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
			uUsed[u] = 1;
			numUsed++;
			u++;
		}
	}

 
	u = nbSides * 2 + 2;  
 
	// Sides
	int u_sides = 0;
	while (u <= uvs.Length - 4 )
	{
		float t = (float)u_sides / nbSides;
		uvs[u] = new Vector3(t, 1f);
		uvs[u + 1] = new Vector3(t, 0f);
		uUsed[u] = 1;
		uUsed[u+1] = 1;
		numUsed+=2;
		u += 2;
		u_sides++;
	}
	uvs[u] = new Vector2(1f, 1f);
	uvs[u + 1] = new Vector2(1f, 0f);
	uUsed[u] = 1;
	uUsed[u+1] = 1;
    
    
    jj=0;
	for(int ii=0;ii<NN;ii++){
	    if(uUsed[ii] == 1){
			uvsA[jj] = uvs[ii];
			guvs.Add(uvs[ii]);
			jj++;
		}
	}

//	#endregion 
 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////     Triangles      ///////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////// 
 
 
//	#region Triangles
	int nbTriangles = nbSides + nbSides + nbSides*2;
	int[] triangles = new int[nbTriangles * 3 + 3];

	int NT = nbTriangles * 3 + 3;
	

	int[] tUsed = new int[NT];
	
	for(int ii=0;ii<NT;ii++){
		tUsed[ii] = 0;
	}
 

    numUsed=0;
	// Bottom cap
	int tri = 0;
	int i = 0;
	
	int missTris = 0;

	
	if(useBottomCap == 1){
		while (tri < nbSides - 1)
		{
			if(useBottomCap == 1){
				triangles[ i ] = 0;
				triangles[ i+1 ] = tri - missTris + 1;
				triangles[ i+2 ] = tri - missTris + 2;
			
				tUsed[i] = 1;
				tUsed[i+1] = 1;
				tUsed[i+2] = 1;
			}
			numUsed+=3;
			tri++;
			i += 3;
		}


	
			triangles[i] = 0;
			triangles[i + 1] = tri - missTris + 1;
			triangles[i + 2] = 1;
		
			tUsed[i] = 1;
			tUsed[i+1] = 1;
			tUsed[i+2] = 1;
		
			numUsed+=3;
	
	
		tri++;
		i += 3;
	}
    
    
    if(useBottomCap == 1){
        if(useTopCap == 1){
    		missTris = 0;
    	}
    	if(useTopCap == 0){
    		missTris = 0;
    	}
    }
    else if(useBottomCap == 0){
        if(useTopCap == 1){
    		missTris = nbSides+1;
    	}
    	else if(useTopCap == 0){
    		missTris = nbSides+1;
    	}
    }
    
    
	tri = nbSides;
	i = 3*tri;
 
	// Top cap
	if(useTopCap == 1){
		while (tri < nbSides*2)
		{
	
			triangles[ i ] = tri - missTris + 2;
			triangles[i + 1] = tri - missTris + 1;
			triangles[i + 2] = nbVerticesCap - missTris;
			
			tUsed[i] = 1;
			tUsed[i+1] = 1;
			tUsed[i+2] = 1;
	        
	        numUsed+=3;
	        
			tri++;
			i += 3;
			
		}

		triangles[i] = nbVerticesCap - missTris + 1;
		triangles[i + 1] = tri - missTris + 1;
		triangles[i + 2] = nbVerticesCap - missTris;	
		
		tUsed[i] = 1;
		tUsed[i+1] = 1;
		tUsed[i+2] = 1;
	    
	    numUsed+=3;
		tri++;
		i += 3;
		tri++;
	}


	
    if(useBottomCap == 1){
        if(useTopCap == 1){
    		missTris = 0;
    	}
    	if(useTopCap == 0){
    		missTris = nbSides+1;
    	}
    }
    else if(useBottomCap == 0){
        if(useTopCap == 1){
    		missTris = nbSides+1;
    	}
    	else if(useTopCap == 0){
    		missTris = nbSides*2+2;
    	}
    }
    
	tri = nbSides*2+2;
	i = 3*tri-3;

    
	// Sides
	while( tri <= nbTriangles )
	{
		triangles[ i ] = tri - missTris + 2;
		triangles[ i+1 ] = tri - missTris + 1;
		triangles[ i+2 ] = tri - missTris + 0;
		
		tUsed[i] = 1;
		tUsed[i+1] = 1;
		tUsed[i+2] = 1;
		
		numUsed+=3;
		tri++;
		i += 3;
 
		triangles[ i ] = tri - missTris + 1;
		triangles[ i+1 ] = tri - missTris + 2;
		triangles[ i+2 ] = tri - missTris + 0;
		
		tUsed[i] = 1;
		tUsed[i+1] = 1;
		tUsed[i+2] = 1;
		
		numUsed+=3;
		tri++;
		i += 3;
	}
	
    int[] trianglesA = new int[numUsed];
    
    jj = 0;
	for(int ii=0;ii<NT;ii++){
	    if(tUsed[ii] == 1){
			trianglesA[jj] = triangles[ii];
			gtriangles.Add(triangles[ii]+verticesOffset);
			jj++;
		}
	}
	
	
	
	
	jj = 0;
	minVertex.Add(verticesOffset);
	for(int ii=0;ii<NN;ii++){
	    
	    if(vUsed[ii] == 1){
			
			jj++;
		}
	}
	maxVertex.Add(verticesOffset+jj);
	verticesOffset = verticesOffset+jj;
	currentSegmentOffset = jj;
	

	
	minTriangle.Add(trianglesOffset);
	jj = 0;
	for(int ii=0;ii<NT;ii++){
	    if(tUsed[ii] == 1){
			
			jj++;
		}
	}
//	print(jj);
	maxTriangle.Add(trianglesOffset+jj);
	trianglesOffset = trianglesOffset+jj;
	
	
	currentSegmentId++;


//	#endregion

/////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////


 


	
	}
	
	void CreateMesh(){
	
	        if(gvertices.Count>40000){
            int tempCount = 40000;
        	while((tempCount % (nbSides * 2+2))!=0){
        		tempCount--;
        	}
       // 	print(tempCount);
       // 	print(gvertices.Count/tempCount+1);
        	
       // 	int splitCount = 
        	
        	
        	List<List<Vector3>> ggvertices = new List<List<Vector3>>();
			List<List<Vector3>> ggnormales = new List<List<Vector3>>();
			List<List<Vector2>> gguvs = new List<List<Vector2>>();
			List<List<int>> ggtriangles = new List<List<int>>();
			
			int maxj = gvertices.Count/tempCount+1;
			for(int j=0;j<maxj;j++){
		//	    print(j);
				ggvertices.Add(new List<Vector3>());
				ggnormales.Add(new List<Vector3>());
				gguvs.Add(new List<Vector2>());
				ggtriangles.Add(new List<int>());
			
			    int mini = j*tempCount;
			    int maxi = (j+1)*tempCount;
			    
			    if(j==maxj-1){
			    	maxi = gvertices.Count;
			    }
			
				for(int i=mini; i<maxi; i++){
					ggvertices[j].Add(gvertices[i]);
					ggnormales[j].Add(gnormales[i]);
					gguvs[j].Add(guvs[i]);
				}
			    
			    int minTri = j*(3*tempCount - tempCount/(nbSides * 2+2)*nbSides/3);
				int maxTri = (j+1)*(3*tempCount - tempCount/(nbSides * 2+2)*nbSides/3);
				
				if(j==maxj-1){
			    	maxTri = gtriangles.Count;
			    }
			
				for(int i=minTri; i<maxTri; i++){
					ggtriangles[j].Add(gtriangles[i]-mini);
				
				}
			
				GameObject plane = new GameObject("Plane");
				MeshFilter filter = plane.AddComponent<MeshFilter>();
				Mesh mesh = filter.mesh;
				mesh.Clear();
			
			
				mesh.vertices = ggvertices[j].ToArray();
				mesh.normals = ggnormales[j].ToArray();
				mesh.uv = gguvs[j].ToArray();
				mesh.triangles = ggtriangles[j].ToArray();
 
				mesh.RecalculateBounds();
				mesh.Optimize();
			
				MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
				renderer.material.shader = Shader.Find ("Diffuse");
				Texture2D tex = new Texture2D(1, 1);
				tex.SetPixel(0, 0, Color.green);
				tex.Apply();
				renderer.material.mainTexture = tex;
				renderer.material.color = Color.green;
			
			}
        }
		
		else{
			GameObject plane = new GameObject("Plane");
			MeshFilter filter = plane.AddComponent<MeshFilter>();
			Mesh mesh = filter.mesh;
			mesh.Clear();
			
			mesh.vertices = gvertices.ToArray();
			mesh.normals = gnormales.ToArray();
			mesh.uv = guvs.ToArray();
			mesh.triangles = gtriangles.ToArray();
 
			mesh.RecalculateBounds();
			mesh.Optimize();
			
			MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
			renderer.material.shader = Shader.Find ("Diffuse");
			Texture2D tex = new Texture2D(1, 1);
			tex.SetPixel(0, 0, Color.green);
			tex.Apply();
			renderer.material.mainTexture = tex;
			renderer.material.color = Color.green;
			
		}
	}
	
	
}
