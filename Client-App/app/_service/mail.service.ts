import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Auth } from '../_model/auth';
import { Notify } from '../_model/notify';

@Injectable({
  providedIn: 'root'
})
export class MailService {

  private readonly url: string = environment.baseUrl;

  constructor(private http: HttpClient) { }

  public codeRequest(auth: Auth): Observable<Auth> {
    return this.http.post<Auth>(`${this.url}api/Mail/CodeRequest`, auth);
  }

  public newContent(notify: Notify) {
    return this.http.post<Notify>(`${this.url}api/Mail/NewContent`, notify);
  }

  public roundDeadline(notify: Notify) {
    return this.http.post<Notify>(`${this.url}api/Mail/RoundDeadline`, notify);
  }
}
