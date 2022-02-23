import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup,ValidatorFn  , Validators  } from '@angular/forms';
import { Router } from '@angular/router';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
 /*  @Input() usersFromHomeComponent:any; */
  @Output() cancelRegister= new EventEmitter();
  /* model:any={}; (L149) */
  registerForm: FormGroup;
  maxDate:Date;
  validationErrors:string[]=[];
 /*  constructor(private accountService:AccountService, private toastr:ToastrService) { } L(145)*/
  /* constructor(private accountService:AccountService, private toastr:ToastrService, private fb:FormBuilder) { } L(149) */
  constructor(private accountService:AccountService, private toastr:ToastrService, 
    private fb:FormBuilder, private router:Router) { } 
 
  ngOnInit(): void {
    this.initializeForm();
    this.maxDate=new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear()-18)

  }
  
 /* (L145) initializeForm()
  {
    this.registerForm=new FormGroup({
     // username:new FormControl(),
      password:new FormControl(),
      confirmPassword:new FormControl() (L141)// 
      username: new FormControl( //'Hello'(L143) //'',Validators.required),
      password: new FormControl('',[Validators.required, Validators.minLength(4),
      Validators.maxLength(8)]),
     //  confirmPassword: new FormControl('',Validators.required) (L142) //
      confirmPassword: new FormControl('',[Validators.required,this.matchValues('password')])
    });
  }  (l145)*/
  /* initializeForm()
  {
    this.registerForm=this.fb.group({
    
      username: ['',Validators.required],

      password: ['',[Validators.required, Validators.minLength(4),
      Validators.maxLength(8)]],
      
      confirmPassword: ['',[Validators.required,
      this.matchValues('password')]]
    });
  } (L146) */
initializeForm()
  {
    this.registerForm=this.fb.group({
    
      gender: ['male'],
      username: ['',Validators.required],
      knownAs: ['',Validators.required],
      dateOfBirth: ['',Validators.required],
      city: ['',Validators.required],
      country: ['',Validators.required],
      password: ['',[Validators.required, Validators.minLength(4),
      Validators.maxLength(8)]],
      
      confirmPassword: ['',[Validators.required,
      this.matchValues('password')]]
    });
  }

  matchValues(matchTo:string):ValidatorFn
  {
      
    /*  Stack OverFlow:return control.value === (control.parent.controls as { [key: string]: AbstractControl })[matchTo].value ? null : { isMatching: true }; */
  
    /* return (control:AbstractControl)=>
    {return control?.value === (control?.parent?.controls as 
      {[key: string]: AbstractControl })[matchTo].value? null
    :{isMatching:true}
    } */
    return (control:AbstractControl)=>{
     const controls = control?.parent?.controls as {[key: string]: AbstractControl };
      let matchToControl=null;
      if(controls) matchToControl=controls[matchTo];
      return control?.value===matchToControl?.value? null:{isMaching:true};

    }
  }
  register()
  {
    /*this.accountService.register(this.registerForm.value// (this.model)(L149) 
      ).subscribe(response=>
      { // console.log(response); 
      this.cancel();  (L149) //
      this.router.navigateByUrl('/members');
      },error=>{//console.log(error); this.toastr.error(error.error); (L149)
                this.validationErrors=error;
               }
      )*/
    
    this.accountService.register(this.registerForm.value).subscribe({
      next: response=>{this.router.navigateByUrl('/members')},
      error: error => {this.validationErrors=error}
    });
   
   
   
   
   
    /* this.accountService.register(this.model).subscribe(response=>
      {console.log(response);
      this.cancel();
    },error=>{console.log(error);
      this.toastr.error(error.error)})
    console.log(this.model) */
   /* this.accountService.register(this.model).subscribe(response=>
      { console.log(response);
      this.cancel();
    },error=>{console.log(error);this.toastr.error(error.error);}
      )
    console.log(this.model)   L(140)*/
     
    /* console.log(this.model)  */  /* otkomentarisan kod iz L(140)*/

    /* console.log(this.registerForm.value); (L149) */
  
  /*         this.accountService.register(this.registerForm.value).subscribe({
      next: (response: any) => {this.users = response},
      error: (error: any) => {console.log(error)}
    });                                   */ 
  
  
  }
  cancel() {

   /*  console.log("canceled") */
    this.cancelRegister.emit(false);

  }
}

