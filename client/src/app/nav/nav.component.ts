import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit 
{
  model: any = {}
  /*loggedIn:boolean | undefined;*/
  
  /* kada se implementira async  pipe  linija 17 je nepotrebna jer se podaci nalaze u liniji 23*/
 /*  currentUser$ : Observable<User | null> | undefined   ; */ /* treba dodati u tsconfig.json liniju '"strictPropertyInitialization": false"'*/ 
  constructor(/* private  */ public accountService:AccountService, private router:Router, private toastr:ToastrService) { }

  ngOnInit(): void 
  {
    /* this.getCurrentUser(); */
    /* this.currentUser$=this.accountService.currentUser$; */// nema definicje parametra (l 17)
  
  }
  login()
  {

    /* console.log(this.model); */
    /* this.accountService.login(this.model).subscribe({next:response=>
      {
        {console.log(response),
        this.loggedIn=true}
      }, error: (error: User) => {console.log(error)}
    
    }) */
/*  this.accountService.login(this.model).subscribe({
      next:response => {console.log(response)
                       //  this.loggedIn=true },
      error: error => {console.log(error)} 
    
    })*/


    
    this.accountService.login(this.model).subscribe({
      next:response => {this.router.navigateByUrl('/members')
                        }/* , */
     /*  error: error => {console.log(error),this.toastr.error(error.error)} */ 
    
    })
     /* this.accountService.login(this.model).subscribe({
      
      next:response => {console.log(response)},
      next:response => {this.router.navigateByUrl('/members')},
      error: error => {console.log(error);
       this.toastr.error(error.error) }*/
    
    } 
    logout()
    {

    this.accountService.logout();
    this.router.navigateByUrl('/')
    
    /* this.loggedIn=false */;
    }
  
   /*  getCurrentUser()
    {
  this.accountService.currentUser$.subscribe({
  next: user=>{this.loggedIn=!!user},
  error: error => {console.log(error)}
    })
  } */
  
  
  }
