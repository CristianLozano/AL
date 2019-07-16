using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LSystemGenerator : MonoBehaviour {
    
    
    private List<List<Vector3>> nextOffset = new List<List<Vector3>>();
    private List<Vector3> nextOffseta = new List<Vector3>();
    
    private List<Vector3> baseVertices = new List<Vector3>();
    
    public int numberOfPlants = 7;
    
    public int nBranches = 10;
    
    public float branchWidth = 0.01f;
    
    public float branchThickenning1 = 1.15f;
    public float branchThickenning2 = 0.0f;
    
    public float branchVariation = 15.0f;
    public float groundStartVariation = 0.0f;
    
    public int nSegments = 25;
    
    public float openingAngle = 1f;
    public float segmentLength = 1f;
    
    public int nbSides = 18;

    public Texture tex;

    private int iterationId = 0;
    
    private Vector3 startLocation;
    
    
    
    private List<bool> isVerticeTop = new List<bool>();
    
    void Start(){
    
    
    	
    for(int j=0;j<numberOfPlants;j++){	
    	startLocation = new Vector3(0f,0f,0f);
    	
    	for(int i=0;i<nBranches;i++){
    	    float ff = Random.Range(-openingAngle, openingAngle);
    	    
    	    float o1 = Random.Range(-1.0f, 1.0f);
    	    float o2 = Random.Range(-1.0f, 1.0f);
    	    
    		BuildBranch (ff, new Vector3(o1,0f,o2));
    		nextOffseta.Clear();
    //		nextOffseta.Add(new Vector3(o1,0f,o2));
    	}
    }
    
    }
    
    

	void BuildBranch (float angle, Vector3 orientation) {
		int nn = 1;
		
		
		
		for(int i=0;i<nbSides*2+2;i++){
			baseVertices.Add(new Vector3(0f,0f,0f));
		}
		for(int i=0;i<nbSides*4+4;i++){
			isVerticeTop.Add(false);
		}
		

		
		List<List<float>> bottomNode = new List<List<float>>();
		List<List<float>> topNode = new List<List<float>>();
		List<List<Vector3>> offsetL = new List<List<Vector3>>();
		
		List<List<Quaternion>> rotationL = new List<List<Quaternion>>();
		List<List<Vector3>> rotDir = new List<List<Vector3>>();
		
		List<List<float>> prevAng = new List<List<float>>();
		
		List<int> jLevels = new List<int>();
		
		jLevels.Add(1);
		jLevels.Add(2);
		jLevels.Add(4);
		
		for(int i=0;i<nSegments;i++){
		nextOffset.Add(new List<Vector3>());
		nextOffseta.Add((new Vector3(Random.Range(-groundStartVariation, groundStartVariation),0f,Random.Range(-groundStartVariation, groundStartVariation)))+startLocation);
		for(int j=0;j<nn;j++){
		    nextOffset[i].Add(new Vector3(0f,0f,0f));

		}
		}
		
		
		for(int i=0;i<nSegments;i++){
			bottomNode.Add(new List<float>());
			topNode.Add(new List<float>());
			offsetL.Add(new List<Vector3>());
			rotationL.Add(new List<Quaternion>());
			rotDir.Add(new List<Vector3>());

			prevAng.Add(new List<float>());
			
			for(int j=0;j<nn;j++){
				bottomNode[i].Add(0.0f);
				topNode[i].Add(0.0f);
				offsetL[i].Add(new Vector3(0f,0f,0f));
				rotDir[i].Add(orientation);
				rotationL[i].Add(Quaternion.AngleAxis( 0.0f, new Vector3(0f,0f,1f) ));

				prevAng[i].Add(0.0f);
			}
		}
		

		
		float iniBN = branchWidth+0.2f*branchWidth;
		float iniTN = branchWidth;
		
		for(int i=0;i<nSegments;i++){
			for(int j=0;j<nn;j++){
				bottomNode[i][j] = iniBN;
				topNode[i][j] = iniTN;
			}
		}
		
		for(int i=1;i<nSegments;i++){
			
			for(int ii=1;ii<=i;ii++){
			    for(int j=0;j<nn;j++){
					bottomNode[ii-1][j] = bottomNode[ii-1][j]*branchThickenning1+branchThickenning2;
					topNode[ii-1][j] = topNode[ii-1][j]*branchThickenning1+branchThickenning2;
				}
			}
			for(int j=0;j<nn;j++){
				bottomNode[i][j] = topNode[i-1][j];
				topNode[i][j]=iniTN;
			}
		}
		
		for(int i=1;i<nSegments;i++){
			for(int j=0;j<nn;j++){
		//		offsetL[i][j] = offsetL[i-1][j]+(new Vector3(0f,1f,0f));
			}

		}
		
		for(int i=1;i<nSegments;i++){
			for(int j=0;j<nn;j++){
				rotDir[i][j]= rotationL[i-1][j] * rotDir[i-1][j];
				float ang = 0.0f;
				
				if(j<2){
					if((j)%2==0){
						ang = angle;
						
			//			print('1');
					}
					else{
						ang = -angle;
						
		//				print('2');
					}
				}
				else{
					if((j)%2==0){
						ang = -angle;
						
		//				print('3');
					}
					else{
						ang = angle;
						
		//				print('4');
					}
				}
				
				
					prevAng[i][j]=prevAng[i-1][j]+
								  ang+
								  Random.Range(-branchVariation, branchVariation);
				
				rotationL[i][j]=Quaternion.AngleAxis( prevAng[i][j],rotDir[i][j]);
				
			}
		}
		
		
		

		for(int i=0;i<nSegments;i++){

			for(int j=0;j<nn;j++){
				CreateCone(bottomNode[i][j],topNode[i][j],nextOffseta[j],rotationL[i][j],i,j);
				//CreateCone(bottomNode[i][1],topNode[i][1],nextOffset[1],rotationL[i][1],1);
			}
			iterationId = iterationId+1;
			
		}
		
		
		

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	Mesh CreateMesh(float width, float height)
	{
    	Mesh m = new Mesh();
    	m.name = "ScriptedMesh";
    	m.vertices = new Vector3[] {
      //  	new Vector3(-width, -height, 0.01f),
      //  	new Vector3(width, -height, 0.01f),
      //  	new Vector3(width, height, 0.01f),
      //  	new Vector3(-width, height, 0.01f)
        	
        	new Vector3(-width, 0.0f, -height),
        	new Vector3(width, 0.0f, -height),
        	new Vector3(width, 0.0f, height),
        	new Vector3(-width, 0.0f, height)
    	};	
    	m.uv = new Vector2[] {
        	new Vector2 (0, 0),
        	new Vector2 (0, 1),
        	new Vector2(1, 1),
        	new Vector2 (1, 0)
        	
     //   	new Vector2 (0, 0),
     //   	new Vector2 (1, 0),
     //   	new Vector2(1, 1),
     //   	new Vector2 (0, 1)
    	};
    	m.triangles = new int[] { 0, 2, 1, 0, 3, 2};
    	m.RecalculateNormals();
 
    	return m;
	}
	
	
	Mesh CreateTriMesh(
	
		Vector3 a,
		Vector3 b,
		Vector3 c
	
	)

	{
    	Mesh m = new Mesh();
    	m.name = "ScriptedMesh";
    	m.vertices = new Vector3[] {

        	
        	a,
        	b,
        	c
    	};	
    	m.uv = new Vector2[] {
        	new Vector2 (0, 0),
        	new Vector2 (0, 1),
        	new Vector2(1, 1)
        	

    	};
    	m.triangles = new int[] { 0, 1, 2};
    	m.RecalculateNormals();
 
    	return m;
	}
	
	
	
	int vari;
	void CreatePlane(){
	GameObject plane = new GameObject("Segment");
    plane.transform.SetParent(this.transform);
    plane.transform.localPosition = new Vector3(0f, 0f, 0f);
    MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
    
    

  // meshFilter.mesh = CreateCone();
    

    
    MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
    renderer.material.shader = Shader.Find ("Diffuse");
    /*Texture2D tex = new Texture2D(1, 1);
    tex.SetPixel(0, 0, Color.green);
    tex.Apply();*/
    renderer.material.mainTexture = tex;
    renderer.material.color = Color.green;
	
	Mesh msh = meshFilter.mesh;

	meshFilter.mesh = msh;
	}
	
	
    void CreateCone(float bottomRadius, 
                    float topRadius, 
                    Vector3 offset,
                    Quaternion qRotation,
                    int iii,
                    int jjj
                    ){

GameObject plane = new GameObject("Segment");
plane.transform.SetParent(this.transform);
plane.transform.localPosition = new Vector3(0f, 0f, 0f);
MeshFilter filter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));


Mesh mesh = filter.mesh;
mesh.Clear();
 
float height = segmentLength;
//float bottomRadius = .25f;
//float topRadius = .05f;

int nbHeightSeg = 1; // Not implemented yet
 
int nbVerticesCap = nbSides + 1;
#region Vertices
 
// bottom + top + sides
Vector3[] vertices = new Vector3[nbVerticesCap + nbVerticesCap + nbSides * nbHeightSeg * 2 + 2];
int vert = 0;
float _2pi = Mathf.PI * 2f;
 
// Bottom cap
vertices[vert++] = new Vector3(0f, 0f, 0f);
while( vert <= nbSides )
{
	float rad = (float)vert / nbSides * _2pi;
	vertices[vert] = new Vector3(Mathf.Cos(rad) * bottomRadius, 0f, Mathf.Sin(rad) * bottomRadius);

	vert++;
}
 
// Top cap
vertices[vert++] = new Vector3(0f, height, 0f);
while (vert <= nbSides * 2 + 1)
{
	float rad = (float)(vert - nbSides - 1)  / nbSides * _2pi;
	vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
	vert++;
}
 
// Sides
int v = 0;
while (vert <= vertices.Length - 4 )
{
	float rad = (float)v / nbSides * _2pi;
	vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
	vertices[vert + 1] = new Vector3(Mathf.Cos(rad) * bottomRadius, 0, Mathf.Sin(rad) * bottomRadius);
	vert+=2;
	v++;
}
vertices[vert] = vertices[ nbSides * 2 + 2 ];
vertices[vert + 1] = vertices[nbSides * 2 + 3 ];


int N = vertices.Length;

if(iterationId==0){
	for(int ii = 0; ii < N; ii ++){
		if(vertices[ii].y>0.1f){
			isVerticeTop[ii]=true;
			
		}
	}
}

// rotation
for (int ii = 0; ii < N; ii ++)
{
        vertices[ii] = qRotation * vertices[ii];
}

// position
for (int ii = 0; ii < N; ii ++)
{
	vertices[ii]=vertices[ii]+offset;
}

nextOffseta[jjj] = vertices[nbSides+1];
//print(vertices[nbSides+1]);

int ch=0;
if(iterationId==0){



    
    for (int ii = 0; ii < N; ii ++){

		if(isVerticeTop[ii]==false){
	  //      print(ch);
		//	vertices[ii] = baseVertices[ch];
			ch = ch+1;
		}

	}
	
	ch = 0;
	for (int ii = 0; ii < N; ii ++){
		if(isVerticeTop[ii]==true){
			baseVertices[ch]=vertices[ii];
			ch = ch+1;
		}
	}
	
}


#endregion
 
#region Normales
 
// bottom + top + sides
Vector3[] normales = new Vector3[vertices.Length];
vert = 0;
 
// Bottom cap
while( vert  <= nbSides )
{
	normales[vert++] = Vector3.down;
}
 
// Top cap
while( vert <= nbSides * 2 + 1 )
{
	normales[vert++] = Vector3.up;
}
 
// Sides
v = 0;
while (vert <= vertices.Length - 4 )
{			
	float rad = (float)v / nbSides * _2pi;
	float cos = Mathf.Cos(rad);
	float sin = Mathf.Sin(rad);
 
	normales[vert] = new Vector3(cos, 0f, sin);
	normales[vert+1] = normales[vert];
 
	vert+=2;
	v++;
}
normales[vert] = normales[ nbSides * 2 + 2 ];
normales[vert + 1] = normales[nbSides * 2 + 3 ];
#endregion
 
#region UVs
Vector2[] uvs = new Vector2[vertices.Length];
 
// Bottom cap
int u = 0;
uvs[u++] = new Vector2(0.5f, 0.5f);
while (u <= nbSides)
{
    float rad = (float)u / nbSides * _2pi;
    uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
    u++;
}
 
// Top cap
uvs[u++] = new Vector2(0.5f, 0.5f);
while (u <= nbSides * 2 + 1)
{
    float rad = (float)u / nbSides * _2pi;
    uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
    u++;
}
 
// Sides
int u_sides = 0;
while (u <= uvs.Length - 4 )
{
    float t = (float)u_sides / nbSides;
    uvs[u] = new Vector3(t, 1f);
    uvs[u + 1] = new Vector3(t, 0f);
    u += 2;
    u_sides++;
}
uvs[u] = new Vector2(1f, 1f);
uvs[u + 1] = new Vector2(1f, 0f);
#endregion 
 
#region Triangles
int nbTriangles = nbSides + nbSides + nbSides*2;
int[] triangles = new int[nbTriangles * 3 + 3];
 
// Bottom cap
int tri = 0;
int i = 0;
while (tri < nbSides - 1)
{
	triangles[ i ] = 0;
	triangles[ i+1 ] = tri + 1;
	triangles[ i+2 ] = tri + 2;
	tri++;
	i += 3;
}
triangles[i] = 0;
triangles[i + 1] = tri + 1;
triangles[i + 2] = 1;
tri++;
i += 3;
 
// Top cap
//tri++;
while (tri < nbSides*2)
{
	triangles[ i ] = tri + 2;
	triangles[i + 1] = tri + 1;
	triangles[i + 2] = nbVerticesCap;
	tri++;
	i += 3;
}
 
triangles[i] = nbVerticesCap + 1;
triangles[i + 1] = tri + 1;
triangles[i + 2] = nbVerticesCap;		
tri++;
i += 3;
tri++;
 
// Sides
while( tri <= nbTriangles )
{
	triangles[ i ] = tri + 2;
	triangles[ i+1 ] = tri + 1;
	triangles[ i+2 ] = tri + 0;
	tri++;
	i += 3;
 
	triangles[ i ] = tri + 1;
	triangles[ i+1 ] = tri + 2;
	triangles[ i+2 ] = tri + 0;
	tri++;
	i += 3;
}
#endregion










 
mesh.vertices = vertices;
mesh.normals = normales;
mesh.uv = uvs;
mesh.triangles = triangles;
 
mesh.RecalculateBounds();
mesh.Optimize();




  
  
    MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
    renderer.material.shader = Shader.Find ("Diffuse");
    /*Texture2D tex = new Texture2D(1, 1);
    tex.SetPixel(0, 0, Color.green);
    tex.Apply();*/
    renderer.material.mainTexture = tex;
    renderer.material.color = Color.green;
	
	Mesh msh = filter.mesh;

	filter.mesh = msh;  
  
  
    
//return mesh;    
    
    
    
    
    }
	
	
	
	
	
}
