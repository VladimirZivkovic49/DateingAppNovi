import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { PresenceService } from './presence.service';
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  /* baseUrl='https://localhost:5001/api/'; */
  baseUrl=environment.apiUrl;
  private currentUserSource=new ReplaySubject<User |null>(1);
  currentUser$=this.currentUserSource.asObservable();
  /* constructor(private http:HttpClient) { } (L222) */

  //(L222)
  constructor(private http:HttpClient,private presence:PresenceService){}
  //(L222)
  login(model:/* any */User) 
  {
   /*  return this.http.post(this.baseUrl +'account/login', model); */
  
    return this.http.post<User>(this.baseUrl +'account/login', model).pipe(
    map((response:User ) => 
    {
     const user = response;
      if(user)  
       {
             this.setCurrentUser(user);  
           //(L222)
              this.presence.createHubConnection(user);

           //(L222)
             /* localStorage.setItem('user',JSON.stringify(user));
             this.currentUserSource.next(user);  (L134)*/
       }
    })

    )

  }
  register(model:any) 
  {
   return this.http.post<User>(this.baseUrl +'account/register', model).pipe(
    map((user:User ) => 
    {
     
      if(user)  
       {
             this.setCurrentUser(user);   
           //(L222)
           this.presence.createHubConnection(user);

           //(L222)
           
             /*  localStorage.setItem('user',JSON.stringify(user)); 
             this.currentUserSource.next(user);(L134) */
       }
    return user;
    
      })

   )
  
  }
  
  
  
    setCurrentUser(user: User|any) {
    //(L212)
      user.roles =[] ;
      const roles= this.getDecodedToken(user.token).role;
      Array.isArray(roles) ? user.roles  = roles : user.roles?.push(roles);
    //(L212)
    
      localStorage.setItem('user',JSON.stringify(user));
      this.currentUserSource.next(user);

  }

  logout()
  {
      localStorage.removeItem('user');
      
      this.currentUserSource.next(null);
   //(L222)
   this.presence.stopHubConnection();

   //(L222)
  }
  getDecodedToken(token : string)
  {
    return JSON.parse(atob(token.split(".")[1]));

  }

}


