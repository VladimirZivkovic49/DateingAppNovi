import { HttpClient, HttpParams } from '@angular/common/http';
import { Container } from '@angular/compiler/src/i18n/i18n_ast';
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';
/* import { next } from 'process'; */
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Group } from '../_models/group';
import { Message } from '../_models/message';
import { User } from '../_models/user';
import { getPaginationHeaders } from './paginationHelper';
import { getPaginatedResult } from './paginationHelper';
@Injectable({
  providedIn: 'root'
})
export class MessageService
{
  baseUrl = environment.apiUrl;
  
  //(L227)
  hubUrl= environment.hubUrl;
  private hubConnection:HubConnection;
  private messageThreadSource= new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();
 //(227)

  constructor(private http:HttpClient) {
    
   }
//(L227)
createHubConnection(user:User|any, otherUsername:string)
{
  
  
  /* const signalR = require("@microsoft/signalr"); */
 
  this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl
    (
      this.hubUrl + 'message?user='+ otherUsername, 
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
        'ReciveMessageThread', messages =>
        {
            this.messageThreadSource.next(messages );

        }
      )
      //(L229)
      this.hubConnection.on
      (
        'NewMessage', message =>
        {
            this.messageThreadSource.pipe
            (
              take(1)
             
              ).subscribe
              (
                messages=>
                {

                  this.messageThreadSource.next
                  (
                      [...messages, message]
                  )
                }

              )
       
        }
      )
      //(L229)
    //(L235)
    this.hubConnection.on
    (
      'UpdatedGroup', (group:Group) =>
      {
         if(group.connections.some(x=>x.username===otherUsername))
         {
          this.messageThread$.pipe
          (
            take(1)
           
            ).subscribe
            (
              messages=>
              {
                 messages.forEach
                 (
                   message=>
                   {
                    if(!message.dateRead)
                    {

                  message.dateRead=new Date(Date.now());
                    }

                   }
                 )
               this.messageThreadSource.next([...messages]);
              }

            )

         }
        
       
     
      }
    )
    //(L235)
    
    
    }
    stopHubConnection()
    {
     //(L228)
      if(this.hubConnection)
     {

      this.hubConnection
      .stop();
     }
     //(L228)
     /*  this.hubConnection
      .stop(); (L228) */
      /* .catch((error: any)=>console.log(error)); */

    }


//(L227)

   getMessages(pageNumber: number, pageSize: number, container:string  )
  {
    let params=getPaginationHeaders(pageNumber,pageSize);
    
     
     params= params.append('Container', container )
     return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
 
  } 

  getMessageThread(username:string)
  {
      return this.http.get<Message[]>(this.baseUrl + 'messages/thread/'+username);

  }
 
 /*  sendMessage(username:string,content:string) (L229)*/
  async sendMessage(username:string,content:string)
  {
   /*  return this.http.post<Message> (this.baseUrl + 'messages',{recipientUsername:username,content}) (L229)*/
  
  //(L229)
   return this.hubConnection.invoke('SendMessage',{recipientUsername:username,content})
   .catch(error=>console.log(error));
  
 //(L229)
 
  }
  deleteMessage(id:number)
  
  {
    return this.http.delete (this.baseUrl + 'messages/'+ id);

  }


}
