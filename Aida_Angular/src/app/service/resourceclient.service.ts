import { Inject, Injectable } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})


export class ResourceClientService { 

  public REST_API_SERVER = "http://localhost:63644/";  
  //public REST_API_SERVER = "http://aidadev.smartdigitalprojects.com/";
  //public REST_API_SERVER = "http://aida.smartdigitalprojects.com/";
  //public REST_API_SERVER = "http://aidasit.achibiz.com/"

  //public REST_API_SERVER = "/";
  
  constructor(private httpClient: HttpClient) { 
    if( window.location.hostname!=='localhost')
      this.REST_API_SERVER = window.location.protocol +'//'+ window.location.hostname +':'+ window.location.port+'/';
  
	console.log(this.REST_API_SERVER)
  }

  public getDataInGet(path){
    return this.httpClient.get(this.REST_API_SERVER+path);
  }
  public getDataInPost(path,data){
    return this.httpClient.post(this.REST_API_SERVER+path,data);
  }
  public getDataInPut(path,data){
    return this.httpClient.put(this.REST_API_SERVER+path,data);
  }
  public getDataInDelete(path,data){
    return this.httpClient.delete(this.REST_API_SERVER+path+'/'+data);
  }

  public getDataInPostDirect(path,data){
    return this.httpClient.post(path,data);
  }
}
