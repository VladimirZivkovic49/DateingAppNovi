import { HttpClient,/* , HttpHeaders */ 
HttpParams} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of, reduce, take } from 'rxjs';
/* import { Observable } from 'rxjs'; */
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
/* const httpOptions=
{
  
  headers: new HttpHeaders(
   //mora se dopisati " || '{}'"
    
   
   {Authorization:'Bearer '+ JSON.parse(localStorage.getItem('user') || '{}' )?.token})

} */

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl=environment.apiUrl;
  members:Member[] =[];
  /*(L159) */
  //(L166)
  memberCache=new Map();
  //(L166)
  //(L168)
   user: User| null;
   userParams:UserParams;
  //(L168)
  //(L168)
   // constructor(private http:HttpClient){}
      constructor(private http:HttpClient,private accountService:AccountService)
   {
    this.accountService.currentUser$.pipe(take(1)).subscribe
    ( user=>
      {
          this.user=user;
          this.userParams=new UserParams(user);
      }



    )
  
   }
 getUserParams()
 {
    return this.userParams;
 } 
setUserParams(params: UserParams)
{
  this.userParams=params;
}
resetUserParams()
{
this.userParams=new UserParams(this.user);
return this.userParams;
}

   //(L168)
  
  
  
  /* getMembers():Observable<Member[]>
  {
    return this.http.get<Member[]>(this.baseUrl+ 'users', httpOptions)
  } */
  /* getMembers()
  {
        
    return this.http.get<Member[]>(this.baseUrl+ 'users' )// ,httpOptions
  } */

 /*  getMembers(page?:number,itemsPerPage?:number) (L159) */
      getMembers(userParams:UserParams)
     { 
    //(L166)
       // console.log(Object.values(userParams).join('-')); 
      var response = this.memberCache.get(Object.values(userParams).join('-'))
      if(response)
      {
        return of(response);

      }
    //(L166)
    
    
      //(L159)
      let params=this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);
      params=params.append('minAge', userParams.minAge.toString());
      params=params.append('maxAge', userParams.maxAge.toString());
      params=params.append('gender', userParams.gender.toString());
     //(L164)
      params=params.append('orderBy', userParams.orderBy.toString());
    //(L164)
      /*  return this.getPaginatedResult<Member[]>(this.baseUrl + 'users',params); */
   /*  return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params); */
    //(L166)
    return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params)
    .pipe(map(response=>
      {
        this.memberCache.set(Object.values(userParams).join('-'),response);
        return response;
      }
      
            ));
    //(166)
    //(L159)
 /*    let params=new HttpParams;
    if((page!==null && page!==undefined ) && (itemsPerPage!==null && itemsPerPage!==undefined))
{
  params=params.append('pageNumber', page.toString());
  params=params.append('pageSize', itemsPerPage.toString());


} (L159)*/
    /* if(this.members.length>0)
     {
        return of(this.members)

     }  */
   
   /*  let params = new HttpParams();
    if((page!==null && page!==undefined ) && (itemsPerPage!==null && itemsPerPage!==undefined)) (L159)
   
   {
    
    params=params.append('pageNumber', page.toString());
    params=params.append('pageSize', itemsPerPage.toString());
   }*/
   
    /*  return this.http.get<Member[]>(this.baseUrl+ 'users', {observe: 'response', params } ).pipe(      /*  return this.http.get<Member[]>(this.baseUrl + 'users', {observe: 'response', params }).pipe( (L159a) */
       /*  map(members=>{
          this.members=members;
          return members;

        } (L155))*/
        
        
       //(L159)  map(response=>{
          
        //(L159)  this.paginatedResult.result=response.body;
         //(L159) if(response.headers.get('Pagination')!=null)
         // {   
            /* this.paginatedResult.pagination=JSON.parse(reponse.headers.get('Pagination'));
           
           Greška:
            (parameter) reponse: HttpResponse<Member[]>
            Argument of type 'string | null' is not assignable to parameter of type 'string'.
            Type 'null' is not assignable to type 'string'.ts(2345)
            Rešenje  : "||'{}'"
            
            */
          //(L159)  this.paginatedResult.pagination=JSON.parse(response.headers.get('Pagination')||'{}');
          
         // }
       //(L159)     return this.paginatedResult;
      //  })
    //) 
  }
 private getPaginatedResult<T>(url: string,params: HttpParams)
 {
  const paginatedResult:PaginatedResult<T> = new PaginatedResult<T>();
  return this.http.get<T>(url  /* 'users' */, {observe: 'response', params } ).pipe(      
  map(response=>{
   paginatedResult.result=response.body;
    if(response.headers.get('Pagination')!=null)
    {
      paginatedResult.pagination=JSON.parse(response.headers.get('Pagination')||'{}');
    }
    return paginatedResult;
   })
  )
 }
 //(L159)
  /* private getPaginatedResult<T>(url: string,params: HttpParams)
  { */
 /*  const paginatedResult:PaginatedResult<T> = new PaginatedResult<T>();
  return this.http.get<T>(url, {observe: 'response', params }).pipe( 
          map(reponse=>{
          
          paginatedResult.result=reponse.body;
          if(reponse.headers.get('Pagination')!==null)
          {  
          paginatedResult.pagination=JSON.parse(reponse.headers.get('Pagination')||'{}');
          
          }
            return paginatedResult;
        })
    ) 
  } */
  //(L159)
 //(L159)
   private getPaginationHeaders(pageNumber:number, pageSize: number)
  {
   let params = new HttpParams();
 
     var aa=pageNumber.toString();
     var bb=pageSize.toString();
    params=params.append('pageNumber', aa);
    params=params.append('pageSize',bb );
    return params;

 
/* let params=new HttpParams;
if((page!==null && page!==undefined ) && (itemsPerPage!==null && itemsPerPage!==undefined))
{
params=params.append('pageNumber', page.toString());
params=params.append('pageSize', itemsPerPage.toString());


} */

} 

  
 //(L159)
 /* getMember(username: string | null)*///obavezno proveriti kada je javlja error ts2345
                                    //U ovom slučaju dodat "| null"
  /*{
    
    return this.http.get<Member>(this.baseUrl+ 'users/'+ username*//* , httpOptions *//*)
  }*/
  getMember(username: string | null)
  {
    /* const member=this.members.find(x=>x.username===username)
    if(member!==undefined)
    {
      return of(member)

    } (L167) */
    //(L167)
    //console.log(this.memberCache);
    const member=[...this.memberCache.values()]
    .reduce((arr,elem)=>arr.concat(elem.result),[])
    .find((member:Member)=>member.username===username);
    if(member)
    {

    return of(member);

    }
   //console.log(member);
    //(L167)
    return this.http.get<Member>(this.baseUrl+ 'users/'+ username);

  }



  /* updateMember(member:Member)
  {
    return this.http.put(this.baseUrl +'users',member);

  } */
  updateMember(member:Member)
  {
    return this.http.put(this.baseUrl +'users',member).pipe(

      map(()=>{

        const index=this.members.indexOf(member);
        this.members[index]=member;
      })


    );

  }

    setMainPhoto(photoId:number)
    {
      return this.http.put(this.baseUrl+'users/set-main-photo/'+photoId,{});


    }

    deletePhoto(photoId:number)
    {
      return this.http.delete(this.baseUrl+'users/delete-photo/'+photoId,{});

    }
//(L175)
addLike(username:string)
{
return this.http.post(this.baseUrl + 'likes/'+ username,{});// {} emty body for empty object

}

/* getLikes(predicate:string|any)
{
return this.http.get(this.baseUrl + 'likes?=', predicate);

} */
//(L176)
/* getLikes(predicate:string)
{
return this.http.get<Partial<Member[]>>(this.baseUrl + 'likes?predicate='+ predicate);

}  */
//(178)

getLikes(predicate:string, pageNumber:number, pageSize: number)
{
  /* let params=this.getPaginationHeaders(pageNumber, pageSize); */
  let params = new HttpParams();
  params= params.append('predicate',predicate);
  params= params.append('pageNumber',pageNumber.toString());
  params= params.append('pageSize',pageSize.toString());
  return this.getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'likes', params);


}  

//(178)
//(L176)
//(L175)
  }
