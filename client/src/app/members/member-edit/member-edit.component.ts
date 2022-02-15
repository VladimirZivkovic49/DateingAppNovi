import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editform:NgForm;
  member: Member;
  user: User|null ;
  @HostListener('window:beforeunload',['$event']) unloadNotification($event:any){
    if(this.editform.dirty){
      $event.returnValue=true;

    }

  }

  constructor(private memberService:MembersService,
     private accountService:AccountService, private toastr:ToastrService  )
    {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user=>this.user=user);

    }

  ngOnInit(): void
   {
      this.loadMemeber();
   }

  loadMemeber()
  {
     var uuser=this.user?.username;
     if(uuser!=null){
      this.memberService.getMember(uuser).subscribe(
        member=>{this.member=member;
                })
   
     }
    
  }
  updateMemeber()
  {
    /* console.log(this.member); */
    this.memberService.updateMember(this.member).subscribe(()=>

   { this.toastr.success("Profile updated sussefully");
    this.editform.reset(this.member);}

    );
    /* this.toastr.success("Profile updated sussefully");
    this.editform.reset(this.member); */
  }

}
