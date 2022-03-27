import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
hubUrl= environment.hubUrl;
private hubConnection:HubConnection;
 private onlineUsersSource= new BehaviorSubject<string[]>([]);
  /* constructor(private toastr:ToastrService) { }  (L233)*/
  constructor(private toastr:ToastrService, private router:Router) { }
  onlineUsers$ = this.onlineUsersSource.asObservable();
  createHubConnection(user:User|any)
  {
    
    
    /* const signalR = require("@microsoft/signalr"); */
   
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl
      (
        this.hubUrl + 'presence', 
      {
          accessTokenFactory:()=>user.token
      }
      )
        .withAutomaticReconnect()
        .build()
        this.hubConnection
        .start()
        .catch((error: any)=>console.log(error));

        this.hubConnection.on
        (
          'UserIsOnLine', username =>
          {
             /*  this.toastr.info(username + 'has connected'); (L234) */
           //(L234)
           
             this.onlineUsers$.pipe
              (take(1)
              
              )
          .subscribe
          (
              usernames =>
              {
                this.onlineUsersSource.next([...usernames,username])

              }
          )
          //(L234)
        }
        )
        this.hubConnection.on
        (
          'UserIsOffLine', username =>
          {
             /*  this.toastr.warning(username + 'has disconnected'); (L234) */
            //(L234)
           
            this.onlineUsers$.pipe
            (take(1)
            
            )
        .subscribe
        (
            usernames =>
            {
              this.onlineUsersSource.next([...usernames.filter(x=> x!==username) ])

            }
        )
        //(L234)
          
          
          
          
          }
        )
        this.hubConnection.on
        (
          'GetOnlineUsers', (usernames:string[]) =>
          {
              this.onlineUsersSource.next(usernames);

          }
      
          )
          //(L233)
          this.hubConnection.on
          (
            'NewMessageRecived', ({username , knownAs}) =>
            {
              this.toastr.info(username + ' has sent you a new message')  
              .onTap
              .pipe(take(1))
              .subscribe
              (
                  ()=> this.router.navigateByUrl('/members/' + username + '?tab=3' )

              );
           
            }
        
            )
          //(L233)
      
      
      
      
        }
      stopHubConnection()
      {
        this.hubConnection
        .stop()
        .catch((error: any)=>console.log(error));

      }



  
}
