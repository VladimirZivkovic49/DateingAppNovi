import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-members-card',
  templateUrl: './members-card.component.html',
  styleUrls: ['./members-card.component.css']
})
export class MembersCardComponent implements OnInit {
@Input() member:Member;
 
//(L175)
/* constructor(private memberService: MembersService,private toastr:ToastrService) { } (L224) */
//(L175)
//(L224)
constructor(private memberService: MembersService,private toastr:ToastrService, public presence: PresenceService) { } 
//(L224)

  ngOnInit(): void {
  }
//(L175)
addLike(member:Member)
{
this.memberService.addLike(member.username).subscribe(
()=>{this.toastr.success('You have liked'+member.knownAs);}


)


}
//(L175)
}
