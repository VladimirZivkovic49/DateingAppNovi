import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
/* export class TextInputComponent implements OnInit (L144) */ 

  export class TextInputComponent implements ControlValueAccessor {

  @Input() label:string;
  @Input() type="text";
  constructor(@Self() public ngControl:NgControl) 
  {

    this.ngControl.valueAccessor=this;

   }
  writeValue(obj: any): void {
   
  }
  registerOnChange(fn: any): void {
    
  }
  registerOnTouched(fn: any): void {
    
  }

  /* ngOnInit(): void {
  }     (L144)*/

}
