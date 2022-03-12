import { Component, OnInit } from '@angular/core';
import { MatSnackBar} from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { ResourceClientService } from 'src/app/service/resourceclient.service';
import { DropDownModel, ResponseModel } from 'src/app/shared/models/common-data';

@Component({
  selector: 'app-manage-master',
  templateUrl: './manage-master.component.html',
  styles: []
})
export class ManageMasterComponent implements OnInit {

  title='';
  master='';
  enteredvalue='';

  documentsSelected: DropDownModel[] =[];

  displayedColumns: string[] = ['text','toolbox'];
  
  dataSource = new MatTableDataSource<DropDownModel>(this.documentsSelected);

  constructor(
    private resourceClient: ResourceClientService,private route: ActivatedRoute,private router: Router,
    private snackBar: MatSnackBar)
    { 
      
    } 

    ngOnInit(): void {

      this.route.params.subscribe(params => {
        this.master = params['master']

        if (this.master==='industrial-classification')
          this.title='Manage Industrial Classification master' 
        else  if (this.master==='client-status')
          this.title='Manage Client Status master' 
        else
          this.title='Manage ' + this.master + ' master' 

        this.resourceClient.getDataInGet('api/masters/dropdown/'+this.master).subscribe((data: DropDownModel[])=>{
        
          this.dataSource = new MatTableDataSource<DropDownModel>(data);
        
          this.documentsSelected = data;
        
            
        })  

        });
         
      
    }

    OnAdd()
    {
      this.enteredvalue = this.enteredvalue.trim();
      if (this.enteredvalue.length===0)
        return;
        
      var documents: DropDownModel[] =[];
      var hasValue=false;

      if (this.documentsSelected?.length>0)
      for(var doc of this.documentsSelected)
      {
        if (this.enteredvalue ===doc.text)
        {
          hasValue=true;
          continue;
        }
          else
          documents.push(doc);
      }

      
      if (!hasValue)
      {
        this.resourceClient.getDataInPost('api/masters/add',{Value:this.master,Text:this.enteredvalue} ).subscribe((data: ResponseModel)=>{
          if (data.isSuccess)
          {
            documents.push({value:'',text:this.enteredvalue} );
            this.enteredvalue='';
            this.documentsSelected = documents;
            this.dataSource = new MatTableDataSource<DropDownModel>(this.documentsSelected);
     
          }
   
          this.OnShowMessage(data.message)
             
         })  
     
      }
      else
        this.OnShowMessage('Duplicate value can not be added.')
    }

    OnDelete(documentsData:DropDownModel)
    {

      this.resourceClient.getDataInDelete('api/masters/delete/'+this.master,documentsData.text).subscribe((data: ResponseModel)=>{
        
       if (data.isSuccess)
       {
        var documents: DropDownModel[] =[];

        for(var doc of this.documentsSelected)
        {
          if (documentsData.text ===doc.text)
            continue;
          else
            documents.push(doc);
        }
  
        this.documentsSelected = documents;
        this.dataSource = new MatTableDataSource<DropDownModel>(this.documentsSelected);
  
       }

       this.OnShowMessage(data.message)
          
      })  

    }
    OnShowMessage(message)
    {
      this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
    }
}
