
import { Component, OnInit } from '@angular/core';

import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { ConfirmService } from '../_services/confirm.service';
import { MessageService } from '../_services/message.service';
@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
 /* messages:Message[]|any; (L191) */
 messages:Message[]| any=[];
 
 pagination: Pagination;
 container= 'Unread';
 pageNumber=1;
 pageSize=5;

 //(L195)
loading= false;

 //(L195)
 /*  constructor(private messageService: MessageService) { } (L244) */
    constructor(private messageService: MessageService, private confirmService: ConfirmService ) { }

  ngOnInit(): void 
  {
    this.loadMessages();
  }
  loadMessages()
  {
     this.loading=true;
   
    this.messageService.getMessages(this.pageNumber,this.pageSize,this.container)
      .subscribe
      (
        response=> 
        {
         
          this.messages=response.result;
          this.pagination=response.pagination;
          this.loading=false;
        }

      )
       
    }
    deleteMessage(id:number)
    {
      this.confirmService.confirm('Confirm delite message', 'This cannot be undone')
      
        .subscribe(
      
      
        (result: any)=>
        {
            if(result)
            {
              this.messageService.deleteMessage(id)
                .subscribe
                (
                  () => {
                    this.messages.splice(this.messages.findIndex((m: { id: number; }) => m.id === id), 1);


                  }

                ) 

            }

        }
        
        )

     /*  this.messageService.deleteMessage(id)
      .subscribe
      (
        ()=> 
        {
          this.messages.splice(this.messages.findIndex((m: { id: number; }) => m.id===id),1);
          
          
        }

      ) (L244)*/

    }


  pageChanged(event:any)
  {
    this.pageNumber=event.page;
    this.loadMessages();

  }

}
