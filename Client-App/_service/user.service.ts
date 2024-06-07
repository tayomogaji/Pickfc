import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { tokenGetter } from '../app.module';
import { User } from '../_model/user';
import { Auth } from '../_model/auth';
import { Jwt } from '../_model/jwt';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly url: string = environment.baseUrl;

  constructor(private http: HttpClient) { }

  public user(id: number): Observable<User> {
    return this.http.get<User>(`${this.url}api/User/User?id=${id}`);
  }

  public currentUser(): Observable<User> {
    return this.http.get<User>(`${this.url}api/User/CurrentUser`, { headers: new HttpHeaders({Authorization: `Bearer${tokenGetter()}`})})
  }

  public viaCode(code: string): Observable<User> {
    return this.http.get<User>(`${this.url}api/User/ViaCode?code=${code}`);
  }

  public viaEmail(email: string): Observable<User> {
    return this.http.get<User>(`${this.url}api/User/ViaEmail?email=${email}`);
  }

  public users(): Observable<User[]> {
    return this.http.get<User[]>(`${this.url}api/User/Users`);
  }

  public exist(email: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}api/User/Exist?email=${email}`);
  }

  public verifiedExist(code: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}api/User/VerifiedExist?code=${code}`);
  }

  public login(auth: Auth): Observable<Jwt> {
    return this.http.post<Jwt>(`${this.url}api/User/Login`, auth, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    });
  }

  public facebook(cred: string): Observable<any> {
    return this.http.post(`${this.url}api/User/Facebook`, JSON.stringify(cred), { headers: new HttpHeaders({ 'Content-Type': 'application/json' }), withCredentials: true });
  }

  public google(cred: string): Observable<any> {
    return this.http.post(`${this.url}api/User/Google`, JSON.stringify(cred), { headers: new HttpHeaders({ 'Content-Type': 'application/json' }), withCredentials: true });
  }

  public addedit(user: User): Observable<User> {
    return user.id === 0 ? this.http.post<User>(`${this.url}api/User/Add`, user) : this.http.put<User>(`${this.url}api/User/Edit`, user);
  }

  public delete(id: number): Observable<User> {
    return this.http.delete<User>(`${this.url}api/User/Delete?id=${id}`);
  }

  public verify(auth: Auth): Observable<Auth> {
    return this.http.post<Auth>(`${this.url}api/User/Verify`, auth);
  }

  //public admin(): Observable<boolean> {
  //  return this.http.get<boolean>(`${this.url}api/User/Admin`);
  //}

}
