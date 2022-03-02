import { Component, OnInit } from '@angular/core';
import { Observable} from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { take} from 'rxjs';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  /* members: Member[]; */
 /*members$:Observable<Member[]>; (L155)*/
 members:Member[]|null;
 pagination:Pagination;
 /* pageNumber=1;
 pageSize=5; (L159)  */
 //(L159) */
 userParams:UserParams;
 user: User|null ;
//(L159)
//(L160)
genderList=[{value :'male',display:'Males'},{value :'female',display:'Females'}]

//(L160)

  /* constructor(private memberService:MembersService) { } (L159)*/
  
  /* constructor(private memberService:MembersService,private accountService :AccountService)
    
    { 
      this.accountService.currentUser$.pipe(take(1)).subscribe
        ( user=>
          {
              this.user=user;
              this.userParams=new UserParams(user);
          }



        )
    } (L168) */
    constructor(private memberService:MembersService)
    
    { 
      this.userParams=this.memberService.getUserParams();

    }


  ngOnInit(): void 
  {
   /*  this.loadMembers(); */
    /* this.members$=this.memberService.getMembers();  */
     this.loadMembers();/* (L155) */
  }
/* loadMembers()
{

  this.memberService.getMembers().subscribe(members=>{this.members=members;})


} */
loadMembers() /* L(155) */
{

  /* this.memberService.getMembers(this.pageNumber,this.pageSize).subscribe(response=>
    {
      this.members=response.result;
      this.pagination=response.pagination;
    }) (L159)  */
   //(L168)
   this.memberService.setUserParams(this.userParams);
   //(L168)
    this.memberService.getMembers(this.userParams).subscribe(response=>
      {
        this.members=response.result;
        this.pagination=response.pagination;
      })

}
//(L160)
/* resetFilters()
{
  this.userParams= new UserParams(this.user);
}  (L168)*/
//(L160)

//(L168)
resetFilters()
{
  this.userParams= this.memberService.resetUserParams();
  this.loadMembers();

}  
//(L168)

pageChanged(event:any)
{
/* this.pageNumber=event.page;
this.loadMembers();  (L159)*/
this.userParams.pageNumber=event.page;
//(L168)
this.memberService.setUserParams(this.userParams);
//(L168)
this.loadMembers();
}
}
