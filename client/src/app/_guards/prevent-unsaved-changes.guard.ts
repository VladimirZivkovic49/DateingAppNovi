import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { ConfirmService } from '../_services/confirm.service';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  /* canDeactivate(
    component: unknown,    
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return true;
  } */
  //(244)
    constructor(private confirmSevice:ConfirmService)
    {

    }

  //(244)
  
  canDeactivate(
    /* component: MemberEditComponent):boolean  (L244)*/
    component: MemberEditComponent):Observable<boolean>|boolean
    
    {
        if (component.editform.dirty){

           /*  return confirm('are you sure you want to contionue...'); (L244) */
           //(L244)
           return this.confirmSevice.confirm();
           //(L244)

        }
        
        return true;  
    } 
}
