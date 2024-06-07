import { HttpClient, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { User } from '../_model/user';

@Injectable({
  providedIn: 'root'
})
export class PicService {

  private readonly url: string = environment.baseUrl;

  constructor(private http: HttpClient) { }

  //public userPic(user: User) : Observable<string> {
  //  return this.http.get<string>(`${this.url}api/Pic/UserPic?userVm=${user}`)
  //}

  public upload(file: File, api: string, id: number)
  {
    if (file === null)
      return;

    const fd: FormData = new FormData();
    fd.append('file', file, file.name);
    return this.http.post<User>(`${this.url}api/Pic/${api}?id=${id}`, fd, { observe: 'events' })
      .subscribe(e => {
        if (e.type === HttpEventType.Response) {
          console.log(file + ' uploaded');
        }
      }, (err: any) => { console.log(err); });
  }
}
