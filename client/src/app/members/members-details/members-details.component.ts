import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';


import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';


@Component({
  selector: 'app-members-details',
  templateUrl: './members-details.component.html',
  styleUrls: ['./members-details.component.css']
})
export class MembersDetailsComponent implements OnInit {
 member:Member;
 galleryOptions: NgxGalleryOptions[];
 galleryImages: NgxGalleryImage[];
  
 
  
  constructor(private memberService:MembersService, private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.loadMemeber();

    this.galleryOptions=[{
      width:'100px',
      height:'100px',
      imagePercent:100,
      thumbnailsColumns:4,
      imageAnimation:NgxGalleryAnimation.Slide,
      preview:false

      

    }]
    /* this.galleryImages=this.getImages(); */
    
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
loadMemeber()
{
 /*  this.route = JSON.parse(localStorage.getItem('username') || '{}'); */
  this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(
   member=>{this.member=member;
            this.galleryImages=this.getImages();
  
  }) 
 
 
}
}
