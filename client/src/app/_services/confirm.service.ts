import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import {  Observable,of } from 'rxjs';
import { ConfirmDialogComponent } from '../modals/confirm-dialog/confirm-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService 
{
bsModelRef: BsModalRef | any;
  constructor(private modalService:BsModalService) { }
  

  /* confirm(title='Confirm', message='Are you shore to do this?',
  btnOkText='Ok',btnCancelText='Cancel') (L244) */
 //(L244)
 confirm(title='Confirmation', message='Are you shore to do this?',
 btnOkText='Ok',btnCancelText='Cancel'): Observable<boolean>
  {
      const config=
      {
         initialState:
        {
          title,
          message,
          btnOkText,
          btnCancelText
        }
      }
        /* this.bsModelRef=this.modalService.show('confirm',config); (L244)*/
        //(L244)
        this.bsModelRef=this.modalService.show(ConfirmDialogComponent,config);
        return new Observable<boolean>(this.getResult());
       
        //(L244)
    
      }
      //(L244)
      private getResult()
      {
          return (observer: { next: (arg0: any) => void; complete: () => void; })=>
          {
            const subscription =this.bsModelRef.onHidden
            
             .subscribe(
                
                () => 
                {
                  observer.next(this.bsModelRef.content.result)
                  observer.complete();
                });
               
                 return{

                
                unsubscribe()
                  {
                      subscription.unsubscribe();
      
                  }
                }
            

          }
          
   
      }
      //(L244)
     
    
   



    /* function unsubscribe() {
      throw new Error('Function not implemented.'); 
    } */
  }

