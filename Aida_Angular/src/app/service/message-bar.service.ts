import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class MessageBarService {

  constructor(private snackBar: MatSnackBar) { }

  public ShowInfo(message,displayDuration)
  {
    this.Show(message,displayDuration,'messageInfo')
  }
  public ShowWarning(message,displayDuration)
  {
    this.Show(message,displayDuration,'messageWarning')
  }
  
  Show(message,displayDuration,styleClass)
  {
    this.snackBar.open(message,'', {duration: displayDuration,verticalPosition: 'top',panelClass: styleClass});
  }
}
