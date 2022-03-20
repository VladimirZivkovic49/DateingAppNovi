import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable  } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';
/* import { map } from 'rxjs'; */



type NewType = Observable<boolean> ;

@Injectable({
  providedIn: 'root'
})

export class AdminGuard implements CanActivate {
 
 constructor(private accountService:AccountService, private toastr:ToastrService) {}
 
  /* canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return true;
  }  */
  
  canActivate(): NewType  {
  return this.accountService.currentUser$.pipe(
    map(user=>{
     
      if(user?.roles?.includes('Admin') || user?.roles?.includes('Moderator'))
      {
          return true;

      }
    /*  else if(user?.roles?.includes('Admin')===undefined || user?.roles?.includes('Moderator')===undefined )
      {
         
        this.toastr.error('You cannot enter to this area')
        return false;
          
      } */
    
    
      else
     {
       
      this.toastr.error('You cannot enter to this area')
      return false;

     }
    })
  )
   
  }
  
  
}
