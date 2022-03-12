import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FormaterService { 

    getFormatedCurrencyValue(inputValue)
    {
        let str = (inputValue+'').split(',');
        inputValue = str.join('');

        if (! isNaN(inputValue)) {

            var result = inputValue.toString().split('.');
            var lastThree = result[0].substring(result[0].length - 3);
            var otherNumbers = result[0].substring(0, result[0].length - 3);
            
            if (otherNumbers != '')
                lastThree = ',' + lastThree;
            
            var output = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;

            if (result.length > 1) {
                output += "." + result[1];
            }            
            
            return output;
        }
        else
            return 0;
        
    }
 
}
