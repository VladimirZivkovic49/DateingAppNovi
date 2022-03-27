import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';
import { PresenceService } from './_services/presence.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
/*export class AppComponent*/ export class AppComponent implements OnInit {
  title = 'The Dateing App';
  users:any;
 // constructor(/* private  http:HttpClient, */ private accountService:AccountService){} (L222)
 constructor( private accountService:AccountService, private presence:PresenceService){}
  ngOnInit() /*void*/ {
    /*throw new Error('Method not implemented.');formiran interfejs za liniju 9*/
  
 /*  this.getUsers(); */
  this.setCurrentUser();
  }
  setCurrentUser()
  {
  /* const user: User=JSON.parse(localStorage.getItem('user')||'{}');
  this .accountService.setCurrentUser(user); L(222) */

  //(L222)
  const user: User=JSON.parse(localStorage.getItem('user')||'{}');
      if(user)
      {
        this .accountService.setCurrentUser(user);
        this.presence.createHubConnection(user);
      }

 
  //(L222)
  
  }
 
 /*  getUsers()
  
  {

    this.http.get('https://localhost:5001/api/users').subscribe({
      next: (response: any) => {this.users = response},
      error: (error: any) => {console.log(error)}
    });


   } */

}
  function getUsers() {
  throw new Error('Function not implemented.');
}

