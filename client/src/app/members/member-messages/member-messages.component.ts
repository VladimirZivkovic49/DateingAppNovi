
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
/* @Input() username:string;
 messages:Message[]|any; (L191) */
 /*  constructor(private messageService:MessageService) { } (L191) */
//(L194)
 /* constructor(private messageService:MessageService) { } (L228) */

 //(L228)
 constructor(public messageService:MessageService) { }
 //(L228)
 @Input() username:string;
 @ViewChild('messageForm') messageForm:NgForm;
 messageContent:string;

//(L194)

@Input() messages: Message[];

     /* constructor() {} (L194)*/
  ngOnInit(): void
   {
 
    /* this.loadMessages(); (L191) */
  }
/* loadMessages()
{
  this.messageService.getMessageThread(this.username)
  .subscribe
  (
    messages=> 
    {
      this.messages=messages;
     
    }

  )
} (L191) */

//(194)
sendMessage()
{
/* this.messageService.sendMessage(this.username,this.messageContent).subscribe (L229) */
this.messageService.sendMessage(this.username,this.messageContent).then

(
  ()=> 
  {
   /*  this.messages.push(message);  (L229)*/
    this.messageForm.reset();
  }

)

}
//(L194)

}

