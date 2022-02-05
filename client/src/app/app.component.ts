import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
/*export class AppComponent*/ export class AppComponent implements OnInit {
  title = 'The Dateing App';
  users:any;
  constructor(private  http:HttpClient/*private accountService:AccountService*/){}
  ngOnInit() /*void*/ {
    /*throw new Error('Method not implemented.');formiran interfejs za liniju 9*/
  
  this.getUsers();
  
  }
  getUsers()
  
  {

    this.http.get('https://localhost:5001/api/users').subscribe({
      next: (response: any) => {this.users = response},
      error: (error: any) => {console.log(error)}
    });


   }

}
function getUsers() {
  throw new Error('Function not implemented.');
}

