using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyCreate : MonoBehaviour
{
    public int ra=100;
    public int i1;
    public CameraController CameraController;
    public GameObject OB;
    public GameObject Robot;
    public GameObject OB1;
    public GameObject OB2;
    public GameObject OB3;
    public Vector4 Posibility;
    public int EnermyNumber = 100;
    public int EA=0;
    public int E=0;
    // Start is called before the first frame update
    void Start()
    {   for(int n=0;n<100;n++){
        float a1=Random.Range(-50,50);
        float a2=Random.Range(-50,50);
        GameObject copy = Instantiate(OB);
        copy.transform.position = new Vector3(a1, a2, 0);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(i1<EnermyNumber){
            GameObject copy = Instantiate(OB);
            float a1=Random.Range(-ra,ra);
            float a2=Random.Range(-ra,ra);
            float a3=Random.Range(-100,100);
            Vector2 k=CameraController.Po();
            while(i1<EnermyNumber){
                a1=Random.Range(-ra,ra);
                a2=Random.Range(-ra,ra);
                if((a1-k[0])*(a1-k[0])+(a2-k[1])*(a2-k[1])<-600||(a1-k[0])*(a1-k[0])+(a2-k[1])*(a2-k[1])>600){
                    break;}
                }
            if((a3<Posibility[0]&&a3>-Posibility[0])&&E<EA){
            GameObject copy1 = Instantiate(Robot);
            copy1.transform.position = new Vector3(a1, a2, 0);
            i1+=1;
            E+=1;
            }
            else if(a3<Posibility[1]+Posibility[0]&&a3>-Posibility[0]-Posibility[1]){
            GameObject copy1 = Instantiate(OB1);
            copy1.transform.position = new Vector3(a1, a2, 0);
            i1+=1;
            }
            else if(a3<Posibility[1]+Posibility[0]+Posibility[2]&&a3>-Posibility[0]-Posibility[1]-Posibility[2]){
            GameObject copy1 = Instantiate(OB2);
            copy1.transform.position = new Vector3(a1, a2, 0);
            i1+=1;
            }
            else if(a3<Posibility[1]+Posibility[0]+Posibility[2]+Posibility[3]&&a3>-Posibility[0]-Posibility[1]-Posibility[2]-Posibility[3]){
            GameObject copy1 = Instantiate(OB3);
            copy1.transform.position = new Vector3(a1, a2, 0);
            i1+=1;
            }
            else{
            copy.transform.position = new Vector3(a1, a2, 0);
            i1+=1;
            }
        }
        
    }
    public void de(){
        i1=i1-2;
    }
}
