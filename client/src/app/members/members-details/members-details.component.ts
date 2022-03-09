import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';


import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-members-details',
  templateUrl: './members-details.component.html',
  styleUrls: ['./members-details.component.css']
})
export class MembersDetailsComponent implements OnInit {
 //(L191)
 /*  @ViewChild('memberTabs') memberTabs:TabsetComponent;(L192) */
  //(L191)
  @ViewChild('memberTabs',{static:true}) memberTabs:TabsetComponent;
  //(L192)

  //(L192)
  member:Member ;
 galleryOptions: NgxGalleryOptions[];
 galleryImages: NgxGalleryImage[];
//(L191)
 activeTab:TabDirective;
 messages: Message[] | any=[];
  //(L191)
 
  
  /* constructor(private memberService:MembersService, private route:ActivatedRoute) { } (L191) */
  constructor(private memberService:MembersService, private route:ActivatedRoute, private messageService:MessageService) { }
  ngOnInit(): void {
    /* this.loadMemeber(); (L193)*/

    //(L193)
    this.route.data.subscribe
    (
      data=>
      {
        this.member=data['member'];
      }



    )
    //(L193)

    //(L192)
      this.route.queryParams.subscribe(
        params=>
        {
          params['tab'] ? this.selectTab(params['tab']) :this.selectTab(0);
        }



      )
    //(L192)
    
    this.galleryOptions=[{
      width:'100px',
      height:'100px',
      imagePercent:100,
      thumbnailsColumns:4,
      imageAnimation:NgxGalleryAnimation.Slide,
      preview:false

     

    }]
    /* this.galleryImages=this.getImages(); */
    //(L193)
       this.galleryImages=this.getImages();
    //(L193)
  }

  getImages():NgxGalleryImage[]
  {
    const imageUrls=[];
    for (const photo of this.member.photos) {
     imageUrls.push({
      small:photo?.url,
      medium:photo?.url,
      big:photo?.url
     });
      
    }
    return imageUrls;


  }
/* loadMemeber()
{
 //   this.route = JSON.parse(localStorage.getItem('username') || '{}'); (zakomentarisan ranije)
  this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(
   member=>{this.member=member;
           //  this.galleryImages=this.getImages();(L193) 
  
  }) 
 
 
}(L193) */

//(L191)
loadMessages()
{
  this.messageService.getMessageThread(/* this.username (L192) */ this.member.username)
  .subscribe
  (
    messages=> 
    {
      this.messages=messages;
     
    }

  )
} 
//(L191)

//(L191)
onTabActivated(data:TabDirective)
{
this.activeTab=data;
if(this.activeTab.heading==='Messages' )/* && this.messages.length===0 */
{
this.loadMessages();

}

}

//(L191)

//(L192)
selectTab(tabId:number)
{
this.memberTabs.tabs[tabId].active=true;

}
//(L192)

}
