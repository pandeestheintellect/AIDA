import { Component, OnInit } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'

@Component({
  selector: 'app-business-myinfo-intro',
  templateUrl: './client-myinfo-intro.component.html',
  styles: []
})
export class ClientMyinfoIntroComponent implements OnInit {

  email='';
  note='';
  constructor(public dialogRef: MatDialogRef<ClientMyinfoIntroComponent>) { }
  
  ngOnInit(): void {
  }

  OnAction(response){
    if(response==='Email' )
    {
      if ( this.email.trim().length===0)
      {
        this.note='Please enter valid email';
        return;
      }
      else
        response = this.email.trim();
    }
    
    this.dialogRef.close({event:response});
  }

}
