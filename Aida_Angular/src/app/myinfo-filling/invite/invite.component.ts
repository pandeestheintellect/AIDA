import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/service/auth.service';
import { ResourceClientService } from 'src/app/service/resourceclient.service';
import { ResponseModel } from 'src/app/shared/models/common-data';

@Component({
  selector: 'app-invite',
  templateUrl: './invite.component.html',
  styleUrls: ['../../login/login.component.scss']
})
export class InviteComponent implements OnInit {

  username: string;
  password:string;
  @Input() notes: string | null;
  key:string='';
  email:string='';
  showMyInfo=false;
  myInfoPosted=false;
  myInfoURL='';
  serviceBusinessId=0;
  officerId=0;
  constructor(private resourceClient: ResourceClientService,private route: ActivatedRoute,private router: Router,
    private auth: AuthService) { }

  ngOnInit(): void {
    
    
    this.route.params.subscribe(params => {
      this.key = params['key'];
      this.email = params['email'];
      if (this.key !==undefined)
      {
        this.resourceClient.getDataInGet( 'api/myinfo-filling-otp/'+this.key+'/'+this.email).subscribe((data:ResponseModel)=>{
          this.notes=data.message;
          
        });
      }
    });
    
    this.notes='Please wait sending OTP ...';
  }

  Onlogin() {
    
    if (this.password.length>5)
    {
      this.resourceClient.getDataInGet('api/myinfo-validate-otp/'+this.key+'/'+this.email+'/'+this.password).subscribe((data:string)=>{
        if (data.length>30)
        {
          this.notes='';
          this.showMyInfo=true;
          this.myInfoURL=data;
        }
        else
          this.notes=data;
      });
    }
    
  }
  OnMyInfo() {
    this.myInfoPosted=true;
    window.open(this.myInfoURL,'_blank');
  }
}