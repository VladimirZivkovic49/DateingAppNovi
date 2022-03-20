import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { RolesModalsComponent } from 'src/app/modals\/roles-modals/roles-modals.component';




import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit 
{
    users:Partial<User[]>| any;
    bsModalRef:BsModalRef | any;
      constructor(private adminService:AdminService, private modalService:BsModalService) { }

      ngOnInit(): void 
      {
        this.getUsersWithRoles();
      
      };
    getUsersWithRoles()
  {
      this.adminService.getUsersWithRoles().subscribe
      (users=>
        {
            this.users=users;

        }
        
        
      )

  };
  openRolesModals(user:User)
  {
//(L215)
/* const initialState: ModalOptions = {
  initialState: {
    list: [
      'Open a modal with component',
      'Pass your data',
      'Do something else',
      '...'
    ],
    title: 'Modal with component'
  }
};  (L217)*/
//(L217)
      const config=
      {
          class:'modal-dialog-centerd',
          initialState:
            {
              user,
              roles:this.getRolesArray(user)
            }
      }
//(L217)
/* (this.bsModalRef = this.modalService.show(RolesModalsComponent, {initialState});(L217) */
       this.bsModalRef = this.modalService.show(RolesModalsComponent,config);
/* this.bsModalRef.content.closeBtnName = 'Close'; (L217) */

//(L217)
   ///////////////////////////  
   this.bsModalRef.content.updateSelectedRoles.subscribe(
    (values: any) =>
          {
            const rolesToUpdate=
            {
              roles:[...values?.filter((el: { checked: boolean })=> 
              el.checked===true).map((el: { name: any })=>el.name)]
            }
            if(rolesToUpdate)
            {
              this.adminService.updateUserRoles
              (
               user.username,
               rolesToUpdate.roles

              ).subscribe(
                ()=>
                {
                 user.roles=[...rolesToUpdate.roles]

                }
            

              )
            
            }
          
          }
   )
    }


///////////////////////////////////////



//(L217)

//(L215)



  /* this.bsModalRef=this.modalService.show(RolesModalsComponent);(L215) */


 
  private getRolesArray(user: User)
    {
      const roles: string[]=[]  ;
      const userRoles:any=user.roles;
      const availableRoles:any[]=
        [
          {name:'Admin',value:'Admin'},    
          {name:'Moderator',value:'Moderator'},
          {name:'Member',value:'Member'}
        ];
          availableRoles.forEach
          ( role=> 
               {
                  let isMatch=false;
                  for(const userRole of userRoles)
                    {
                      if(role.name===userRole)
                        {
                          isMatch=true;
                          role.checked=true;
                          roles.push(role);
                          break;
                        };

                    };
                      if(!isMatch)
                        {
                            role.checked=false;
                            roles.push(role);
                        };

                }
          );
           return roles;
    };
};




