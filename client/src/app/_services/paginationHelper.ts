//(L187)
/*private getPaginatedResult<T>(url: string,params: HttpParams)
{
 const paginatedResult:PaginatedResult<T> = new PaginatedResult<T>();
 return this.http.get<T>(url  // 'users'(zakomentarisano) , {observe: 'response', params } ).pipe(      
 map(response=>{
  paginatedResult.result=response.body;
   if(response.headers.get('Pagination')!=null)
   {
     paginatedResult.pagination=JSON.parse(response.headers.get('Pagination')||'{}');
   }
   return paginatedResult;
  })
 )
}*/
/*private getPaginationHeaders(pageNumber:number, pageSize: number)
{
 let params = new HttpParams();

   var aa=pageNumber.toString();
   var bb=pageSize.toString();
  params=params.append('pageNumber', aa);
  params=params.append('pageSize',bb );
  return params;


//početak komentara        let params=new HttpParams;
        if((page!==null && page!==undefined ) && (itemsPerPage!==null && itemsPerPage!==undefined))
        {
        params=params.append('pageNumber', page.toString());
        params=params.append('pageSize', itemsPerPage.toString());


        } //krajkomentara

} */

import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs";
import { PaginatedResult } from "../_models/pagination";

export function getPaginatedResult<T>(url: any, params: any, http:HttpClient)
{
 const paginatedResult:PaginatedResult<T> = new PaginatedResult<T>();
 return http.get<T>(url   /* 'users' */ , {observe: 'response', params } ).pipe(      
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
export function  getPaginationHeaders(pageNumber:number, pageSize: number)
{
 let params = new HttpParams();

   var aa=pageNumber.toString();
   var bb=pageSize.toString();
  params=params.append('pageNumber', aa);
  params=params.append('pageSize',bb );
  return params;
}

//(L187)