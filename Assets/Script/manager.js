#pragma strict
var clones:int;
var prefab:GameObject;

function Start () {
var i:int;
	for(i=0; i<clones; i++)
	{
		Instantiate(prefab,Vector3(i%8*15,i*2+20,0),Quaternion.identity);
	}
}

function Update () {

}