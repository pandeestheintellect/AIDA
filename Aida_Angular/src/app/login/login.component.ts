import { Component, OnInit,Input } from '@angular/core';

import { AuthService } from '../service/auth.service';
import { MessageBarService } from '../service/message-bar.service';
import { ResourceClientService } from '../service/resourceclient.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  username: string='';
  usernameDisabled=false;
  password:string='';
  passwordPlaceholder='Password';
  note: string='';

  constructor(private authService: AuthService,private resourceClient: ResourceClientService,
    private messagebar:MessageBarService) { }

  ngOnInit(): void {

    
  }

  Onlogin() {
    if (this.passwordPlaceholder==='Password')
    {
      this.getOTP(this.username,this.password);
    }
    else if (this.passwordPlaceholder==='OTP')
    {
      this.authService.signIn(this.username,this.password);
      this.passwordPlaceholder='Password';
      this.username='';
      this.password='';
      this.usernameDisabled=false;
      this.note = ' ';
    }
      

  }
  
  getOTP(userId: string, password:string,onOTP:void){
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
    this.note = 'Please wait generating OTP';


    this.resourceClient.getDataInPost('api/login-otp',{userId:userId,password:btoa( password)}).subscribe((data: any)=>{
    
      if (data.isSuccess===true)
      {
        this.passwordPlaceholder='OTP';
        this.usernameDisabled=true;
        this.messagebar.ShowInfo(data.message,1000);
        this.note = data.message;
      }
      else
      {
        this.messagebar.ShowWarning(data.message,1000);
        this.note = data.message;
      }
        
      
    })

    this.password='';
  }
}
