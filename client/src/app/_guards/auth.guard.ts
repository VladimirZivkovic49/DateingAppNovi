import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
 /*  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return true;
  } */
  
  constructor(private accountService:AccountService,private toastr:ToastrService){}
  canActivate()
    : Observable<boolean>  {
      return this.accountService.currentUser$.pipe(
        map(user=>{
        if(user){ return true;}
       this.toastr.error('Ne možete da otvorite!!!');
        return false;
        })
  
      )
  }
}
