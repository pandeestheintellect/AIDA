import { Injectable } from '@angular/core';
import {Router} from '@angular/router';
import { ResourceClientService } from './resourceclient.service';
import { MessageBarService } from './message-bar.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public usernName:string='';
  public sessionId:string='';
  public serviceBusinessId:number=0;
  public officerId:number=0;

  public menus:any[]=[];
  private linksList:string[]=[];

  constructor(private router: Router,private messagebar:MessageBarService,
    private resourceClient: ResourceClientService,) { }
  
  public signIn(userId: string, password:string){
    if(userId.trim().length===0)
    {
      this.messagebar.ShowWarning('Please enter userid',1000);
      return;
    }
    if(password.trim().length===0)
    {
      this.messagebar.ShowWarning('Please enter password',1000);
      return;
    }

    this.resourceClient.getDataInPost('api/login',{userId:userId,password:btoa( password)}).subscribe((data: any)=>{
    
      if (data.isSuccess===true)
      {
        this.messagebar.ShowInfo(data.message,1000);

        if (data.menus!==null)
        {
          this.menus = data.menus;

          this.menus.forEach((menu)=>{
            this.loadLinks(menu);
            if (menu.link==='false')
            {
              menu.sub.forEach((submenu)=>{
                this.loadLinks(submenu);
              })
  
            }
          });

          this.linksList.push('/secured/services/execution'); 
          this.linksList.push('/secured/services/onboarding');
          this.linksList.push('/secured/services/registration');
          this.linksList.push('/secured/services/listing');
          
          this.linksList.push('/secured/dashboards/client-view');
          this.linksList.push('/secured/reports/enquiry-summary');

          this.linksList.push('/secured/reports/uploaded-documents');
          this.linksList.push('/secured/reports/signed-documents');
          
          this.linksList.push('/secured/registers');

        }
          
        this.usernName=data.name;
        this.router.navigate(['/secured/dashboards/company']);
      }
      else
        this.messagebar.ShowWarning(data.message,1000);
      
    })

    //localStorage.setItem('ACCESS_TOKEN', "access_token");
  }
  loadLinks(menu)
  {
    if(menu.link!=='false')
      this.linksList.push(menu.link)
  }

  public doFormFilling(serviceBusinessId,officerId){

    this.serviceBusinessId=serviceBusinessId;
    this.officerId=officerId;
    this.linksList.push('/document-filling/filling');
    this.router.navigate(['/document-filling/filling']); 
    
  }

  public isLoggedIn(){
    return localStorage.getItem('ACCESS_TOKEN') !== null;
  }
  public logout(){
    this.usernName=''
    this.menus=[];
    this.linksList=[];
    this.router.navigate(['/login']);
  }

  public canGo(nextUrl:string):boolean{

    if(this.linksList.length===0)
    {
      this.router.navigate(['/login']);
      return false;
    }
    for (var i=0; i<this.linksList.length;i++)
    {
      if (nextUrl.indexOf(this.linksList[i])>=0)
      {
        return true;
      }
        
    }

    return false;
  }

}