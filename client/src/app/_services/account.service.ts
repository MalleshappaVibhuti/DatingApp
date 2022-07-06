import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../_modules/user';
import { map } from 'rxjs/operators';
import { ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AccountService {
baseUrl = environment.apiUrl;
private currentUserSource=new ReplaySubject<User>(1);
cussrentUser$ = this.currentUserSource.asObservable();
  constructor(private http:HttpClient) { }

  login(model:any){
    return this.http.post(this.baseUrl + "account/login", model).pipe(
      map((response:User)=>{
        const user=response;
        if(user){
          this.setCurrentUsr(user);
        }        
      })
    )
  }
  register(model:any){
    return this.http.post(this.baseUrl+'account/register',model).pipe(
     map((user:User)=>{
      if(user){
         this.setCurrentUsr(user);
      }
     })
    )    
  }

  setCurrentUsr(user:User){
    localStorage.setItem("user",JSON.stringify(user));
    this.currentUserSource.next(user);
  }
  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
