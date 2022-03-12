import { Component, OnInit } from '@angular/core';

import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {ActivatedRoute,Router} from '@angular/router';
import { MatSnackBar} from '@angular/material/snack-bar';

import { ResponseModel,DropDownModel } from '../../shared/models/common-data';
import { ResourceClientService } from '../../service/resourceclient.service';


@Component({
  selector: 'app-client-dashboard',
  templateUrl: './client-dashboard.component.html',
  styles: []
})
export class ClientDashboardComponent implements OnInit {

  clientProfileId='0';  
  clientProfileList: DropDownModel[] = [];

  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService,private route: ActivatedRoute,
    private router: Router,private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/masters/dropdown/business-profile').subscribe((data: DropDownModel[])=>{
      this.clientProfileList = data;
    })

  }

  OnAddNewProfile()
  {
    this.router.navigate(['secured/masters/client-profile']);
  }
  OnGet(){

  }
}

